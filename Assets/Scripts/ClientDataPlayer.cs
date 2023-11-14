using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
public class ClientDataPlayer : NetworkBehaviour
{
    public GameObject characterSelector;
    public bool isEntity;
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!base.IsOwner)
            return;
        characterSelector.SetActive(true);
    }
}
