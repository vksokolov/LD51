using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameModeService
{
    private const int DontRepeat = 0;
    
    private List<GameModeModifier> _modifiers = new List<GameModeModifier>()
    {
        new EnemySpeedModifier("FASTER ENEMIES", "just.. run!", 2f),
        new EnemySpeedModifier("SLOWER ENEMIES", "chill a bit :>", .5f),
        new InputModifier("INPUT INVERTED", "@#$%^&*", InputModifier.InvertedAxis.Horizontal | InputModifier.InvertedAxis.Vertical),
    };

    public event Action<GameModeInfo> GameModeChanged;
    private Queue<GameModeModifier> _usedModifiers = new Queue<GameModeModifier>();
    
    private GameModeModifier _currentMode;
    private GameModeTextWrapper _gameModeTextWrapper;
    
    public GameModeService(GameModeTextWrapper gameModeTextWrapper)
    {
        _gameModeTextWrapper = gameModeTextWrapper;
        GameModeChanged += _gameModeTextWrapper.OnGameModeChanged;
    }
    
    public void ApplyRandomMode()
    {
        if (_currentMode != null)
        {
            _currentMode.Remove();
            if (_usedModifiers.Count > DontRepeat)
                _modifiers.Add(_usedModifiers.Dequeue());
        }
        
        _currentMode = _modifiers.ExtractRandom();
        _currentMode.Apply();
        _usedModifiers.Enqueue(_currentMode);
        
        GameModeChanged?.Invoke(_currentMode.Info);
    }
}
