using UnityEngine;
using System.Collections;

/// <summary>
/// This class defines an object that can be selected by the cursor and opens a menu.
/// </summary>
public class SelectableObject : MonoBehaviour {
    /// <summary>
    /// Determines which menu prefab is opened.
    /// </summary>
    public Menu menuPrefab;

	/// <summary>
	/// Sets this menu active on start.
	/// </summary>
	public bool menuActiveOnStart;

	public float selectionTime = 0;

	public GameObject progressBar;

	void Start()
	{
		if (progressBar) progressBar.SetActive(false);
		if (menuActiveOnStart)
		{
			//Inform the ui controller about the start menu
			UIController.Instance.SetMenu(menuPrefab.gameObject);
		}
	}

	public void SetProgress(float factor)
	{
		if (!progressBar) return;
		if (!progressBar.activeSelf)
		{
			if (menuPrefab.soundEffects.useSound && menuPrefab.soundEffects.hover)
			{
				AudioSource.PlayClipAtPoint(menuPrefab.soundEffects.hover, transform.position);
			}
		}
		progressBar.SetActive (true);
		progressBar.renderer.material.SetFloat ("_PercentageFilled", factor);
	}
	public bool useSound = false;

	public void SelectionEvent()
	{
		if (!useSound) return;
		if (menuPrefab.soundEffects.useSound && menuPrefab.soundEffects.select)
		{
			AudioSource.PlayClipAtPoint(menuPrefab.soundEffects.select, transform.position);
		}
	}
}
