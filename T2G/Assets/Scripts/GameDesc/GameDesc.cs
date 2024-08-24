using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;
using System;
using T2G.UnityAdapter;

public class GameDesc : System.Object
{
    public string Name;
    public string Title;
    public string Genre;
    public string ArtStyle;
    public int VersionNumber;
    public int MinorVersionNumber;
    public string Developer;
    public GameProject Project;
    public GameWorld[] GameWorlds;

    public GameDesc()
    {
        Project = new GameProject();
        GameWorlds = new GameWorld[1];
        GameWorlds[0] = new GameWorld();
    }
}

public class GameProject
{
    public string Engine;
    public string Path;
}

public class GameWorld
{
    public string Name;
    public string Ground;
    public string SunLight;
    public string PlayerCharacter;
    public string Camera;
    public string GameGoal;
    public string HUD;
}
