using System;
using System.Collections;
using UnityEngine;

namespace Player {
    public class PlayerMotor : MonoBehaviour {
        private CharacterController _controller;
        private Vector3 _velocity;
        private bool _isGrounded;

        private bool CanJump => _isGrounded;
        private bool CanCrouch => _isGrounded && !_isDuringCrouchAnimation;

        public GameObject player;
        
        [Header("Movement parameters")]
        public float walkSpeed = 50f;
        public float sprintSpeed = 7f;
        public float crouchSpeed = 2f;
        private bool _isSprinting = false;

        [Header("Jump parameters")]
        public float gravity = -9.8f;
        public float jumpHeight = 1f;

        [Header("Crouch parameters")] 
        [SerializeField] private float crouchingHeight = 0.5f;
        [SerializeField] private float standingHeight = 2f;
        [SerializeField] private float timeToCrouch = 0.25f;
        [SerializeField] private Vector3 crouchingCenter = new(0, 0.5f, 0);
        [SerializeField] private Vector3 standingCenter = Vector3.zero;
        private bool _isCrouching = false;
        private bool _isDuringCrouchAnimation = false;

        void Start() {
            _controller = GetComponent<CharacterController>();
        }

        void Update() {
            _isGrounded = _controller.isGrounded;
        }

        public void ProcessMove(Vector2 input) {
            ProcessInput(input);
            ProcessGravity();
        }


        private void ProcessInput(Vector2 input) {
            var moveDirection = transform.TransformDirection(
                direction: TranslateHorizontal(input)
            );

            var move = moveDirection * (Speed * Time.deltaTime);
            _controller.Move(move);
        }

        private float Speed =>
            _isCrouching ? crouchSpeed 
                : _isSprinting ? sprintSpeed : walkSpeed;

        private static Vector3 TranslateHorizontal(Vector2 input) => new() {
            x = input.x,
            z = input.y,
            y = 0
        };


        private void ProcessGravity() {
            if (_isGrounded && _velocity.y < 0) {
                _velocity.y = -2f;
            }

            _velocity.y += gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }


        public void ProcessJump() {
            if (CanJump) {
                _velocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
            }
        }


        public void ProcessSprint() {
            _isSprinting = !_isSprinting;
        }
        

        public void ProcessCrouch() {
            if (CanCrouch) {
                StartCoroutine(CrouchStand());
            }
        }

        private IEnumerator CrouchStand() {
            _isDuringCrouchAnimation = true;

            float timeElapsed = 0f;
            var (targetHeight, targetCenter) = TargetCrouchParameters();
            var (currentHeight, currentCenter) = CurrentCrouchParameters();

            while (timeElapsed < timeToCrouch) {
                float time = timeElapsed / timeToCrouch;
                _controller.height = Mathf.Lerp(currentHeight, targetHeight, time);
                _controller.center = Vector3.Lerp(currentCenter, targetCenter, time);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            _controller.height = targetHeight;
            _controller.center = targetCenter;

            _isCrouching = !_isCrouching;
            _isDuringCrouchAnimation = false;
        }

        private (float, Vector3) TargetCrouchParameters() =>
            _isCrouching 
                ? (standingHeight, standingCenter) 
                : (crouchingHeight, crouchingCenter);

        private (float, Vector3) CurrentCrouchParameters() => 
            (_controller.height, _controller.center);
    }
}