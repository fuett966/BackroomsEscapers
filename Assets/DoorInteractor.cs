using Mirror;
using UnityEngine;

public class DoorInteractor : NetworkBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] LayerMask doorLayer;
    GameObject dragPointGameobject;
    GameObject selectedDoor;
    int leftDoor = 0;
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
    [Client]
    private void LocalInteract()

    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 20, doorLayer))
        {
            if (Input.GetMouseButtonDown(0))
            {
                selectedDoor = hit.collider.gameObject;
            }
        }

        if (selectedDoor != null)
        {
            HingeJoint joint = selectedDoor.GetComponent<HingeJoint>();
            JointMotor motor = joint.motor;
            float speedMultiplier = 60000;
            float velocity = 0;
            if (dragPointGameobject == null)
            {
                dragPointGameobject = new GameObject("Ray door");
                dragPointGameobject.transform.parent = selectedDoor.transform;
            }

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            dragPointGameobject.transform.position = ray.GetPoint(Vector3.Distance(selectedDoor.transform.position, transform.position));
            dragPointGameobject.transform.rotation = selectedDoor.transform.rotation;


            float delta = Mathf.Pow(Vector3.Distance(dragPointGameobject.transform.position, selectedDoor.transform.position), 3);

            if (selectedDoor.GetComponent<MeshRenderer>().localBounds.center.x > selectedDoor.transform.localPosition.x)
            {
                leftDoor = 1;
            }
            else
            {
                leftDoor = -1;
            }
            //   if (door.transform.parent != null)
            //   {
            if (Mathf.Abs(selectedDoor.transform.parent.forward.z) > 0.01f)
            {
                if (dragPointGameobject.transform.position.x > selectedDoor.transform.position.x)
                {
                    velocity = delta * -speedMultiplier * Time.deltaTime * leftDoor;
                    if (isServer)
                    {
                        ServerInteractDoor(selectedDoor, velocity);
                    }
                    else
                    {
                        CmdInteractDoor(selectedDoor, velocity);
                    }
                    Debug.Log("MotorVel: " + motor.targetVelocity);

                }
                else
                {
                    velocity = delta * speedMultiplier * Time.deltaTime * leftDoor;
                    if (isServer)
                    {
                        ServerInteractDoor(selectedDoor, velocity);
                    }
                    else
                    {
                        CmdInteractDoor(selectedDoor, velocity);
                    }
                    Debug.Log("MotorVel: " + motor.targetVelocity);
                }
            }
            //    }
            else
            {
                if (dragPointGameobject.transform.position.z > selectedDoor.transform.position.z)
                {
                    velocity = delta * -speedMultiplier * Time.deltaTime * leftDoor;
                    if (isServer)
                    {
                        ServerInteractDoor(selectedDoor, velocity);
                    }
                    else
                    {
                        CmdInteractDoor(selectedDoor, velocity);
                    }
                    Debug.Log("MotorVel: " + motor.targetVelocity);
                }
                else
                {
                    velocity = delta * speedMultiplier * Time.deltaTime * leftDoor;
                    if (isServer)
                    {
                        ServerInteractDoor(selectedDoor, velocity);
                    }
                    else
                    {
                        CmdInteractDoor(selectedDoor, velocity);
                    }
                    Debug.Log("MotorVel: " + motor.targetVelocity);
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (isServer)
                {
                    ServerInteractDoor(selectedDoor, 0);
                }
                else
                {
                    CmdInteractDoor(selectedDoor, 0);
                }
                Destroy(dragPointGameobject);
                selectedDoor = null;
            }
        }


    }
}




