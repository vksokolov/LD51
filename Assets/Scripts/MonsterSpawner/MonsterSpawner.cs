using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using Random = UnityEngine.Random;

public class MonsterSpawner : MonoBehaviour, IDisposable
{
    private Transform _root;
    private Enemy _enemyPrefab;
    private float _secondsToSpawn;
    private float _minDistanceToPlayer;
    private float MinSqrDistanceToPlayer => _minDistanceToPlayer * _minDistanceToPlayer;
    
    private float _acc;

    private ObjectPool<Enemy, Enemy> _pool;

    public void Init(
        Transform root,
        Enemy enemyPrefab,
        float secondsToSpawn,
        float minDistanceToPlayer)
    {
        _root = root;
        _enemyPrefab = enemyPrefab;
        _secondsToSpawn = secondsToSpawn;
        _minDistanceToPlayer = minDistanceToPlayer;

        _pool = new ObjectPool<Enemy, Enemy>(
            enemy => enemy,
            () => Instantiate(enemyPrefab, _root),
            enemy =>
            {
                enemy.Reset();
            });
    }
    
    private void Update()
    {
        _acc += Time.deltaTime;
        if (_acc < _secondsToSpawn) return;

        _acc -= _secondsToSpawn;
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        var obj = _pool.GetObject();

        var playerPos = Player.Instance.transform.position;
        var randomPos = (Vector3) Random.insideUnitCircle + playerPos;
        var dist = playerPos - randomPos;
        if (dist.sqrMagnitude < MinSqrDistanceToPlayer)
            randomPos = dist.normalized * _minDistanceToPlayer + playerPos;
        
        obj.transform.position = randomPos;
        obj.OnDie += () => OnEnemyDied(obj);
        
        obj.gameObject.SetActive(true);
    }

    private void OnEnemyDied(Enemy enemy)
    {
        _pool.FreeObject(enemy);
    }

    public void Dispose()
    {
        _pool.FreeAll();
    }
}
