using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static event Action<KillType> OnKill;
    
    public static float SpeedMultiplier = 1;
    
    public float Speed;

    public event Action OnDie;
    
    // Update is called once per frame
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
        if (!other.CompareTag(Tags.Bullet)) return;
        Die(KillType.Bullet);
    }

    private void Die(KillType killType) 
    {
        OnDie?.Invoke();
        OnKill?.Invoke(killType);
    }

    public void Reset()
    {
        gameObject.SetActive(false);
        OnDie = null;
    }
}
