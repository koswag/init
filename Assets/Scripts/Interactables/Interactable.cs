using UnityEngine;

namespace Interactables {
    public abstract class Interactable : MonoBehaviour {

        public bool useEvents;
    
        [SerializeField]
        public string promptMessage;

        public virtual string OnLook() {
            return promptMessage;
        }

        protected virtual void Interact() { }

        public void BaseInteract() {
            if (useEvents) {
                GetComponent<InteractionEvent>().OnInteract.Invoke();
            }
            Interact();
        }
    }
}
