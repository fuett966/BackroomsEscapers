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
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        if (SceneManager.GetActiveScene().name == "LobbySteam")
        {
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

            Debug.Log("ПлеерКонМенеджер: " + conn);

            Debug.Log("ПлеерКонМенеджерИнстанс: " + GamePlayerInstance.playerConnection);
        }
    }

    public void StartGame(string SceneName)
    {
        ServerChangeScene(SceneName);
    }
}
