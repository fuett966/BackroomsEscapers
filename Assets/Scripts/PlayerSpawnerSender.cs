using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnerSender : MonoBehaviour
{
    [SerializeField] private GameObject characterSelector;
    [SerializeField] private ClientDataPlayer clientDataPlayer;
    public void TrySpawnPlayer()
    {
        if (clientDataPlayer.isEntity)
        {
            LobbyManager.Instance.SpawnEntity(characterSelector);
        }
        else
        {
            LobbyManager.Instance.SpawnHuman(characterSelector);
        }
    }
}
