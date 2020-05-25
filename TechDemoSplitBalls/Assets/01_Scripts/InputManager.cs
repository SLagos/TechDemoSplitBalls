using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    
    public delegate void DragAction(float drag);
    public static event DragAction OnDrag;
    /// <summary>
    /// Value to control the drag, can move from -1 (full left) to 1 (full right)
    /// </summary>
    private float _dragValue;

    [SerializeField]
    private float _dragSensitivity;

    private Vector3 _startPos;

    private void Awake()
    {
        Input.simulateMouseWithTouches = true;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            switch (Input.touches[0].phase)
            {
                case TouchPhase.Moved:
                    _dragValue += Input.touches[0].deltaPosition.x*_dragSensitivity;
                    _dragValue = Mathf.Clamp(_dragValue, -1f, 1f);
                    OnDrag?.Invoke(_dragValue);
                    Debug.Log("Drag value: "+_dragValue);
                    break;
            }
        }
    }
}
