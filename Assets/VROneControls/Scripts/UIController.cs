using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles the general functionality of the UI. Is automatically instantiated
/// </summary>
public class UIController : MonoBehaviour {

    private static UIController _instance;

    public static UIController Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            else
            {
                GameObject uiController = new GameObject("UIController");
                _instance = uiController.AddComponent<UIController>();
                return _instance;
            }
        }
    }

    /// <summary>
    /// Stacks the menus if you go into subfolders to enable you to go back.
    /// </summary>
    public List<GameObject> menuStack = new List<GameObject>();

    public GameObject menu {
		get;
		private set;
	}

    /// <summary>
    /// Sets a new menu
    /// </summary>
    /// <param name="menuPrefab">The menu prefab that has to be instantiated</param>
    public void SetMenu(GameObject menuPrefab)
    {
        //Only allow to set a new menu when no other menu is already open
        if (!menu)
        {
            InstantiateMenu(menuPrefab);
        }
    }

    

    /// <summary>
    /// The menu will be positioned relative to the reference object
    /// </summary>
    /// <param name="menu"></param>
    public GameObject reference
    {
        set;
        get;
    }

    private void InstantiateMenu(GameObject menuPrefab)
    {
        if (!reference)
        {
            Debug.LogError("You have to set the reference object before displaying a menu.");
        }
        menu = (GameObject)Instantiate(menuPrefab, Vector3.zero, Quaternion.identity);
		menu.SetActive (true);
        menu.GetComponent<Menu>().CreateMenu();
        menu.transform.position = reference.transform.position;
    }

    public void GoBack()
    {
        Destroy(menu);
        if (menuStack.Count > 0)
        {
            menu = menuStack[menuStack.Count - 1];
			menuStack.RemoveAt(menuStack.Count - 1);
            menu.SetActive(true);
            MenuIcon.selectedItem = null;
        }
    }

	public void CloseMenu()
	{
		Destroy (menu);
		foreach (GameObject obj in menuStack)
		{
			Destroy (obj);
		}
		menuStack.Clear ();
	}

    public void OpenSubMenu(GameObject menuPrefab)
    {
        if (!menu)
        {
            Debug.LogWarning("No menu opened, therefore no sub menu can be opened");
        }
        else
        {
            AddCurrentMenuToStack();
            InstantiateMenu(menuPrefab);
        }
    }

    /// <summary>
    /// Closes the menu, but adds it to the stack, so that it can be restored.
    /// </summary>
    void AddCurrentMenuToStack()
    {
        menuStack.Add(menu);
        menu.SetActive(false);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}


    GameObject tag = null;

}
