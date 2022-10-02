using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioService
{
    private AudioSource _mainSource;
    private AudioSource _gameModeSource;
    private AudioClip _mainClip;
    private Dictionary<GameModeInfo, AudioClip> _modeClips;
    
    public AudioService(
        AudioSource mainSource,
        AudioSource gameModeSource,
        AudioClip mainClip,
        Dictionary<GameModeInfo, AudioClip> modeClips)
    {
        _mainSource = mainSource;
        _gameModeSource = gameModeSource;
        _mainClip = mainClip;
        _modeClips = modeClips;
        
        _mainSource.clip = _mainClip;
        _mainSource.Stop();
    }

    public void Start()
    {
        _mainSource.Play();
    }
    
    public void OnGameModeChanged(GameModeInfo info)
    {
        _gameModeSource.Stop();
        _gameModeSource.clip = _modeClips[info];
        _gameModeSource.Play();
    }
}
