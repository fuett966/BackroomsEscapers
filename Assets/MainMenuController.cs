using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject startMenuPanel;

    [SerializeField]
    private GameObject multiplayerMenuPanel;

    public void SwitchStartMenuPanel()
    {
        startMenuPanel.SetActive(startMenuPanel.activeSelf);
        //Close other menus
    }
}
