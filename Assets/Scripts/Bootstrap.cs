using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cinemachine;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviour
{
    [Header("Camera")] 
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    [Header("Progress Bar")] 
    [SerializeField] private ProgressBar _gameModeProgressBar;

    [Header("Fade")] 
    [SerializeField] private GameModeTextWrapper _gameModeTextWrapper;
    [SerializeField] private GameObject _fullScreenFade;

    [Header("Score")] 
    [SerializeField] private Transform _scoreWrapper;
    [SerializeField] private TextMeshProUGUI _scoreText;

    [Header("Player")] 
    [SerializeField] private Player _playerPrefab;

    [Header("Monster Spawner")] [SerializeField]
    private Transform _root;

    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private float _secondsToSpawn;
    [SerializeField] private float _minDistanceToPlayer;

    [Header("GameMenu")] [SerializeField] private Transform _gameMenu;
    [SerializeField] private Button _startButton;

    [FormerlySerializedAs("_gameOver")] [Header("GameOver")] [SerializeField] private Transform _gameOverMenu;
    [SerializeField] private Button _toMainMenuButton;

    [Header("Music")] [SerializeField] private AudioSource _mainSource;
    [SerializeField] private AudioSource _modeSource;
    [SerializeField] private AudioClip _mainTrack;
    [SerializeField] private List<AudioClip> _modeTracks;

    private TimeService _timeService;
    private MonsterSpawner _monsterSpawner;
    private AudioService _audioService;
    private GameModeService _gameModeService;
    private Player _player;

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
            .Subscribe(_gameModeProgressBar.SetValue);


        // GameModeService

        _gameModeService = new GameModeService(_gameModeTextWrapper, _timeService, _fullScreenFade);


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

        _player = Instantiate(_playerPrefab);
        _player.Init(
                _mainCamera, 
                _audioService);
        _player.Score
            .ObserveEveryValueChanged(x => x.Value)
            .Subscribe(SetScoreText);
        _player.OnDie += ShowGameOverScreen;
        _player.OnDie += _gameModeService.Dispose;

        // Score

        _scoreWrapper.gameObject.SetActive(true);
        
        // Cinemachine

        _virtualCamera.Follow = _player.MidPointToCursor;
    }

    private void Reset()
    {
        Destroy(_timeService.gameObject);
        Destroy(_player.gameObject);
        
        _gameOverMenu.gameObject.SetActive(false);
        _gameMenu.gameObject.SetActive(true);
        _monsterSpawner.Dispose();
        _audioService.StopAll();
        _gameModeService.Dispose();
        Destroy(_monsterSpawner.gameObject);
    }

    private void SetScoreText(int score)
    {
        _scoreText.text = score.ToString();
        _scoreText.fontSize = score / 1000 + 80;
    }

    private void ShowGameOverScreen()
    {
        _gameOverMenu.gameObject.SetActive(true);
        _toMainMenuButton.onClick.AddListener(Reset);
    }
}
