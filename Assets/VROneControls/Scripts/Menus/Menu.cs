using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Creates a menu based on the settings
/// </summary>
public class Menu : MonoBehaviour {
    [System.Serializable]
    public class Row
    {
        public List<Icon> icons = new List<Icon>();
    }

    [System.Serializable]
    public class Icon
    {
        public Material mat;
        public Menu subMenuPrefab;
        public bool isBackButton = false;
		public bool isCloseButton = false;

		[System.Serializable]
		public class Callback{
			public MenuCallback component;
			public string[] info;
		}

		//You can define a condition whether the first or second state should be shown or if the object should be hidden completely
		[System.Serializable]
		public class Condition{
			public MenuCondition component;
			public string[] info;
		}

		/// <summary>
		/// When this icon is selected, the second state icon is revealed.
		/// </summary>
		public float zOffset = 0;

		public Callback callback = new Callback();

		public Condition condition = new Condition();

		public string name = "default";

        public string[] tooltip;

		public bool useSecondState = false;
		public SubIcon secondState;

		public bool showInIOS = true;
		public bool showInAndroid = true;

		public Icon()
		{}

        public Icon(Material mat, Menu subMenuPrefab, bool isBackButton = false, bool isCloseButton = false, string[] tooltip = null, string name = "default")
        {
            this.mat = mat;
            this.subMenuPrefab = subMenuPrefab;
            this.isBackButton = isBackButton;
			this.isCloseButton = isCloseButton;
            this.tooltip = tooltip;
			this.name = name;
        }


		//This function has to be used because Unity cannot show variables referring to the same class they belong to in the inspectorsky.
		[System.Serializable]
		public class SubIcon : Icon{};
    }

    public List<Row> rows;


    public float distance = 1.1f;

	//Spherical settings
    public float iconRotation = 12f;
	//
    public float iconSize = 0.18f;

	public float iconMargin = 0.03f;

    public Vector3 menuOffset = new Vector3(0, -0.5f, 0);

	public static float MenuPosition(int i, int count)
	{
		return (-((count * 0.5f) - 0.5f) + i);
	}

	bool IconShown(Icon icon)
	{
#if UNITY_IOS
		return !icon.showInIOS;
#endif
#if UNITY_ANDROID
		return !icon.showInAndroid;
#endif
#if UNITY_EDITOR
		return false;
#endif
	}

    public virtual void CreateMenu()
    {
        //AddBackButton();
		AddBackAndCloseButton ();
        int rowIterator = 0;
        foreach (Row row in rows)
        {
			//Remove all icons not shown on a specific platform
			row.icons.RemoveAll(IconShown);
            int i = 0;
            foreach (Icon icon in row.icons)
            {
				float posX = MenuPosition(i, row.icons.Count);
				float posY = rowIterator;
                CreateIcon(icon, new Vector2(posX, posY), new Vector3(iconSize, iconSize, iconSize));
                i++;
            }
            rowIterator++;
        }
        transform.localScale = new Vector3(distance, distance, distance);
    }

	public enum MenuLayout { Spherical, Cylindrical, Plane};

	public MenuLayout menuLayout = MenuLayout.Cylindrical;

	[System.Serializable]
	public class SoundEffects
	{
		public bool useSound = true;
		public AudioClip hover, select;
	}

	public SoundEffects soundEffects;

	/// <summary>
	/// Adds a back and close button on top of the menu
	/// </summary>
	void AddBackAndCloseButton()
	{
		Vector2 pos = new Vector2 (-0.5f, -0.6f);
		Vector3 scale = new Vector3 (iconSize * 0.5f, iconSize * 0.5f, iconSize * 0.5f);
		//Show a back button if you are not in a sub menu
		if (UIController.Instance.menuStack.Count > 0)
		{
			Icon backIcon = new Icon(Resources.Load("backButtonMat") as Material, null, true, false, null, "back");
			backIcon.zOffset = 0.1f;
			GameObject backObject = CreateIcon (backIcon, pos, scale);
			//offset the back icon to be behind the normal items
		}
		//Show the close button
		Icon closeIcon = new Icon(Resources.Load("closeButtonMat") as Material, null, false, true, null, "close");
		closeIcon.zOffset = 0.1f;
		pos.x = 0.5f;
		CreateIcon (closeIcon, pos, scale);

	}

	/// <summary>
	/// Deprecated: Adds the back button as menu icon. Use addbackandclosebutton instead
	/// which positions the two button separately above the other buttons.
	/// </summary>
    void AddBackButton()
    {
        rows[rows.Count - 1].icons.Add(new Icon(Resources.Load("backButtonMat") as Material, null, true));
    }

    GameObject CreateIcon(Icon icon, Vector2 pos, Vector3 scale)
    {
        GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        quad.renderer.material = icon.mat;
        quad.transform.parent = transform;
        quad.transform.localPosition = menuOffset;
		quad.name = icon.name;
		if (menuLayout == MenuLayout.Spherical) {
			float rotX = pos.x * iconRotation;
			float rotY = pos.y * iconRotation;
			quad.transform.rotation = Quaternion.Euler (new Vector3 (rotY, rotX, 0));
			quad.transform.Translate (new Vector3 (0, 0, 1), Space.Self);
			quad.transform.LookAt (transform.position + menuOffset);
			quad.transform.Rotate (new Vector3 (0, 180, 0));
		}
		else if (menuLayout == MenuLayout.Cylindrical)
		{
			float rotX = pos.x * iconRotation;
			quad.transform.rotation = Quaternion.Euler (new Vector3 (0, rotX, 0));
			quad.transform.Translate (new Vector3 (0, -pos.y * (iconSize + iconMargin), 1), Space.Self);
			//quad.transform.LookAt (transform.position + menuOffset);
			//quad.transform.Rotate (new Vector3 (0, 180, 0));
		}
		quad.transform.localScale = scale;
        MenuIcon mi = quad.AddComponent<MenuIcon>();
        mi.tooltip = icon.tooltip;
        mi.subMenuPrefab = icon.subMenuPrefab;
        mi.isBackButton = icon.isBackButton;
		mi.isCloseButton = icon.isCloseButton;
		mi.callback = icon.callback;
		mi.condition = icon.condition;
		mi.zOffset = icon.zOffset;
		mi.parentMenu = this;

		if (icon.useSecondState)
		{
			GameObject subIcon = CreateIcon(icon.secondState, pos, scale);
			mi.secondState = subIcon;
			subIcon.GetComponent<MenuIcon>().secondState = quad;
			mi.secondState.SetActive(false);
		}
		return quad;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
