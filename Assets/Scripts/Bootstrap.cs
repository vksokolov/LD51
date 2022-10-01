﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviour
{
    [Header("Progress Bar")]
    [SerializeField] private ProgressBar GameModeProgressBar;
    
    [Header("Fade")]
    [SerializeField] private GameModeTextWrapper GameModeTextWrapper;
    [SerializeField] private GameObject FullScreenFade;
    
    [Header("Score")]
    [SerializeField] private Transform ScoreWrapper;
    [SerializeField] private TextMeshProUGUI ScoreText;
    
    [Header("Monster Spawner")]
    [SerializeField] private Transform _root;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private float _secondsToSpawn;
    [SerializeField] private float _minDistanceToPlayer;

    [Header("GameMenu")] 
    [SerializeField] private Transform _gameMenu;
    [SerializeField] private Button _startButton;
    
    private TimeService _timeService;
    private MonsterSpawner _monsterSpawner;
    private GameModeService _gameModeService;
    
    private void Awake()
    {
        _startButton.onClick.AddListener(StartGame);
    }
    
    public void StartGame()
    {
        _gameMenu.gameObject.SetActive(false);
        
        _timeService = new GameObject(nameof(TimeService)).AddComponent<TimeService>();
        _gameModeService = new GameModeService(GameModeTextWrapper, _timeService, FullScreenFade);
        _timeService
            .SecondsToChangeTheRules
            .ObserveEveryValueChanged(x => x.Value / TimeService.Ten)
            .Subscribe(GameModeProgressBar.SetValue);
        _monsterSpawner = new GameObject(nameof(MonsterSpawner)).AddComponent<MonsterSpawner>();
        _monsterSpawner.Init(_root, _enemyPrefab, _secondsToSpawn, _minDistanceToPlayer);
        
        Player.Instance.Reset();
        Player.Instance.Score
            .ObserveEveryValueChanged(x => x.Value)
            .Subscribe(SetScoreText);
        
        ScoreWrapper.gameObject.SetActive(true);
    }

    private void Reset()
    {
        Destroy(_timeService.gameObject);
        Destroy(_monsterSpawner.gameObject);
    }

    private void SetScoreText(int score)
    {
        ScoreText.text = score.ToString();
        ScoreText.fontSize = score / 1000 + 80;
    }
}
