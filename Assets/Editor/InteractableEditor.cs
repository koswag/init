using Interactables;
using UnityEditor;

namespace Editor {
    [CustomEditor(typeof(Interactable), true)]
    public class InteractableEditor : UnityEditor.Editor {
        public override void OnInspectorGUI() {
            var interactable = (Interactable)target;

            if (target.GetType() == typeof(EventOnlyInteractable)) {
                interactable.promptMessage = EditorGUILayout.TextField("Prompt Message", interactable.promptMessage);
                ShowInfo("EventOnlyInteractable can only use UnityEvents");
                if (interactable.GetComponent<InteractionEvent>() == null) {
                    interactable.useEvents = true;
                    interactable.gameObject.AddComponent<InteractionEvent>();
                }
            } else {
                base.OnInspectorGUI();

                var interactionEvent = interactable.GetComponent<InteractionEvent>();

                if (interactable.useEvents) {
                    if (interactionEvent == null) {
                        interactable.gameObject.AddComponent<InteractionEvent>();
                    }
                } else {
                    if (interactionEvent != null) {
                        DestroyImmediate(interactionEvent);
                    }
                }
            }
        }

        private void ShowInfo(string message) => EditorGUILayout.HelpBox(message, MessageType.Info);
    }
}