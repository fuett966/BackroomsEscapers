using System;
using UnityEngine;

namespace Mirror.Examples.RigidbodyPhysics
{
    [RequireComponent(typeof(Rigidbody))]
    public class AddForce : NetworkBehaviour
    {
        public Rigidbody rigidbody3d;
        public Rigidbody rigidbodyServer;
        public float force = 500f;

        public GameObject door;

        public event Action OnMotorValueChanged;
        protected override void OnValidate()
        {
            base.OnValidate();
            rigidbody3d = GetComponent<Rigidbody>();
        }

        void Update()
        {
            rigidbodyServer = GameObject.Find("Server Ball A").GetComponent<Rigidbody>();
            door = GameObject.Find("Door");
            // do we have authority over this?
            if (!rigidbody3d.isKinematic)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (isOwned)
                    {
                        if (isServer)
                        {
                            ServerForce();
                        }
                        else
                        {
                            CmdForce();
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    if (isOwned)
                    {
                        if (isServer)
                        {
                            ServerVis();
                        }
                        else
                        {
                            CmdVis();
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (isOwned)
                    {
                        if (isServer)
                        {
                            ServerDoor();
                        }
                        else
                        {
                            CmdDoor();
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.G))
                {
                    if (isOwned)
                    {
                        if (isServer)
                        {
                            ServerDoorClose();
                        }
                        else
                        {
                            CmdDoorClose();
                        }
                    }
                }
            }
        }

        [Command]
        private void CmdForce()
        {
            ServerForce();
        }
        [ClientRpc]
        private void ServerForce()
        {
            rigidbodyServer.AddForce(Vector3.up * force);
            rigidbodyServer.GetComponent<MeshRenderer>().enabled = false;
            rigidbody3d.AddForce(Vector3.up * force);
        }
        [Command]
        private void CmdVis()
        {
            ServerVis();
        }
        [ClientRpc]
        private void ServerVis()
        {
            rigidbodyServer.GetComponent<MeshRenderer>().enabled = true;

        }

        [Command]
        private void CmdDoor()
        {
            ServerDoor();
        }
        [ClientRpc]
        private void ServerDoor()
        {

            HingeJoint joint = door.GetComponent<HingeJoint>();
            JointMotor motor = joint.motor;

            motor.targetVelocity = 200;
            joint.motor = motor;


        }

        [Command]
        private void CmdDoorClose()
        {
            ServerDoorClose();
        }
        [ClientRpc]
        private void ServerDoorClose()
        {
            HingeJoint joint = door.GetComponent<HingeJoint>();
            JointMotor motor = joint.motor;
            motor.targetVelocity = -200;
            joint.motor = motor;
        }

    }
}
