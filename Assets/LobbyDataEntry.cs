using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Steamworks;
using TMPro;

public class LobbyDataEntry : MonoBehaviour
{
    //Data

    public CSteamID lobbyID;
    public string lobbyName;
    public TextMeshProUGUI lobbyNameTMP;

    public void SetLobbyData()
    {
        if (lobbyName == "")
        {
            lobbyNameTMP.text = "Empty";
        }
        else
        {
            lobbyNameTMP.text = lobbyName;
        }
    }

    public void JoinLobby()
    {
        SteamLobby.Instance.JoinLobby(lobbyID);
    }
}
