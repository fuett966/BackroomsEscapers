using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ClientDataPlayer : NetworkBehaviour
{
    public GameObject characterSelector;
    public bool isEntity;

    public override void OnStartClient()
    {
        if (!isOwned)
        {
            return;
        }

        characterSelector.SetActive(true);
    }
}
