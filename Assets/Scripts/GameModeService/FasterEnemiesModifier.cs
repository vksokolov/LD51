using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FasterEnemiesModifier : IGameModeModifier
{
    public string Name => "FASTER ENEMIES";
    
    private float _speedMultiplier;
    private float _defaultValue;

    public FasterEnemiesModifier(float speedMultiplier)
    {
        _speedMultiplier = speedMultiplier;
        _defaultValue = Enemy.SpeedMultiplier;
    }

    public void Apply() => Enemy.SpeedMultiplier = _speedMultiplier;

    public void Remove() => Enemy.SpeedMultiplier = _defaultValue;
}
