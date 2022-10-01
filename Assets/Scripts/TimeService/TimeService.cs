using System;
using UniRx;
using UnityEngine;

public class TimeService : MonoBehaviour
{
    public const int Ten = 10;

    public ReactiveProperty<float> SecondsToChangeTheRules { get; private set; }
    public event Action TenSecondsPassed;

    private void Awake()
    {
        SecondsToChangeTheRules = new ReactiveProperty<float>(Ten);
    }

    private void Update()
    {
        SecondsToChangeTheRules.Value -= Time.deltaTime;
        if (!(SecondsToChangeTheRules.Value <= 0)) return;
        
        SecondsToChangeTheRules.Value += Ten;
        TenSecondsPassed?.Invoke();
    }
}
