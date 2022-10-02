using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CinemachineShakeController
{
    private CinemachineVirtualCamera _camera;
    private bool _isShaking;
    public CinemachineShakeController(CinemachineVirtualCamera camera)
    {
        _camera = camera;
    }

    public async void Shake(float intensity, int timeMilliseconds)
    {
        if (_isShaking) return;

        _isShaking = true;
        var perlin = _camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        perlin.m_AmplitudeGain = intensity;
        
        await UniTask.Delay(timeMilliseconds);
        perlin.m_AmplitudeGain = 0;
        _isShaking = false;
    }
}
