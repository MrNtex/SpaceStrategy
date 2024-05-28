using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenusManager : MonoBehaviour
{
    public static MenusManager Instance;

    [SerializeField]
    Camera mainCamera, UICamera;

    [SerializeField]
    GameObject[] menus, modals; // 0 is reserved for the game

    public delegate void ChangedMenu();
    public event ChangedMenu OnChangedMenu;

    GameObject activeMenu
    {
        get
        {
            return _activeMenu;
        }
        set
        {
            if (_activeMenu != null)
            {
                _activeMenu.SetActive(false);
            }
            _activeMenu = value;
            if (_activeMenu != null)
            {
                _activeMenu.SetActive(true);
            }
        }
    }
    GameObject _activeMenu;

    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        ChangeMenu(0);
    }
    public void ChangeMenu(int menuIndex)
    {
        if(menuIndex >= menus.Length || menuIndex < 0)
        {
            Debug.LogError("Menu index out of range");
            return;
        }

        if(activeMenu == menus[menuIndex])
        {
            return;
        }
        activeMenu = menus[menuIndex];

        if (menuIndex == 0)
        {
            mainCamera.gameObject.SetActive(true);
            UICamera.gameObject.SetActive(false);
        }
        else
        {
            mainCamera.gameObject.SetActive(false);
            UICamera.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close();
        }
    }

    public void Close()
    {
        foreach (GameObject modal in modals)
        {
            if (modal.activeSelf)
            {
                modal.SetActive(false);
            }
        }
        ChangeMenu(0);
    }
}
