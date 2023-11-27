using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ClientDataPlayer : NetworkBehaviour
{
    public GameObject characterSelector;
    public bool isEntity;

    private void Start()
    {
        if (!isOwned)
        {
            Debug.Log("Не овнер Старт");
            return;
        }

        characterSelector.SetActive(true);
    }
    public override void OnStartClient()
    {
        Debug.Log("Не овнер Клиент старт");

        if (!isOwned)
        {
            return;
        }

        characterSelector.SetActive(true);
    }
    public override void OnStartAuthority()
    {
        Debug.Log("Авторити");
        base.OnStartAuthority();
        characterSelector.SetActive(true);
    }
}
