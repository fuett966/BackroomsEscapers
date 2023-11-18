using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Assertions.Must;
using TMPro;

public class SteamLobby : MonoBehaviour
{
    public static SteamLobby Instance;

    // Callbacks
    protected Callback<LobbyCreated_t> LobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> JoinRequest;
    protected Callback<LobbyEnter_t> LobbyEntered;

    // Variables
    public ulong CurrentLobbyID;
    private const string HostAddressKey = "HostAddress";
    private CustomNetworkManager customNetworkManager;

    //Gameobject


    private void Start()
    {
        if (!SteamManager.Initialized)
        {
            return;
        }
        if (Instance == null)
        {
            Instance = this;
        }

        customNetworkManager = GetComponent<CustomNetworkManager>();
        Debug.Log("Manager Initialized");

        LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        JoinRequest = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
    }

    public void HostLobby()
    {
        SteamMatchmaking.CreateLobby(
            ELobbyType.k_ELobbyTypeFriendsOnly,
            customNetworkManager.maxConnections
        );
    }

    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK)
        {
            return;
        }

        Debug.Log("Lobby created Succesfully");

        customNetworkManager.StartHost();

        SteamMatchmaking.SetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            HostAddressKey,
            SteamUser.GetSteamID().ToString()
        );
        SteamMatchmaking.SetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            "name",
            SteamFriends.GetPersonaName().ToString() + "'S LOBBY"
        );
    }

    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        Debug.Log("Request To Join Lobby");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        //Everyone
        CurrentLobbyID = callback.m_ulSteamIDLobby;

        //clients
        if (NetworkServer.active)
        {
            return;
        }

        customNetworkManager.networkAddress = SteamMatchmaking.GetLobbyData(
            new CSteamID(callback.m_ulSteamIDLobby),
            HostAddressKey
        );
        customNetworkManager.StartClient();
    }
}
