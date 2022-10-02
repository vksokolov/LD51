using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    
    [Header("GameOver")]
    [SerializeField] private Transform _gameOver;
    [SerializeField] private Button _toMainMenuButton;

    [Header("Music")] 
    [SerializeField] private AudioSource _mainSource;
    [SerializeField] private AudioSource _modeSource;
    [SerializeField] private AudioClip _mainTrack;
    [SerializeField] private List<AudioClip> _modeTracks;
    
    private TimeService _timeService;
    private MonsterSpawner _monsterSpawner;
    private AudioService _audioService;
    private GameModeService _gameModeService;
    
    private void Awake()
    {
        _startButton.onClick.AddListener(StartGame);
    }
    
    public void StartGame()
    {
        _gameMenu.gameObject.SetActive(false);
        
        
        // TimeService
        _timeService = new GameObject(nameof(TimeService)).AddComponent<TimeService>();
        _timeService
            .SecondsToChangeTheRules
            .ObserveEveryValueChanged(x => x.Value / TimeService.Ten)
            .Subscribe(GameModeProgressBar.SetValue);
        
        
        // GameModeService
        
        _gameModeService = new GameModeService(GameModeTextWrapper, _timeService, FullScreenFade);
        
        
        // MonsterSpawner
        
        _monsterSpawner = new GameObject(nameof(MonsterSpawner)).AddComponent<MonsterSpawner>();
        _monsterSpawner.Init(_root, _enemyPrefab, _secondsToSpawn, _minDistanceToPlayer);

        
        // Audio
        
        var modeInfos = _gameModeService.GetModifierInfos();
        var trackDictionary = new Dictionary<GameModeInfo, AudioClip>();
        modeInfos.ForEach(x => trackDictionary.Add(x, _modeTracks[trackDictionary.Count]));
        _audioService = new AudioService(
            _mainSource,
            _modeSource,
            _mainTrack,
            trackDictionary);
        _gameModeService.GameModeChanged += _audioService.OnGameModeChanged;
        _audioService.Start();
        
        // Player
        
        Player.Instance.Reset();
        Player.Instance.Score
            .ObserveEveryValueChanged(x => x.Value)
            .Subscribe(SetScoreText);
        Player.Instance.OnDie += ShowGameOverScreen;
        Player.Instance.OnDie += _gameModeService.Unsubscribe;
        
        // Score
        
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

    private void ShowGameOverScreen()
    {
        _gameOver.gameObject.SetActive(true);
        _toMainMenuButton.onClick.AddListener(() => SceneManager.LoadScene(0));
    }
}
