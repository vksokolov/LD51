using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    
    public Camera Camera;
    public float Speed;
    public Transform MidPointToCursor;
    public Transform Cursor;
    
    [Range(0,1f)]
    public float CameraLerp;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        Move();
        LookAtCursor();
        RecalculateMidPoint();
    }

    private void Move()
    {
        Vector3 dir = Vector3.zero;
        
        if (Input.GetKey(KeyCode.W))
            dir += Vector3.up;
        
        if (Input.GetKey(KeyCode.A))
            dir += Vector3.left;
        
        if (Input.GetKey(KeyCode.S))
            dir += Vector3.down;
        
        if (Input.GetKey(KeyCode.D))
            dir += Vector3.right;

        transform.position += dir * (Time.deltaTime * Speed);
    }
    
    private void LookAtCursor()
    {
        var mousePos = Input.mousePosition;
        //mousePos.z = 5.23; //The distance between the camera and object
        var objectPos = Camera.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
        var angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    private void RecalculateMidPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = 10;
        mousePoint = Camera.ScreenToWorldPoint(mousePoint);
        
        var position = transform.position;
        MidPointToCursor.position = Vector3.Lerp(position, mousePoint, CameraLerp);
        Cursor.position = mousePoint;
    }
}
