using UnityEngine;

namespace ZombieRush
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private FloatingJoystick movementJoyStick;
        [SerializeField] private FloatingJoystick aimJoyStick;

        internal Vector2 MovementInput { get; private set; }
        internal Vector2 AimInput { get; private set; }

        internal void UpdateInput()
        {
            MovementInput = movementJoyStick.Direction + (Vector2.right * Input.GetAxis("Horizontal") + Vector2.up * Input.GetAxis("Vertical"));
            AimInput = aimJoyStick.Direction;
            GameManager.Instance.playerInput = MovementInput;
        }
    }
}

