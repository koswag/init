using System;
using Interactables;
using UnityEngine;

namespace Player {
    public class PlayerInteract : MonoBehaviour {
        private Camera _cam;

        [SerializeField]
        private float distance = 3f;

        [SerializeField]
        private LayerMask mask;

        private PlayerUI _playerUI;
        private InputManager _inputManager;

        void Start() {
            _cam = GetComponent<PlayerLook>().cam;
            _playerUI = GetComponent<PlayerUI>();
            _inputManager = GetComponent<InputManager>();
            GetComponent<PlayerHealth>();
        }

        void Update() {
            _playerUI.UpdatePrompt(string.Empty);
        
            var camTransform = _cam.transform;
            var ray = new Ray(camTransform.position, camTransform.forward);

            RaycastHit hitInfo;
            bool hit = Physics.Raycast(ray, out hitInfo, distance, mask);

            if (hit) {
                var interactable = hitInfo.collider.GetComponent<Interactable>();

                if (interactable != null) {
                    _playerUI.UpdatePrompt(interactable.promptMessage);
                    if (_inputManager.OnFoot.Interact.triggered) {
                        interactable.BaseInteract();
                    }
                }
            }
        }
    }
}
