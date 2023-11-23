using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : NetworkBehaviour
{
    public static LobbyManager Instance;

    [SyncVar]
    public int playersReady = 0;

    [SyncVar]
    public int playersEntity = 0;

    [SyncVar]
    public int playersHumans = 0;

    [SyncVar]
    public int maxPlayersEntity = 1;

    [SyncVar]
    public int maxPlayersHumans = 4;

    [SerializeField]
    private Transform _humanSpawnTransform;

    [SerializeField]
    private Transform _entitySpawnTransform;

    [SerializeField]
    private List<GameObject> characters = new List<GameObject>();

    public event Action<int> OnEntityValueChanged;
    public event Action<int> OnHumansValueChanged;


    private void Awake()
    {
        Instance = this;
    }
   
    public void SpawnHuman(GameObject characterSelectorPanel)
    {

        Debug.Log("Try spawn human");
        characterSelectorPanel.SetActive(false);
        Spawn(0, connectionToClient, false);
    }

    public void SpawnEntity(GameObject characterSelectorPanel)
    {
        Debug.Log("Try spawn entity");

        characterSelectorPanel.SetActive(false);
        Spawn(1, connectionToClient, true);
    }

    [Command(requiresAuthority = false)]
    void Spawn(int spawnIndex, NetworkConnectionToClient conn, bool isEntity)
    {
        Debug.Log("Spawn Method");
        Vector3 spawnPosition;
        if (isEntity)
            spawnPosition = _entitySpawnTransform.position;
        else
            spawnPosition = _humanSpawnTransform.position;

        Debug.Log("Try spawn");
        GameObject player = Instantiate(characters[spawnIndex], spawnPosition, Quaternion.identity);
        Debug.Log("Spawned: " + player);

        NetworkServer.AddPlayerForConnection(connectionToClient, player);
        Debug.Log("Added To Connection");

    }

    [Command(requiresAuthority = false)]
    public void UpdateSelectedEntity(
        LobbyManager lobbyManager,
        int amountToChange,
        ToggleButtonSelector senderButton
    )
    {
        Debug.Log("������ �� ������ ������");
        UpdateSelectedServ();
        lobbyManager.playersEntity += amountToChange;
        //  GameManager.Instance.playersEntity = lobbyManager.playersEntity;
        OnEntityValueChanged?.Invoke(playersEntity);
        //  lobbyManager.playersEntityCountText.text = lobbyManager.playersEntity.ToString();
        // if (lobbyManager.playersEntity >= lobbyManager.maxPlayersEntity)
        //     senderButton.DisableButton();
        // else
        //     senderButton.EnableButton();
    }

    [Command(requiresAuthority = false)]
    public void UpdateSelectedHumans(
        LobbyManager lobbyManager,
        int amountToChange,
        ToggleButtonSelector senderButton
    )
    {
        lobbyManager.playersHumans += amountToChange;
        //   GameManager.Instance.playersHumans = lobbyManager.playersHumans;

        // OnHumansValueChanged?.Invoke(playersHumans);
        UpdateSelectedServ();

        if (lobbyManager.playersHumans >= lobbyManager.maxPlayersHumans)
            senderButton.DisableButton();
        else
            senderButton.EnableButton();
    }

    [Command(requiresAuthority = false)]
    public void UpdateSelectedServ()
    {
        OnHumansValueChanged?.Invoke(playersHumans);
        OnEntityValueChanged?.Invoke(playersEntity);
    }

    [Command(requiresAuthority = false)]
    public void UpdateReadyPlayers(
        LobbyManager lobbyManager,
        int amountToChange,
        ToggleButtonSelector senderButton
    )
    {
        lobbyManager.playersReady += amountToChange;
    }
}
