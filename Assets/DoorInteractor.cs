using Mirror;
using UnityEngine;

public class DoorInteractor : NetworkBehaviour
{
    [SerializeField] Camera cam;
    Transform selectedDoor;
    GameObject dragPointGameobject;
    int leftDoor = 0;
    [SerializeField] LayerMask doorLayer;
    private bool isHolding;

    void Update()
    {
        if (isOwned)
        {
            Debug.Log(isOwned);
            LocalInteract();
        }
    }
    [Command]
    private void CmdInteractHold(float delta, Transform door)
    {
        Debug.Log("CmdInteract");
        ServerInteractHold(delta, door);
    }
    [ClientRpc]   
    private void ServerInteractHold(float delta, Transform door)
    {
        if (door != null)
        {
            HingeJoint joint = door.GetComponent<HingeJoint>();
            JointMotor motor = joint.motor;
            if (isHolding)
            {
                motor.targetVelocity = 200;
                joint.motor = motor;
            }
            else
            {
                Debug.Log("Exit");
                motor.targetVelocity = -200;
                joint.motor = motor;
            }    
            /*
            float speedMultiplier = 60000;
            Debug.Log("ServerInteract"); 
            if (isHolding)
            {
                Debug.Log("isHolding: " + isHolding);

                Debug.Log(door);
                if (door.parent != null)
                {
                    if (Mathf.Abs(door.parent.forward.z) > 0.5f)
                    {
                        if (dragPointGameobject.transform.position.x > door.position.x)
                        {
                            motor.targetVelocity = delta * -speedMultiplier * Time.deltaTime * leftDoor;
                            Debug.Log("MotorVel: " + motor.targetVelocity);

                        }
                        else
                        {
                            motor.targetVelocity = delta * speedMultiplier * Time.deltaTime * leftDoor;
                            Debug.Log("MotorVel: " + motor.targetVelocity);
                        }
                    }
                     else
                {
                    if (dragPointGameobject.transform.position.z > selectedDoor.position.z)
                    {
                        motor.targetVelocity = delta * -speedMultiplier * Time.deltaTime * leftDoor;
                        Debug.Log("MotorVel: " + motor.targetVelocity);
                    }
                    else
                    {
                        motor.targetVelocity = delta * speedMultiplier * Time.deltaTime * leftDoor;
                        Debug.Log("MotorVel: " + motor.targetVelocity);
                    }
                }
                }
                else
                {
                    if (dragPointGameobject.transform.position.z > door.position.z)
                    {
                        motor.targetVelocity = delta * -speedMultiplier * Time.deltaTime * leftDoor;
                        Debug.Log("MotorVel: " + motor.targetVelocity);
                    }
                    else
                    {
                        motor.targetVelocity = delta * speedMultiplier * Time.deltaTime * leftDoor;
                        Debug.Log("MotorVel: " + motor.targetVelocity);
                    }
                }
                joint.motor = motor;
            }
            else
            {
                Debug.Log("Exit");
                motor.targetVelocity = 0;
                joint.motor = motor;
                door = null;
            }
            */
        }
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
                isHolding = true;
                selectedDoor = hit.collider.gameObject.transform;
            }
        }
        if (selectedDoor != null)
        {
            if (dragPointGameobject == null)
            {
                dragPointGameobject = new GameObject("Ray door");
                dragPointGameobject.transform.parent = selectedDoor;
            }

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            dragPointGameobject.transform.position = ray.GetPoint(Vector3.Distance(selectedDoor.position, transform.position));
            dragPointGameobject.transform.rotation = selectedDoor.rotation;


            float delta = Mathf.Pow(Vector3.Distance(dragPointGameobject.transform.position, selectedDoor.position), 3);

            if (selectedDoor.GetComponent<MeshRenderer>().localBounds.center.x > selectedDoor.localPosition.x)
            {
                leftDoor = 1;
            }
            else
            {
                leftDoor = -1;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isHolding = false;
                Destroy(dragPointGameobject);
            }
            Debug.Log("IsServer: " + isServer);
            if (isServer)
            {
                ServerInteractHold(delta, selectedDoor);
            }
            else
            {
                CmdInteractHold(delta, selectedDoor);
            }
        }
    }
}





