using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<KillType> OnKill;
    public static float SpeedMultiplier = 1;
    public event Action OnDie;
    
    [SerializeField] public GameObject NotBloodPrefab;
    public float Speed;
    
    private void Update()
    {
        if (Player.Instance == null) return;
        Move();
        LookAtPlayer();
    }

    private void Move()
    {
        var dir = Player.Instance.transform.position - transform.position;
        transform.position += dir.normalized * (Speed * SpeedMultiplier * Time.deltaTime);
    }
    
    private void LookAtPlayer()
    {
        var playerPos = Player.Instance.transform.position;
        transform.LookAt2D(playerPos);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Bullet))
            Die(KillType.Bullet);
    }

    private void Die(KillType killType)
    {
        var vfx = Instantiate(NotBloodPrefab).transform;
        vfx.SetParent(null);
        vfx.transform.position = transform.position;
        vfx.transform.localRotation = transform.localRotation;
        OnDie?.Invoke();
        OnKill?.Invoke(killType);
    }

    public void Reset()
    {
        gameObject.SetActive(false);
        OnDie = null;
    }
}
