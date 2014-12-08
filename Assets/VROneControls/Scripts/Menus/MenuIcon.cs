using UnityEngine;
using System.Collections;

/// <summary>
/// Identifies this object as menu icon
/// </summary>
public class MenuIcon : MonoBehaviour {
    //Constants
    const float standardTransparency = 0.15f;
    const float filledTransparency = 0.1f;
	const float moveForwardLength = 0.03f;



    public static MenuIcon selectedItem;

	public Menu.Icon.Callback callback;

	/// <summary>
	/// Can be used to position a menu icon behind the others (e.g. used for the back and close button)
	/// </summary>
	public float zOffset = 0f;

	public Menu.Icon.Condition condition;

    public string[] tooltip = null;

    public Menu subMenuPrefab;

	Vector3 standardPosition;

	/// <summary>
	/// When this icon is selected, the second state icon is revealed.
	/// </summary>
	public GameObject secondState;

    void Start()
    {
		SetStandardAndMovedForwardPosition ();
		TestCondition ();
    }

	void SetStandardAndMovedForwardPosition()
	{
		standardPosition = transform.localPosition;
	}

    public bool isBackButton = false;
	public bool isCloseButton = false;

    float timeSelected = 0;

    const float selectTimer = 1.4f;

	bool isSelected;

	/// <summary>
	/// If this flag is set, the icons show the defined text in a tag above the icon.
	/// </summary>
	protected bool showTag = true;


	void UpdateShader()
	{
		renderer.material.color = (objectSelectable ? Color.white : Color.gray);
	}

    public void Update()
    {
        if (selectedItem == this && objectSelectable)
        {
            if (timeSelected >= selectTimer)
            {
				TriggerAction();
            }
			//If the object hasnt been selected in the last frame, play sound
			if (!isSelected) PlaySound (parentMenu.soundEffects.hover);
            isSelected = true;
            timeSelected += Time.deltaTime;
            timeSelected = Mathf.Min(selectTimer, timeSelected);
        }
        else
        {
            timeSelected = 0;
			ShowTagObject(false);
            isSelected = false;
        }
		ChangeIconShader ();
		TestCondition ();
		if (showTag) ShowTag ();
		//Set color of item depending on whether it is selectable or not
		UpdateShader ();
    }


	void ShowTag()
	{
		if (tooltip == null || tooltip.Length == 0) return;
		ShowTagObject(MenuIcon.selectedItem == this);
	}

	GameObject tagObject;

	void ShowTagObject(bool value)
	{
		//If a tag object already exists, show or hide it
		if (tagObject)
		{
			tagObject.SetActive(value);
			return;
		}
		//If there is no tag object and the tag object shall not be shown, do nothing
		if (!tagObject && !value) return;
		//otherwise create a new tag object

		//Create the background
		GameObject background = GameObject.CreatePrimitive(PrimitiveType.Quad);
		/*background.renderer.material = Resources.Load("EmptyButtonMat") as Material;
		background.renderer.material.SetFloat("_Transparency", 0.8f);
		background.renderer.material.SetFloat("_FilledTransparency", 0);*/
		background.renderer.material.color = Color.black;
		background.transform.parent = transform;
		background.transform.localRotation = Quaternion.identity;
		background.transform.localPosition = Vector3.zero;
		int additionalLines = Mathf.Max (0, tooltip.Length - 1);
		//background.transform.Translate(new Vector3(0,0.5f + (0.2f + additionalLines * 0.1f) * transform.localScale.y,-0.01f), Space.Self);
		background.transform.localPosition = new Vector3(0,0.5f + (0.2f + additionalLines * 0.1f),0);
		background.transform.localScale = new Vector3(1, 0.4f + additionalLines * 0.2f, 0.2f);



		//When the parent object is scaled, the tooltip would be scaled differently as well. This variable redoes the parent scale.
		float backgroundScaleRatio = background.transform.localScale.x / background.transform.localScale.y;

		if (tooltip != null)
		{
			for (int i = 0; i < tooltip.Length; i++)
			{
				GameObject tag = (GameObject)Instantiate(Resources.Load("TagPrefab") as GameObject, Vector3.zero, Quaternion.identity);	
				//Vector3 tagPosition = MenuIcon.selectedItem.transform.position + new Vector3(0, 0.1f, 0);
				tag.GetComponent<TextMesh>().text = tooltip[i];
				tag.transform.parent = background.transform;
				float offset = Menu.MenuPosition(i, tooltip.Length);
				tag.transform.localPosition = new Vector3(0,-offset * 0.22f * backgroundScaleRatio,-0.01f);
				tag.transform.localRotation = Quaternion.identity;
				float textScale = 0.11f;
				tag.transform.localScale = new Vector3 (textScale, textScale * backgroundScaleRatio, textScale);
			}
		}
		tagObject = background;
	}

	bool objectSelectable = true;

	void ObjectSelectable(bool value)
	{
		objectSelectable = value;
	}

	/// <summary>
	/// Tests the condition. If there is no condition or the condition is met, switches to the second state.
	/// </summary>
	void TestCondition ()
	{
		if (!condition.component) return;
		switch (condition.component.TestCondition (condition.info))
		{
			case MenuState.Hidden:
				ObjectSelectable(false);
				break;
			case MenuState.ShowSecondState:
				SwitchToSecondState();
				break;
			case MenuState.Show:
				ObjectSelectable(true);
				break;
		}
	}

	void ChangeIconShader()
	{
		float factor = 0;

		float percentage = 1;
		if (selectTimer != 0) percentage = timeSelected / selectTimer;
		if (timeSelected > 0) {
				factor = 0.15f;
		}
		renderer.material.SetFloat ("_Transparency", standardTransparency + factor);
        renderer.material.SetFloat("_FilledTransparency", standardTransparency + factor + filledTransparency);
		renderer.material.SetFloat ("_PercentageFilled", percentage);

		//Set icon
		transform.localPosition = standardPosition;
		transform.Translate (new Vector3 (0, 0, -moveForwardLength * percentage + zOffset));
	}

	[HideInInspector]
	public Menu parentMenu;

	void PlaySound(AudioClip sound)
	{
		//Play sound
		if (parentMenu.soundEffects.useSound && sound)
		{
			AudioSource.PlayClipAtPoint(sound, transform.position, 0.5f);
		}
	}
	/// <summary>
	/// When the icon is clicked, it triggers an action
	/// </summary>
	void TriggerAction()
	{
		PlaySound (parentMenu.soundEffects.select);
		//Call callback
		if (callback != null && callback.component != null)
		{
			callback.component.ReceiveMenuCallback(callback.info);
		}
		if (secondState)
		{
			TestCondition();
			if (!condition.component && secondState)
			{
				SwitchToSecondState();
			}
		}
		if (subMenuPrefab)
		{
			UIController.Instance.OpenSubMenu(subMenuPrefab.gameObject);
		}
		else if (isBackButton)
		{
			UIController.Instance.GoBack();
		}
		else if (isCloseButton)
		{
			UIController.Instance.CloseMenu();
		}
		timeSelected = 0;
	}

	void SwitchToSecondState()
	{
		secondState.SetActive(true);
		gameObject.SetActive(false);
	}
}
