using System;
using UnityEngine;

public class TimeService : MonoBehaviour
{
    private const int Ten = 10;

    private float SecondsToChangeTheRules { get; set; }
    public event Action TenSecondsPassed;
    
    private void Update()
    {
        SecondsToChangeTheRules -= Time.deltaTime;
        if (!(SecondsToChangeTheRules <= 0)) return;
        
        SecondsToChangeTheRules += Ten;
        TenSecondsPassed?.Invoke();
    }
}
