using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenusManager : MonoBehaviour
{
    public static MenusManager Instance;

    [SerializeField]
    Camera mainCamera, UICamera;

    [SerializeField]
    GameObject[] menus; // 0 is reserved for the game

    GameObject activeMenu
    {
        get
        {
            return activeMenu;
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
    public void ChangeMenu(int menuIndex)
    {
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
}
