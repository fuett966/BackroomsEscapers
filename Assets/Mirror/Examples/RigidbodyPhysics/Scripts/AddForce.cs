using UnityEngine;

namespace Mirror.Examples.RigidbodyPhysics
{
    [RequireComponent(typeof(Rigidbody))]
    public class AddForce : NetworkBehaviour
    {
        public Rigidbody rigidbody3d;
        public Rigidbody rigidbodyServer;
        public float force = 500f;

        protected override void OnValidate()
        {
            base.OnValidate();
            rigidbody3d = GetComponent<Rigidbody>();
        }

        void Update()
        {
            rigidbodyServer = GameObject.Find("Server Ball A").GetComponent<Rigidbody>();
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
            }
        }

        [Command]
        private void CmdForce()
        {
           // Debug.Log(connectionToClient);
          // rigidbodyServer.gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
          // rigidbodyServer.AddForce(Vector3.up * force);

            // Debug.Log(rigidbodyServer.gameObject.GetComponent<NetworkIdentity>().isOwned);

            ServerForce();
        }
        [ClientRpc]
        private void ServerForce()
        {
            rigidbodyServer.AddForce(Vector3.up * force);
            rigidbody3d.AddForce(Vector3.up * force);
        }
    }
}
