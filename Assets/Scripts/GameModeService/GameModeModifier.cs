using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGameModeModifier
{
    string Name { get; }
    void Apply();
    void Remove();
}
