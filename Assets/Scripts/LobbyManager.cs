using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;
using FishNet.Object.Synchronizing;
using TMPro;
using System;

public class LobbyManager : NetworkBehaviour
{
    public static LobbyManager Instance;
    private bool gameStarted = false;

    // public TextMeshProUGUI playersReadyCountText;
     //public TextMeshProUGUI playersEntityCountText;
     //public TextMeshProUGUI playersHumansCountText;



    [SyncVar] public int playersReady = 0;
    [SyncVar] public int playersEntity = 0;
    [SyncVar] public int playersHumans = 0;

    [SyncVar] public int maxPlayersEntity = 1;
    [SyncVar] public int maxPlayersHumans = 4;

    public event Action<int> OnEntityValueChanged;
    public event Action<int> OnHumansValueChanged;


    //[SerializeField] private GameObject canvasObject;
    [SerializeField] private List<GameObject> characters = new List<GameObject>();
    // [SerializeField] private GameObject characterSelectorPanel;
    private void Awake()
    {
        Instance = this;
    }
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!base.IsOwner)
            return;
    }

    public void SpawnHuman()
    {
      //  characterSelectorPanel.SetActive(false);
        Spawn(0, LocalConnection);
    }
    public void SpawnEntity()
    {
       // characterSelectorPanel.SetActive(false);
        Spawn(1, LocalConnection);

    }

    [ServerRpc(RequireOwnership = false)]
    void Spawn(int spawnIndex, NetworkConnection conn)
    {
        GameObject player = Instantiate(characters[spawnIndex], Vector3.zero, Quaternion.identity);
        Spawn(player, conn);
    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateSelectedEntity(LobbyManager lobbyManager, int amountToChange, ToggleButtonSelector senderButton)
    {
        Debug.Log("Сигнал на энтити принят");

        lobbyManager.playersEntity += amountToChange;
        OnEntityValueChanged?.Invoke(playersEntity);
      //  lobbyManager.playersEntityCountText.text = lobbyManager.playersEntity.ToString();
        if (lobbyManager.playersEntity >= lobbyManager.maxPlayersEntity)
            senderButton.DisableButton();
        else
            senderButton.EnableButton();

    }

    [ServerRpc(RequireOwnership = false)]
    public void UpdateSelectedHumans(LobbyManager lobbyManager, int amountToChange, ToggleButtonSelector senderButton)
    {
        Debug.Log("Сигнал на человека принят");
        lobbyManager.playersHumans += amountToChange;

       // OnHumansValueChanged?.Invoke(playersHumans);
         UpdateSelectedServ();

        if (lobbyManager.playersHumans >= lobbyManager.maxPlayersHumans)
            senderButton.DisableButton();
        else
            senderButton.EnableButton();
    }
    [ObserversRpc]
    public void UpdateSelectedServ()
    {
        Debug.Log("Вызывало ивент");
        OnHumansValueChanged?.Invoke(playersHumans);
    }
    [ServerRpc]
    public void UpdateReadyPlayers(LobbyManager lobbyManager, int amountToChange, ToggleButtonSelector senderButton)
    {
        lobbyManager.playersReady += amountToChange;
    }

}
