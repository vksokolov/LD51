using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeService : MonoBehaviour
{
    private List<IGameModeModifier> _modifiers = new List<IGameModeModifier>()
    {
        new FasterEnemiesModifier(1.5f)
    };
}
