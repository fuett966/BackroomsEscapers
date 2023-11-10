using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamController : NetworkBehaviour
{
    public AudioClip screamSound; // Звук крика

    public AudioSource screamSource;
    // public float screamRadius = 5f; // Радиус действия крика
    //  public int screamDamage = 20; // Урон
    public override void OnStartClient()
    {
        base.OnStartClient();
        if(base.IsOwner)
        {
            screamSource.clip = screamSound;
        }
        else
        {
            Debug.Log("Монстр может быть только один");
            return;
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Нажимаю");
            ScreamServer();
        }
    }
    [ServerRpc]
    public void ScreamServer()
    {
        Scream();
    }
    [ObserversRpc]
    public void Scream()
    {
        screamSource.clip = screamSound;
        screamSource.Play();
    }
}
