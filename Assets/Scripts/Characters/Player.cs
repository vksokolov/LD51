using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    private const string AnimatorIsMovingTrigger = "IsMoving";
    private static readonly int IsMoving = Animator.StringToHash(AnimatorIsMovingTrigger);
    public static Player Instance { get; private set; }
    
    [SerializeField] private Camera Camera;
    [SerializeField] private float Speed;
    [SerializeField] private Transform MidPointToCursor;
    [SerializeField] private Transform Cursor;
    [Range(0,1f)]
    [SerializeField] private float CameraLerp;

    private Animator _animator;
    
    [Header("Pistol")]
    public Pistol Pistol;
    public Bullet BulletPrefab;

    public ReactiveProperty<int> Score;
    public event Action OnDie;
    private bool isDead = false;
    
    private enum MovementDirection
    {
        Up,
        Left,
        Down,
        Right,
    }
    
    private Dictionary<MoveDirection, KeyCode> _inputMap;

    public void SetControls(KeyCode up, KeyCode left, KeyCode down, KeyCode right)
    {
        _inputMap = new Dictionary<MoveDirection, KeyCode>()
        {
            {MoveDirection.Up, up},
            {MoveDirection.Left, left},
            {MoveDirection.Down, down},
            {MoveDirection.Right, right},
        };
    }

    public void SetDefaultControls() =>
        SetControls(
            KeyCode.W,
            KeyCode.A,
            KeyCode.S,
            KeyCode.D);
    
    private void Awake()
    {
        Reset();
    }

    public void Reset()
    {
        _animator = GetComponent<Animator>();
        isDead = false;
        Instance = this;
        Pistol.Init(BulletPrefab);
        SetDefaultControls();
        Enemy.OnKill -= OnKill;
        Enemy.OnKill += OnKill;
        Score = new ReactiveProperty<int>(0);
    }

    private void Update()
    {
        if (isDead) return;
        
        Move();
        Shoot();
        LookAtCursor();
        RecalculateMidPoint();
    }

    private void Move()
    {
        Vector3 dir = Vector3.zero;
        
        if (Input.GetKey(_inputMap[MoveDirection.Up]))
            dir += Vector3.up;
        
        if (Input.GetKey(_inputMap[MoveDirection.Left]))
            dir += Vector3.left;
        
        if (Input.GetKey(_inputMap[MoveDirection.Down]))
            dir += Vector3.down;
        
        if (Input.GetKey(_inputMap[MoveDirection.Right]))
            dir += Vector3.right;

        transform.position += dir * (Time.deltaTime * Speed);
        
        _animator.SetBool(IsMoving, dir != Vector3.zero);
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
            Pistol.Shoot();
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(Tags.Enemy)) return;
        Die(KillType.MonsterTouch);
    }

    private void Die(KillType killType)
    {
        isDead = true;
        OnDie?.Invoke();
    }
    
    private void OnKill(KillType killType)
    {
        if (killType != KillType.Bullet) return;
        Score.Value++;
    }
}
