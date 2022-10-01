using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IDisposable
{
    private float _speed;
    private float _lifeTime;
    private Vector3 _dir;

    public void Init(
        Vector3 dir, 
        float speed,
        float lifeTime)
    {
        _dir = dir;
        transform.LookAt2D(dir);
        _speed = speed;
        _lifeTime = lifeTime;
    }

    private void Update()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0)
            Dispose();
        
        transform.position += Time.deltaTime * _speed * _dir;
    }

    public void Dispose()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(Tags.Enemy)) return;
        Dispose();
    }
}
