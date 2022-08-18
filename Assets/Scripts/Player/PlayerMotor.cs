using System;
using System.Collections;
using UnityEngine;

namespace Player {
    public class PlayerMotor : MonoBehaviour {
        private CharacterController _controller;
        private Vector3 _velocity;
        private bool _isGrounded;
        private Vector3 _movingDirection;

        private bool CanJump => _isGrounded;
        private bool CanCrouch => _isGrounded && !_isDuringCrouchAnimation;
        private bool IsMoving => Mathf.Abs(_movingDirection.x) > 0.1f || Mathf.Abs(_movingDirection.z) > 0.1f;
        private float BobSpeed => _isCrouching ? crouchBobSpeed : _isSprinting ? sprintBobSpeed : walkBobSpeed;
        private float BobAmount => _isCrouching ? crouchBobAmount : _isSprinting ? sprintBobAmount : walkBobAmount;

        public GameObject player;
        public Camera playerCamera;
        
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

        [Header("Headbob parameters")] 
        [SerializeField] private float walkBobSpeed = 14f;
        [SerializeField] private float walkBobAmount = 0.05f;
        [SerializeField] private float sprintBobSpeed = 18f;
        [SerializeField] private float sprintBobAmount = 0.1f;
        [SerializeField] private float crouchBobSpeed = 8f;
        [SerializeField] private float crouchBobAmount = 0.025f;
        private float _defaultYPos = 0;
        private float timer;

        void Start() {
            _controller = GetComponent<CharacterController>();
            _defaultYPos = playerCamera.transform.localPosition.y;
            _movingDirection = Vector3.zero;
        }

        void Update() {
            _isGrounded = _controller.isGrounded;
        }

        public void ProcessMove(Vector2 input) {
            ProcessInput(input);
            ProcessGravity();
            ProcessHeadbob();
        }


        private void ProcessInput(Vector2 input) {
            _movingDirection = transform.TransformDirection(
                direction: TranslateHorizontal(input)
            );

            var move = _movingDirection * (Speed * Time.deltaTime);
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

        
        private void ProcessHeadbob() {
            if (_isGrounded && IsMoving) {
                Debug.Log("du[pa");
                timer += Time.deltaTime * BobSpeed;
                var cameraTransform = playerCamera.transform;
                
                cameraTransform.localPosition = new(
                    cameraTransform.localPosition.x,
                    _defaultYPos + Mathf.Sin(timer) * BobAmount,
                    cameraTransform.localPosition.z
                );
            }
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
