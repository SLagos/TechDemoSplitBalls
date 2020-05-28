using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class InputManager : Singleton<InputManager>
{
    
    public delegate void DragAction();
    public static event DragAction OnDrag;
    /// <summary>
    /// Value to control the drag, can move from -1 (full left) to 1 (full right)
    /// </summary>
    private float _dragValue;

    public float DragValue => _dragValue;

    [SerializeField]
    private float _dragSensitivity;
    
    

    private Vector3 _startPos;

    public override void Awake()
    {
        base.Awake();
        Input.simulateMouseWithTouches = true;
    }

    private void Update()
    {
        //#if UNITY_ANDROID && !UNITY_EDITOR
        //Commented for testing purposes, reading unput in editor using Unity Remote 5 on device
        if (Input.touchCount > 0)
        {
            switch (Input.touches[0].phase)
            {
                case TouchPhase.Moved:
                    _dragValue += Input.touches[0].deltaPosition.x*_dragSensitivity* Time.deltaTime;
                    _dragValue = Mathf.Clamp(_dragValue, -1f, 1f);
                    OnDrag?.Invoke();
                    //Debug.Log("Drag value: "+_dragValue);
                    break;
            }
        }
        
       // #else
        // _dragValue += Input.GetAxis("Horizontal")*_dragSensitivity* Time.deltaTime;
        // _dragValue = Mathf.Clamp(_dragValue, -1f, 1f);
       // #endif
    }
    public void Restart()
    {
        _dragValue = 0;
    }
}
