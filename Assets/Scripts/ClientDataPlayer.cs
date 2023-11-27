using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ClientDataPlayer : NetworkBehaviour
{
    public GameObject characterSelector;
    public bool isEntity;
    public NetworkConnection playerConnection;
    private void Start()
    {
        if (!isOwned)
        {
            return;
        }

        characterSelector.SetActive(true);
    }
    public override void OnStartClient()
    {

        if (!isOwned)
        {
            return;
        }

        characterSelector.SetActive(true);
    }
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        characterSelector.SetActive(true);
    }
}
