using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using FishNet.Connection;
using FishNet.Object;

namespace EvolveGames
{
    public class MovementEffects : NetworkBehaviour
    {
        [Header("MOVEMENT FX")]
        [SerializeField]
        PlayerController Player;

        [SerializeField, Range(0.05f, 2)]
        float RotationAmount = 0.2f;

        [SerializeField, Range(1f, 20)]
        float RotationSmooth = 6f;

        [Header("Movement")]
        [SerializeField]
        bool CanMovementFX = true;

        [SerializeField, Range(0.1f, 2)]
        float MovementAmount = 0.5f;

        Quaternion InstallRotation;
        Vector3 MovementVector;

        // private void Start()
        // {
        //     Player = GetComponentInParent<PlayerController>();
        //     InstallRotation = transform.localRotation;
        // }

        public override void OnStartClient()
        {
            base.OnStartClient();
            if (!base.IsOwner)
            {
                return;
            }

            Player = GetComponentInParent<PlayerController>();
            InstallRotation = transform.localRotation;
        }

        private void Update()
        {
            if (!base.IsOwner)
            {
                return;
            }
            float movementX = (Player.vertical * RotationAmount);
            float movementZ = (-Player.horizontal * RotationAmount);
            MovementVector = new Vector3(
                CanMovementFX
                    ? movementX + Player.characterController.velocity.y * MovementAmount
                    : movementX,
                0,
                movementZ
            );
            transform.localRotation = Quaternion.Lerp(
                transform.localRotation,
                Quaternion.Euler(MovementVector + InstallRotation.eulerAngles),
                Time.deltaTime * RotationSmooth
            );
        }
    }
}
