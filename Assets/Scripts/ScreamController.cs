using Mirror;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamController : NetworkBehaviour
{
    public AudioClip screamSound;

    public AudioSource screamSource;
    public float screamRadius = 5f; 
    public int screamDamage = 20; 


    private void Update()
    {
        if (!isOwned)
            return;
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isServer)
            {
                RpcScreamServer();
            }
            else
            {
                CmdScreamServer();
            }
        }
    }

    void DealDamageToNearbyPlayers()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, screamRadius);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Human"))
            {
                col.GetComponent<IDamageable>().TakeDamage(screamDamage);
            }
        }
    }

    [Command]
    public void CmdScreamServer()
    {
        RpcScreamServer();
    }

    [ClientRpc]
    public void RpcScreamServer()
    {
        Scream();
    }
    public void Scream()
    {
        DealDamageToNearbyPlayers();
        PlaySound();
    }
   private void PlaySound()
    {
        screamSource.clip = screamSound;
        screamSource.Play();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, screamRadius);
    }
}
