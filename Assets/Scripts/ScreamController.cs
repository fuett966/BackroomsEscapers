using FishNet.Object;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamController : NetworkBehaviour
{
    public AudioClip screamSound; // ���� �����

    public AudioSource screamSource;
    public float screamRadius = 5f; // ������ �������� �����
    public int screamDamage = 20; // ����

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            screamSource.clip = screamSound;
        }
        else
        {
            Debug.Log("������ ����� ���� ������ ����");
            return;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("�������");
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
                Debug.Log("������ ��� ������� ����");
                col.GetComponent<IDamageable>().TakeDamage(screamDamage);
            }
        }
    }

    [ServerRpc]
    public void ScreamServer()
    {
        Scream();
    }

    //[ServerRpc]
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
