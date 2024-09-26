using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;
using System;
using System.IO;
using T2G.UnityAdapter;

public class GameDesc : System.Object
{
    public string Name;
    public string Title;
    public string Genre;
    public string ArtStyle;
    public string Developer;
    public GameProject Project;
    public GameWorld[] GameWorlds;

    public GameDesc()
    {
        Project = new GameProject();
        GameWorlds = new GameWorld[1];
        GameWorlds[0] = new GameWorld();
    }

    public string GetProjectPathName()
    {
        return Path.Combine(Project.Path, Project.Name);
    }

    static public GameDesc GetSample(int sampleId)
    {
        GameDesc gameDesc = new GameDesc();
        switch(sampleId)
        {
            case 0:
            case 1:
                fillUpSampleGameDesc1(gameDesc);
                break;
            default:
                break;
        }
        return gameDesc;
    }

    static void fillUpSampleGameDesc1(GameDesc gameDesc)
    {
        gameDesc.Name = "Marine Shooter";
        gameDesc.Title = "Marine Shooter";
        gameDesc.Genre = "First Person Shooter";
        gameDesc.ArtStyle = "Realistic";
        gameDesc.Developer = "George";
        gameDesc.Project = new GameProject();
        gameDesc.Project.Path = "C:/MyGames";
        gameDesc.Project.Name = "MarineShooter";
        gameDesc.Project.Engine = "Unity";
        gameDesc.GameWorlds = new GameWorld[1];
        gameDesc.GameWorlds[0] = new GameWorld();
        gameDesc.GameWorlds[0].Name = "IlandBattleField";
        gameDesc.GameWorlds[0].GameObjects = new GameObject[5];
        gameDesc.GameWorlds[0].GameObjects[0] = new GameObject("MainCamera");
        gameDesc.GameWorlds[0].GameObjects[0].Components = new ComponentBase[2];
        MainCamera camera = new MainCamera();
        gameDesc.GameWorlds[0].GameObjects[0].Components[0] = camera;
    }

    static void fillUpSampleGameDesc2(GameDesc gameDesc)
    {

    }
}

public class GameProject
{
    public string Engine = "Unity";
    public string Path = "c:\\MyGames";
    public string Name = "MyGame";
}

public class GameWorld
{
    public string Name;
    public GameObject[] GameObjects;


    public string Sky;

    public string Ground;
}

public class GameObject
{
    public string Name;
    public string Tags;
    public float[] Position = new float[3] { 0.0f, 0.0f, 0.0f };
    public float[] Rotation = new float[4] { 0.0f, 0.0f, 0.0f, 1.0f };     //Quertanion
    public float[] Scale = new float[3] { 1.0f, 1.0f, 1.0f };
    public ComponentBase[] Components = null;
    public string[] Scripts = null;
    public GameObject(string name)
    {
        Name = name;
    }
}

public class ComponentBase
{
    public string ComponentType;

    T GetByType<T>()
    {
        var thisType = this.GetType();
        if(typeof(T) == thisType)
        {
            T typeData = (T)Convert.ChangeType(this, typeof(T));
            return typeData;
        }

        return default;
    }
}

public class MainCamera : ComponentBase
{
    public string CameraType = "Perspective";
    public float FieldOfView = 60.0f;
    public float NearClipPlane = 0.1f;
    public float FarClipPlane = 1000.0f;
    public float[] ViewportRect = new float[4] { 0.0f, 0.0f, 1.0f, 1.0f };
    public string LookAtTarget;

    public MainCamera()
    {
        ComponentType = "MainCamera";
    }
}

public class UI
{
    public class HUD
    {

    }

    public class Menu
    {

    }
}

public class Quest
{
    public float Duration;
    public string Goal;
    public string SubGoals;
    public int Collected;
    public int Kills;
    public int Scores;
    public int Destroies;
    public string Destroied;
    public string WinCondition;
    public string LoseCondition;
}