using UnityEngine;

namespace Player {
    public class PlayerLook : MonoBehaviour {
        public Camera cam;
        private float _xRotation = 0f;

        public float xSensitivity = 30f;
        public float ySensitivity = 30f;

        public void ProcessLook(Vector2 input) {
            ProcessY(input.y);
            ProcessX(input.x);
        }

        private void ProcessY(float y) {
            _xRotation -= y * Time.deltaTime * ySensitivity;
            _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);
        }

        private void ProcessX(float x) {
            cam.transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
            transform.Rotate(Vector3.up * (x * Time.deltaTime * xSensitivity));
        }
    }
}
