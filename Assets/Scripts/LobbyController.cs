using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Steamworks;
using System.Linq;
using TMPro;
using EvolveGames;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    public static LobbyController Instance;

    public TextMeshProUGUI LobbyNameText;

    public GameObject PlayerListViewContent;
    public GameObject PlayerListItemPrefab;
    public GameObject LocalPlayerObject;

    public ulong CurrentLobbyID;
    public bool PlayerItemCreated = false;
    private List<PlayerListItem> PlayerListItems = new List<PlayerListItem>();
    private List<PlayerObjectController> PlayerObjectControllers = new List<PlayerObjectController>();

    public PlayerObjectController LocalPlayerController;

    public Button StartGameButton;
    public TextMeshProUGUI ReadyButtonText;

    public GameObject clientPrefab;

    private CustomNetworkManager manager;
    private CustomNetworkManager Manager
    {
        get
        {
            if (manager != null)
            {
                return manager;
            }
            return manager = CustomNetworkManager.singleton as CustomNetworkManager;
        }
    }

    private void OnEnable()
    {
        if (LobbyManager.Instance != null)
        {
            SpawnClients();
        }
        else
        {
            StartCoroutine(WaitLobbyManagerInitialize());
        }
        if(Manager != null)
        {
            Manager.OnStartHostEvent += CreateHostPlayerItem;
        }
    }

    private void SpawnClients()
    {
        for (int i = 0; i < PlayerObjectControllers.Count; i++)
        {
            Debug.Log("Кол-во: " + PlayerObjectControllers.Count);
            Debug.Log(PlayerObjectControllers[i].playerConnection);
         }
        LobbyManager.Instance.SpawnClients(PlayerObjectControllers);
       // Debug.Log("Clients");
       // for(int i = 0; i < PlayerListItems.Count; i++)
       // {
       //     var client = Instantiate(clientPrefab);
       //     NetworkServer.Spawn(client, PlayerListItems[i].connection);
       // }
    }

    IEnumerator WaitLobbyManagerInitialize()
    {
        yield return new WaitUntil(() => LobbyManager.Instance != null);
        SpawnClients();
            
        }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(Instance);
    }

    public void ReadyPlayer()
    {
        LocalPlayerController.ChangeReady();
    }

    public void UpdateButton()
    {
        if (LocalPlayerController.Ready)
        {
            ReadyButtonText.text = "Unready";
        }
        else
        {
            ReadyButtonText.text = "Ready";
        }
    }

    public void CheckIfAllReady()
    {
        bool AllReady = false;
        foreach (PlayerObjectController player in Manager.Gameplayers)
        {
            if (player.Ready)
            {
                AllReady = true;
            }
            else
            {
                AllReady = false;
                break;
            }
        }

        if (AllReady)
        {
            if (LocalPlayerController.PlayerIDNumber == 1)
            {
                StartGameButton.interactable = true;
            }
            else
            {
                StartGameButton.interactable = false;
            }
        }
        else
        {
            StartGameButton.interactable = false;
        }
    }

    public void UpdateLobbyName()
    {
        CurrentLobbyID = Manager.GetComponent<SteamLobby>().CurrentLobbyID;
        LobbyNameText.text = SteamMatchmaking.GetLobbyData(new CSteamID(CurrentLobbyID), "name");
    }
    
    public void UpdatePlayerList()
    {
       // if (!PlayerItemCreated)
        //{
      //      CreateHostPlayerItem(); //HOST
       // }
        if (PlayerListItems.Count < Manager.Gameplayers.Count)
        {
            CreateClientPlayerItem();
        }
        if (PlayerListItems.Count > Manager.Gameplayers.Count)
        {
            RemovePlayerItem();
        }
        if (PlayerListItems.Count == Manager.Gameplayers.Count)
        {
            UpdatePlayerItem();
        }
    }

    public void FindLocalPlayer()
    {
        LocalPlayerObject = GameObject.Find("LocalgamePlayer");
        LocalPlayerController = LocalPlayerObject.GetComponent<PlayerObjectController>();
    }

    public void CreateHostPlayerItem()
    {
        Debug.Log("Вызвался Хост");
        Debug.Log("Менеджер плеерс число: " + Manager.Gameplayers.Count);
        Debug.Log("Менджер: " + Manager);
        Debug.Log("Менджер: " + Manager);



        foreach (PlayerObjectController player in Manager.Gameplayers)
        {
            Debug.Log("Внутри 1");
            Debug.Log(PlayerObjectControllers);
            Debug.Log(PlayerObjectControllers.Count);
            Debug.Log(player);



            PlayerObjectControllers.Add(player);

            GameObject NewPlayerItem = Instantiate(PlayerListItemPrefab as GameObject);
            PlayerListItem NewPlayerItemScript = NewPlayerItem.GetComponent<PlayerListItem>();

            NewPlayerItemScript.playerConnection = player.playerConnection;
            NewPlayerItemScript.playerName = player.PlayerName;
            NewPlayerItemScript.ConnectionID = player.ConnectionID;
            NewPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
            NewPlayerItemScript.Ready = player.Ready;
            NewPlayerItemScript.SetPlayerValues();

            NewPlayerItem.transform.SetParent(PlayerListViewContent.transform);
            NewPlayerItem.transform.localScale = Vector3.one;

            PlayerListItems.Add(NewPlayerItemScript);
        }
        PlayerItemCreated = true;
    }

    public void CreateClientPlayerItem()
    {
        Debug.Log("Вызвался Клиент");
        foreach (PlayerObjectController player in Manager.Gameplayers)
        {
            PlayerObjectControllers.Add(player);
            if (!PlayerListItems.Any(b => b.ConnectionID == player.ConnectionID))
            {
                GameObject NewPlayerItem = Instantiate(PlayerListItemPrefab as GameObject);
                PlayerListItem NewPlayerItemScript = NewPlayerItem.GetComponent<PlayerListItem>();

                NewPlayerItemScript.playerName = player.PlayerName;
                NewPlayerItemScript.ConnectionID = player.ConnectionID;
                NewPlayerItemScript.PlayerSteamID = player.PlayerSteamID;
                NewPlayerItemScript.Ready = player.Ready;
                NewPlayerItemScript.SetPlayerValues();

                NewPlayerItem.transform.SetParent(PlayerListViewContent.transform);
                NewPlayerItem.transform.localScale = Vector3.one;

                PlayerListItems.Add(NewPlayerItemScript);
            }
        }
    }

    public void UpdatePlayerItem()
    {
        foreach (PlayerObjectController player in Manager.Gameplayers)
        {
            foreach (PlayerListItem PlayerListItemScript in PlayerListItems)
            {
                if (PlayerListItemScript.ConnectionID == player.ConnectionID)
                {
                    PlayerListItemScript.playerName = player.PlayerName;
                    PlayerListItemScript.Ready = player.Ready;
                    PlayerListItemScript.SetPlayerValues();
                    if (player == LocalPlayerController)
                    {
                        UpdateButton();
                    }
                }
            }
        }
        CheckIfAllReady();
    }

    public void RemovePlayerItem()
    {
        List<PlayerListItem> playerListItemToRemove = new List<PlayerListItem>();

        foreach (PlayerListItem playerListItem in PlayerListItems)
        {
            if (!Manager.Gameplayers.Any(b => b.ConnectionID == playerListItem.ConnectionID))
            {
                playerListItemToRemove.Add(playerListItem);
            }
            if (playerListItemToRemove.Count > 0)
            {
                foreach (PlayerListItem playerlistItemToRemove in playerListItemToRemove)
                {
                    GameObject ObjectToRemove = playerlistItemToRemove.gameObject;
                    PlayerListItems.Remove(playerlistItemToRemove);
                    Destroy(ObjectToRemove);
                    ObjectToRemove = null;
                }
            }
        }
    }

    public void StartGame(string SceneName)
    {
        LocalPlayerController.CanStartGame(SceneName);
    }
}
