using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private ProgressBar GameModeProgressBar;
    [SerializeField] private GameModeTextWrapper GameModeTextWrapper;
    
    private TimeService _timeService;
    private GameModeService _gameModeService;
    
    private void Awake()
    {
        _timeService = new GameObject(nameof(TimeService)).AddComponent<TimeService>();
        _gameModeService = new GameModeService(GameModeTextWrapper);
        _timeService.TenSecondsPassed += _gameModeService.ApplyRandomMode;
        _timeService
            .SecondsToChangeTheRules
            .ObserveEveryValueChanged(x => x.Value / TimeService.Ten)
            .Subscribe(GameModeProgressBar.SetValue);
    }
}
