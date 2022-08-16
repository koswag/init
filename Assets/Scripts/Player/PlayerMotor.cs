using UnityEngine;

namespace Player {
    public class PlayerMotor : MonoBehaviour {
        private CharacterController _controller;
        private Vector3 _velocity;
        private bool _isGrounded;
        private bool _crouches;

        public GameObject player;
        public float speed = 5f;
        public float gravity = -9.8f;
        public float jumpHeight = 1f;

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

            var move = moveDirection * (speed * Time.deltaTime);
            _controller.Move(move);
        }

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
            if (_isGrounded) {
                _velocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
            }
        }


        public void ProcessCrouch() {
            var vector = new Vector3(0, 0.7f, 0);
            
            player.transform.localScale = _crouches ? vector : -vector;
            _crouches = !_crouches;
        }
    }
}