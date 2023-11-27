using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerSpawnerSender : NetworkBehaviour
{
    [SerializeField]
    private GameObject characterSelector;

    [SerializeField]
    private ClientDataPlayer clientDataPlayer;

    public void TrySpawnPlayer()
    {
        if (clientDataPlayer.isEntity)
        {
            LobbyManager.Instance.SpawnEntity(characterSelector, clientDataPlayer);
        }
        else
        {
            LobbyManager.Instance.SpawnHuman(characterSelector, clientDataPlayer);
        }
    }
}
