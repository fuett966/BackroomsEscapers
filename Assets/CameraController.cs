using UnityEngine;
using UnityEngine.Networking;
using FishNet.Connection;
using FishNet.Object;

public class CameraController : NetworkBehaviour
{
    [SerializeField]
    GameObject cameraMain;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (base.IsOwner)
        {
            cameraMain.GetComponent<Camera>().enabled = true;
            cameraMain.GetComponent<AudioListener>().enabled = true;
        }
    }
}
