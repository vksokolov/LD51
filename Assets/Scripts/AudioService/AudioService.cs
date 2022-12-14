using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class AudioService
{
    private AudioSource _mainSource;
    private AudioSource _gameModeSource;
    private AudioClip _mainClip;
    private Dictionary<GameModeInfo, AudioClip> _modeClips;

    private List<AudioSource> _freeAudioSources = new List<AudioSource>();
    private HashSet<AudioSource> _audioSourcesInUse = new HashSet<AudioSource>();
    private Transform _spawnRoot;
    
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

        _spawnRoot = new GameObject("SpawnedAudioSources").transform;
        _spawnRoot.SetParent(_mainSource.transform);
    }

    public void Start()
    {
        _mainSource.Play();
    }
    
    public void OnGameModeChanged(GameModeInfo info)
    {
        _gameModeSource.Stop();
        if (!_modeClips.TryGetValue(info, out var clip)) return;
        
        _gameModeSource.clip = clip;
        _gameModeSource.Play();
    }

    public async void PlayOneShot(AudioClip clip, float volume)
    {
        var source = GetAudioSource();
        source.volume = volume;
        source.PlayOneShot(clip);
        await UniTask.WaitUntil(() => source == null || !source.isPlaying);
        if (source != null)
            FreeAudioSource(source);
    }

    private AudioSource GetAudioSource()
    {
        AudioSource result;
        if (_freeAudioSources.Count > 0)
        {
            result = _freeAudioSources.ExtractRandom();
        }
        else
        {
            var source = new GameObject("SpawnedAudioSource");
            source.transform.SetParent(_spawnRoot.transform);
            result = source.AddComponent<AudioSource>();
        }
        
        _audioSourcesInUse.Add(result);

        return result;
    }

    private void FreeAudioSource(AudioSource source)
    {
        if (!_audioSourcesInUse.Contains(source))
            Debug.LogError("Source not found");

        _audioSourcesInUse.Remove(source);
        _freeAudioSources.Add(source);
    }

    public void StopAll()
    {
        while(_audioSourcesInUse.Count > 0)
        {
            var obj = _audioSourcesInUse.ElementAt(0);
            _audioSourcesInUse.Remove(obj);
            obj.Stop();
            _freeAudioSources.Add(obj);
        }
        _gameModeSource.Stop();
    }
}
