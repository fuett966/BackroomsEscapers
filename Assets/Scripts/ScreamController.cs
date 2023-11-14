using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamController : NetworkBehaviour
{
    public AudioClip screamSound; // Звук крика

    public AudioSource screamSource;
    public float screamRadius = 5f; // Радиус действия крика
    public int screamDamage = 20; // Урон
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
    void DealDamageToNearbyPlayers()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, screamRadius);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Human"))
            {
                Debug.Log("Должен был нанести урон");
                col.GetComponent<IDamageable>().TakeDamage(screamDamage);
            }
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
        DealDamageToNearbyPlayers();
        screamSource.clip = screamSound;
        screamSource.Play();
    }
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, screamRadius);
    }
}
