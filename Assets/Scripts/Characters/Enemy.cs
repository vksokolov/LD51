using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static float SpeedMultiplier = 1;
    
    public float Speed;
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
        Die();
    }

    private void Die() 
    {
        Destroy(gameObject);
    }
}
