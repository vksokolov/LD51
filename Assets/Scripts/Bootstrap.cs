using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    private TimeService _timeService;

    private void Awake()
    {
        _timeService = new GameObject(nameof(TimeService)).AddComponent<TimeService>();
    }
}
