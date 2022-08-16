using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public class InputManager : MonoBehaviour {
        private PlayerInput _playerInput;
        private PlayerInput.OnFootActions _onFoot;

        private PlayerMotor _motor;
        private PlayerLook _look;

        public PlayerInput.OnFootActions OnFoot => _onFoot;

        void Awake() {
            _playerInput = new PlayerInput();
            _onFoot = _playerInput.OnFoot;
            _onFoot.Jump.performed += _ => _motor.ProcessJump();
            _onFoot.Crouch.performed += _ => _motor.ProcessCrouch();

            _motor = GetComponent<PlayerMotor>();
            _look = GetComponent<PlayerLook>();
        }


        void FixedUpdate() {
            _motor.ProcessMove(
                input: ReadDirection(_onFoot.Movement)
            );
        }

        void LateUpdate() {
            _look.ProcessLook(
                input: ReadDirection(_onFoot.Look)
            );
        }

        private Vector2 ReadDirection(InputAction inputAction) =>
            inputAction.ReadValue<Vector2>();


        private void OnEnable() {
            _onFoot.Enable();
        }

        private void OnDisable() {
            _onFoot.Disable();
        }
    }
}