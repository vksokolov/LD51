using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameModeService
{
    private const int DontRepeat = 3;

    private readonly List<GameModeModifier> _modifiers;

    private readonly GameModeModifier _defaultModifier = new EnemySpeedModifier(
        "GAME RULE TITLE",
        "game rule description",
        1);
    public event Action<GameModeInfo> GameModeChanged;
    private readonly Queue<GameModeModifier> _usedModifiers = new Queue<GameModeModifier>();
    
    private GameModeModifier _currentMode;
    private readonly GameModeTextWrapper _gameModeTextWrapper;
    private readonly GameObject _fullScreenFade;
    private readonly TimeService _timeService;
    
    public GameModeService(
        GameModeTextWrapper gameModeTextWrapper,
        TimeService timeService,
        GameObject fullScreenFade)
    {
        _gameModeTextWrapper = gameModeTextWrapper;
        _fullScreenFade = fullScreenFade;
        _modifiers = new List<GameModeModifier>()
        {
            new EnemySpeedModifier("FASTER ENEMIES", "just.. run!", 1.6f),
            new EnemySpeedModifier("SLOWER ENEMIES", "chill a bit :>", .65f),
            new InputModifier("INPUT INVERTED", "@#$%^&*", InputModifier.InvertedAxis.Horizontal | InputModifier.InvertedAxis.Vertical),
            new FuncModifier("GLAUCOMA", "you can't see a thing", TurnOnFade, TurnOffFade),
            new GunJamModifier("JAM", "the bad one", .75f),
        };
        _timeService = timeService;
        Subscribe();
        ApplyDefaultMode();
    }

    public List<GameModeInfo> GetModifierInfos() => _modifiers.Select(x => x.Info).ToList();

    private void Subscribe()
    {
        GameModeChanged += _gameModeTextWrapper.OnGameModeChanged;
        _timeService.TenSecondsPassed += ApplyRandomMode;
    }

    public void Unsubscribe()
    {
        _timeService.TenSecondsPassed -= ApplyRandomMode;
        GameModeChanged = null;
    }

    private void ApplyRandomMode()
    {
        RemoveOldMode();
        ApplyNewMode();
        
        GameModeChanged?.Invoke(_currentMode.Info);
    }

    private void RemoveOldMode()
    {
        if (_currentMode == null) return;
        
        _currentMode.Remove();
        if (_usedModifiers.Count > DontRepeat)
            _modifiers.Add(_usedModifiers.Dequeue());
    }

    private void ApplyNewMode()
    {
        _currentMode = _modifiers.ExtractRandom();
        _currentMode.Apply();
        _usedModifiers.Enqueue(_currentMode);
    }

    private void TurnOnFade() => _fullScreenFade.SetActive(true);
    private void TurnOffFade() => _fullScreenFade.SetActive(false);

    public void Dispose()
    {
        RemoveOldMode();
        Unsubscribe();
        ApplyDefaultMode();
    }

    private void ApplyDefaultMode()
    {
        if (_currentMode != null)
            _currentMode.Remove();
        
        _currentMode = _defaultModifier;
        _currentMode.Apply();
        _currentMode = null;
    }
}
