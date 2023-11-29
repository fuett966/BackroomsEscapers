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

        /*
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
        */
    }
}




/*using Mirror;
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
    private void CmdInteractHold(float delta)
    {
        Debug.Log("CmdInteract");
        ServerInteractHold(delta);
    }
    [ClientRpc]   
    private void ServerInteractHold(float delta)
    {
        selectedDoor = GameObject.Find("Door").transform;
            HingeJoint joint = selectedDoor.GetComponent<HingeJoint>();
            JointMotor motor = joint.motor;
            float speedMultiplier = 60000;
            Debug.Log("ServerInteract"); 
            if (isHolding)
            {
                Debug.Log("isHolding: " + isHolding);

                Debug.Log(selectedDoor);
                if (selectedDoor.parent != null)
                {
                    if (Mathf.Abs(selectedDoor.parent.forward.z) > 0.5f)
                    {
                        if (dragPointGameobject.transform.position.x > selectedDoor.position.x)
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
                joint.motor = motor;
            }
            else
            {
                Debug.Log("Exit");
                motor.targetVelocity = 0;
                joint.motor = motor;
                selectedDoor = null;
            }
        
    }
    private void LocalInteract()
    {
        Debug.Log("LocalInteract");
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            selectedDoor = GameObject.Find("Door").transform;
            Debug.Log("MouseDown");
            isHolding = true;

            // selectedDoor = hit.collider.gameObject.transform;
        }
      //  if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 20, doorLayer))
      //  {
      //      if (Input.GetMouseButtonDown(0))
       //     {
                Debug.Log("MouseDown");
       //         isHolding = true;
                
               // selectedDoor = hit.collider.gameObject.transform;
       //     }
        //}
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
                ServerInteractHold(delta);
            }
            else
            {
                CmdInteractHold(delta);
            }
        }
    }
}





*/

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractor : MonoBehaviour
{
    [SerializeField] Camera cam;
    Transform selectedDoor;
    GameObject dragPointGameobject;
    int leftDoor = 0;
    [SerializeField] LayerMask doorLayer;

    void Update()
    {
        //Raycast
        RaycastHit hit;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 20, doorLayer))
        {
            if (Input.GetMouseButtonDown(0))
            {
                selectedDoor = hit.collider.gameObject.transform;
            }
        }

        if (selectedDoor != null)
        {
            HingeJoint joint = selectedDoor.GetComponent<HingeJoint>();
            JointMotor motor = joint.motor;

            //Create drag point object for reference where players mouse is pointing
            if (dragPointGameobject == null)
            {
                dragPointGameobject = new GameObject("Ray door");
                dragPointGameobject.transform.parent = selectedDoor;
            }

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            dragPointGameobject.transform.position = ray.GetPoint(Vector3.Distance(selectedDoor.position, transform.position));
            dragPointGameobject.transform.rotation = selectedDoor.rotation;


            float delta = Mathf.Pow(Vector3.Distance(dragPointGameobject.transform.position, selectedDoor.position), 3);

            //Deciding if it is left or right door
            if (selectedDoor.GetComponent<MeshRenderer>().localBounds.center.x > selectedDoor.localPosition.x)
            {
                leftDoor = 1;
            }
            else
            {
                leftDoor = -1;
            }

            //Applying velocity to door motor
            float speedMultiplier = 60000;
            if (Mathf.Abs(selectedDoor.parent.forward.z) > 0.5f)
            {
                if (dragPointGameobject.transform.position.x > selectedDoor.position.x)
                {
                    motor.targetVelocity = delta * -speedMultiplier * Time.deltaTime * leftDoor;
                }
                else
                {
                    motor.targetVelocity = delta * speedMultiplier * Time.deltaTime * leftDoor;
                }
            }
            else
            {
                if (dragPointGameobject.transform.position.z > selectedDoor.position.z)
                {
                    motor.targetVelocity = delta * -speedMultiplier * Time.deltaTime * leftDoor;
                }
                else
                {
                    motor.targetVelocity = delta * speedMultiplier * Time.deltaTime * leftDoor;
                }
            }
            joint.motor = motor;

            if (Input.GetMouseButtonUp(0))
            {
                selectedDoor = null;
                motor.targetVelocity = 0;
                joint.motor = motor;
                Destroy(dragPointGameobject);
            }
        }
    }
}
*/