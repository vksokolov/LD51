using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MonsterSpawner : MonoBehaviour
{
    private Transform _root;
    private Enemy _enemyPrefab;
    private float _secondsToSpawn;
    private float _minDistanceToPlayer;
    private float MinSqrDistanceToPlayer => _minDistanceToPlayer * _minDistanceToPlayer;
    
    private float _acc;

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
        var obj = Instantiate(_enemyPrefab, _root);

        var playerPos = Player.Instance.transform.position;
        var randomPos = (Vector3) Random.insideUnitCircle + playerPos;
        var dist = playerPos - randomPos;
        if (dist.sqrMagnitude < MinSqrDistanceToPlayer)
            randomPos = dist.normalized * _minDistanceToPlayer + playerPos;
        
        obj.transform.position = randomPos;
    }
}
