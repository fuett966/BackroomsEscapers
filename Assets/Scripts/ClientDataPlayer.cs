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
        base.OnStartClient();

        if (!isLocalPlayer)
            return;
        characterSelector.SetActive(true);
    }
}
