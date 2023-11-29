using Mirror;
using UnityEngine;

public class DoorInteractor : NetworkBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] LayerMask doorLayer;

    void Update()
    {
        if (isOwned)
        {
            Debug.Log(isOwned);
            LocalInteract();
        }
    }
    [Command]
    private void CmdInteractDoor(GameObject door, float velocity)
    {
        Debug.Log("CmdInteract");
        ServerInteractDoor(door, velocity);
    }
    [ClientRpc]
    private void ServerInteractDoor(GameObject door, float velocity)
    {
        HingeJoint joint = door.GetComponent<HingeJoint>();
        JointMotor motor = joint.motor;
        motor.targetVelocity = velocity;
        joint.motor = motor;

    }
    private void LocalInteract()

    {
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 20, doorLayer))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("MouseDown");
                if (isServer)
                {
                    ServerInteractDoor(hit.collider.gameObject, 200);
                }
                else
                {
                    CmdInteractDoor(hit.collider.gameObject, 200);
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (isServer)
                {
                    ServerInteractDoor(hit.collider.gameObject, -200);
                }
                else
                {
                    CmdInteractDoor(hit.collider.gameObject, -200);
                }
            }
        }
    }
}