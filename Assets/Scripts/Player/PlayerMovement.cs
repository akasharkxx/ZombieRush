using DG.Tweening;
using UnityEngine;

namespace ZombieRush
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed = 10f;
        [SerializeField] private float _rotateSpeed = 10f;
        [SerializeField] private Transform _playerRotator;
        [SerializeField] private bool _isJoyStickAimingEnabled = true;
        private bool _isTakingDamage = false;

        private Rigidbody _rigidBody;
        private PlayerController _playerController;

        internal Transform mTransform;

        private Vector3 _movePosition;

        internal void Init()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _playerController = GetComponent<PlayerController>();
            _playerController.mPlayerHealth.OnTakeDamage += PlayerHealth_OnTakeDamage;
            mTransform = transform;
        }

        private void PlayerHealth_OnTakeDamage(int damageAmount)
        {
            // DO Something then enable movement
        }

        internal void UpdateMovement()
        {
            if (_isTakingDamage) { return; }

            Vector3 moveInput = Vector3.right * _playerController.mPlayerInput.MovementInput.x + Vector3.forward * _playerController.mPlayerInput.MovementInput.y;
            
            Vector3 rotateDirection = moveInput;
            
            moveInput *= _movementSpeed * Time.smoothDeltaTime;

            _movePosition = moveInput;

            //Vector3 pos = mTransform.position;
            //pos.y = 0.5f;
            //mTransform.position = pos;

            if (moveInput != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(rotateDirection, Vector3.up);

                _playerRotator.rotation = Quaternion.RotateTowards(_playerRotator.rotation, lookRotation, _rotateSpeed * Time.smoothDeltaTime);
            }

            if (_isJoyStickAimingEnabled && _playerController.mPlayerInput.AimInput != Vector2.zero)
            {
                float yAngle = Mathf.Atan2(_playerController.mPlayerInput.AimInput.y, _playerController.mPlayerInput.AimInput.x) * Mathf.Rad2Deg - 90f;

                Vector3 rotation = _playerRotator.eulerAngles;

                rotation.y = -yAngle;

                _playerRotator.eulerAngles = rotation;
            }
        }

        internal void UpdateFixedMovement()
        {
            //Debug.Log(_movePosition);
            _rigidBody.MovePosition(mTransform.position + _movePosition);
        }
    }
}