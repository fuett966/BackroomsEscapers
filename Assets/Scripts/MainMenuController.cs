using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject hostButton;

    [SerializeField]
    private GameObject startMenuPanel;

    [SerializeField]
    private GameObject multiplayerMenuPanel;

    [SerializeField]
    private GameObject settingsMenuPanel;

    [SerializeField]
    private GameObject lobbiesMenuPanel;

    public void SwitchStartMenuPanel()
    {
        startMenuPanel.SetActive(!startMenuPanel.activeSelf);
        //Close other menus
    }

    public void SwitchMultiplayerMenuPanel()
    {
        multiplayerMenuPanel.SetActive(!multiplayerMenuPanel.activeSelf);
        //Close other menus
    }

    public void SwitchSettingsMenuPanel()
    {
        settingsMenuPanel.SetActive(!settingsMenuPanel.activeSelf);
        //Close other menus
    }

    public void SwitchLobbiesMenuPanel()
    {
        lobbiesMenuPanel.SetActive(!lobbiesMenuPanel.activeSelf);
        //Close other menus
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
