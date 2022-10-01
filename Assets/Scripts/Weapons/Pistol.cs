using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    private const float StartPosOffset = .2f;
    
    private Bullet _bulletPrefab;
    
    public void Init(Bullet bulletPrefab)
    {
        _bulletPrefab = bulletPrefab;
    }

    public void Shoot()
    {
        var bullet = Instantiate(_bulletPrefab);
        bullet.transform.position = transform.position + transform.right * StartPosOffset;
        bullet.Init(transform.right, 10, 10);
    }
}
