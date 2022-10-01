using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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
        transform.position += dir.normalized * (Speed * Time.deltaTime);
    }
    
    private void LookAtPlayer()
    {
        var playerPos = Player.Instance.transform.position;
        transform.LookAt2D(playerPos);
    }
}
