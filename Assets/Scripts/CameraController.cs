using UnityEngine;
using Mirror;

public class CameraController : NetworkBehaviour
{
    [SerializeField]
    GameObject cameraMain;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (isLocalPlayer)
        {
            cameraMain.GetComponent<Camera>().enabled = true;
            cameraMain.GetComponent<AudioListener>().enabled = true;
        }
    }
}
