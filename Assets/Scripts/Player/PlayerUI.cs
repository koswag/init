using System;
using TMPro;
using UnityEngine;

namespace Player {
    public class PlayerUI : MonoBehaviour {
    
        [SerializeField]
        private TextMeshProUGUI promptText;

        [SerializeField]
        private TextMeshProUGUI healthPointsInterface;

        void Start() {
            UpdatePrompt(string.Empty);
        }

        public void UpdatePrompt(string message) {
            promptText.text = message;
        }

        public void UpdateHealth(float value) {
            healthPointsInterface.text = ((int)value).ToString();
        }
    }
}
