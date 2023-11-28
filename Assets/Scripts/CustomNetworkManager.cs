using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using Steamworks;
using Telepathy;
using System;

public class CustomNetworkManager : NetworkManager
{
    [SerializeField]
    private PlayerObjectController GamePlayerPrefab;

    public List<PlayerObjectController> Gameplayers { get ; } = new List<PlayerObjectController>();

    public event Action OnStartHostEvent;

    public void AddPlayerObject(PlayerObjectController playerObjectController)
    {
        Gameplayers.Add(playerObjectController);
        if(Gameplayers.Count == 1)
        {
            OnStartHostEvent?.Invoke();
        }
    }
    [Server]
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().name == "LobbySteam")
        {
            Debug.Log("Вошло");
            PlayerObjectController GamePlayerInstance = Instantiate(GamePlayerPrefab);

            GamePlayerInstance.ConnectionID = conn.connectionId;
            GamePlayerInstance.PlayerIDNumber = Gameplayers.Count + 1;
            GamePlayerInstance.PlayerSteamID = (ulong)
                SteamMatchmaking.GetLobbyMemberByIndex(
                    (CSteamID)SteamLobby.Instance.CurrentLobbyID,
                    Gameplayers.Count
                );

            NetworkServer.AddPlayerForConnection(conn, GamePlayerInstance.gameObject);
            GamePlayerInstance.playerConnection = conn;
        }
        if (SceneManager.GetActiveScene().name == "OutdoorsSceneMirror")
        {
            Debug.Log("Вошло 2");
            PlayerObjectController GamePlayerInstance = Instantiate(GamePlayerPrefab);
            Debug.Log("Создал?");

            /*
            GamePlayerInstance.ConnectionID = conn.connectionId;
            GamePlayerInstance.PlayerIDNumber = Gameplayers.Count + 1;
            GamePlayerInstance.PlayerSteamID = (ulong)
                SteamMatchmaking.GetLobbyMemberByIndex(
                    (CSteamID)SteamLobby.Instance.CurrentLobbyID,
                    Gameplayers.Count
                );
            */
            Debug.Log("Хочет передать конекшн");

            NetworkServer.AddPlayerForConnection(conn, GamePlayerInstance.gameObject);
            Debug.Log("Передал конекшн");
            GamePlayerInstance.playerConnection = conn;
        }
    }

    public void StartGame(string SceneName)
    {
        ServerChangeScene(SceneName);
    }
}
