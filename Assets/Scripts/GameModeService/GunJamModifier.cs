using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunJamModifier : GameModeModifier
{
    private const float DefaultJamChance = 0;
    
    private float _jamChance;
    
    public GunJamModifier(string name, string description, float jamChance) : base(name, description)
    {
        _jamChance = jamChance;
    }

    public override void Apply() => Pistol.JamChance = _jamChance;

    public override void Remove() => Pistol.JamChance = DefaultJamChance;
}
