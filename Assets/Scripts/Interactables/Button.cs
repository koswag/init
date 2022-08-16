using System;
using UnityEngine;

namespace Interactables {
    public class Button : Interactable {
        
        [SerializeField]
        private GameObject door;

        [SerializeField]
        private AudioSource buttonSound;

        [SerializeField]
        private AudioSource doorSound;

        [SerializeField]
        private AudioSource doorCloseSound;

        private static readonly int IsOpen = Animator.StringToHash("IsOpen");

        protected override void Interact() {
            bool isOpen = door.GetComponent<DoorState>().SwitchState();
            door.GetComponent<Animator>().SetBool(IsOpen, isOpen);

            if (isOpen) {
                buttonSound.Play();
                doorSound.PlayDelayed(0.25f);
            } else {
                doorSound.Play();
                doorCloseSound.PlayDelayed(0.2f);
            }
        }
    }
}
