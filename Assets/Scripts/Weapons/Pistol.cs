using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pistol : MonoBehaviour
{
    public static float JamChance = 0;
    private const float StartPosOffset = .2f;

    [SerializeField] private List<AudioClip> _shotSounds;
    [SerializeField] private List<AudioClip> _jamSounds;
    
    private Bullet _bulletPrefab;
    private AudioService _audioService;
    public event Action OnShoot;
    public event Action OnJam;
    
    public void Init(Bullet bulletPrefab, AudioService audioService)
    {
        _bulletPrefab = bulletPrefab;
        _audioService = audioService;
    }

    public void TryShoot()
    {
        var rand = Random.Range(0, 1f);
        if (rand < JamChance)
            Jam();
        else
            Shoot();
    }

    private void Jam() 
    {
        _audioService.PlayOneShot(_jamSounds.GetRandom(), .5f);
        OnJam?.Invoke();
    }

    private void Shoot()
    {
        var bullet = Instantiate(_bulletPrefab);
        bullet.transform.position = transform.position + transform.right * StartPosOffset;
        bullet.Init(transform.right, 10, 10);
        _audioService.PlayOneShot(_shotSounds.GetRandom(), .1f);
        OnShoot?.Invoke();
    }
}
