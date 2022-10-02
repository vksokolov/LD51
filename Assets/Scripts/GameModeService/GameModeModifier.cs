using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameModeModifier
{
    public GameModeInfo Info { get; }
    public GameModeModifier(string name, string description)
    {
        Info = new GameModeInfo(name, description);
    }
    
    public abstract void Apply();
    public abstract void Remove();
}

public struct GameModeInfo
{
    public string Name { get; }
    public string Description { get; }
    
    public GameModeInfo(string name, string description)
    {
        Name = name;
        Description = description;
    }
}
