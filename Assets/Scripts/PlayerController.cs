using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieRush
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private float rotateSpeed = 10f;
        [SerializeField] private float rotateSpeed2 = 10f;
        [SerializeField] private Transform playerRotator;
        private CharacterController characterController;

        [SerializeField] private bool isJoyStickAimingEnabled = true;

        private PlayerInput input;

        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            input= GetComponent<PlayerInput>();
        }

        private void Update()
        {
            Vector3 move = new Vector3(input.MovementInput.x, 0, input.MovementInput.y);
            Vector3 rotateDirection = move;
            move *= movementSpeed * Time.smoothDeltaTime;

            characterController.Move(move);
            
            if (move != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(rotateDirection, Vector3.up);

                playerRotator.rotation = Quaternion.RotateTowards(playerRotator.rotation, lookRotation, rotateSpeed * Time.smoothDeltaTime);
            }

            if (isJoyStickAimingEnabled && input.AimInput != Vector2.zero)
            {
                float yAngle = Mathf.Atan2(input.AimInput.y, input.AimInput.x) * Mathf.Rad2Deg - 90f;

                Vector3 rotation = playerRotator.eulerAngles;

                rotation.y = -yAngle;

                playerRotator.eulerAngles = rotation;
            }
            
        }
    }
}