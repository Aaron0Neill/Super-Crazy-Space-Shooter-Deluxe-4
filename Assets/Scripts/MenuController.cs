using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Tooltip("These are the menus which will be activated when the user selects the respective button")]
    public GameObject _mainMenu, _instructions, _options, _exit, _levelSelect;

    // Keeps track of the currently active menu
    private GameObject _activeMenu;

    public void Start()
    {
        _activeMenu = _mainMenu;
    }

    public void Play()
    {
        CloseCurrent();
        Open(_levelSelect);
    }

    public void Instructions()
    {
        CloseCurrent();
        Open(_instructions);
    }

    public void Options()
    {
        CloseCurrent();
        Open(_options);
    }

    public void Close()
    {
        Application.Quit();
    }

    public void LoadLevel(int t_level)
    {
        SceneManager.LoadScene(t_level);
    }

    public void Back()
    {
        CloseCurrent();
        Open(_mainMenu);
    }

    private void Open(GameObject _menu)
    {
        _activeMenu = _menu;
        _activeMenu.SetActive(true);
    }

    private void CloseCurrent()
    {
        _activeMenu.SetActive(false);
    }

}