using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncModifier : GameModeModifier
{
    private readonly Action _apply;
    private readonly Action _remove;
    
    public FuncModifier(
        string name, 
        string description,
        Action apply,
        Action remove) : base(name, description)
    {
        _apply = apply;
        _remove = remove;
    }

    public override void Apply() => _apply();

    public override void Remove() => _remove();
}
