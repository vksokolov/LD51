using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputModifier : GameModeModifier
{
    [Flags]
    public enum InvertedAxis
    {
        Horizontal,
        Vertical,
    }
    
    private readonly Dictionary<KeyCode, KeyCode> _map;
    
    public InputModifier(
        string name, 
        string description, 
        InvertedAxis axis) : base(name, description)
    {
        _map = GetMap(axis);
    }

    public override void Apply() =>
        Player.Instance.SetControls(
            _map[KeyCode.W],
            _map[KeyCode.A],
            _map[KeyCode.S],
            _map[KeyCode.D]);

    public override void Remove() => Player.Instance.SetDefaultControls();

    public Dictionary<KeyCode, KeyCode> GetMap(InvertedAxis axis)
    {
        var map = new Dictionary<KeyCode, KeyCode>();

        if (axis.HasFlag(InvertedAxis.Horizontal))
        {
            map.Add(KeyCode.A, KeyCode.D);
            map.Add(KeyCode.D, KeyCode.A);
        }

        if (axis.HasFlag(InvertedAxis.Vertical))
        {
            map.Add(KeyCode.W, KeyCode.S);
            map.Add(KeyCode.S, KeyCode.W);
        }

        return map;
    }
}
