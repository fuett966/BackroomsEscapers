using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class CubeInteractor : NetworkBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] LayerMask doorLayer;
    private GameObject selectedCube;

    void Update()
    {
        if (isOwned)
        {
            Debug.Log(isOwned);
            LocalInteract();
        }
    }
    [Command]
    private void CmdInteractHold(GameObject cube)
    {
        Debug.Log("CmdInteract");
        ServerInteract(cube);
    }
    [ClientRpc]
    private void ServerInteract(GameObject cube)
    {
        Color newColor = new Color(Random.value, Random.value, Random.value);
        cube.GetComponent<MeshRenderer>().material.color = newColor;
    }
    private void LocalInteract()
    {
        Debug.Log("LocalInteract");
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 20, doorLayer))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("MouseDown");
                selectedCube = hit.collider.gameObject;
                CmdInteractHold(selectedCube);
            }
        }
    }
}
