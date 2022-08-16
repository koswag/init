using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorState : MonoBehaviour {
    private bool _isOpen;

    public void Start() {
        _isOpen = false;
    }

    public bool SwitchState() {
        _isOpen = !_isOpen;
        return _isOpen;
    }
}
