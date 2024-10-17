using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;
using System;
using System.IO;
using T2G.UnityAdapter;

public class GameDesc : System.Object
{
    public string Name;
    public string Title = string.Empty;
    public string Genre = string.Empty;
    public string ArtStyle = string.Empty;
    public string Developer = string.Empty;
    public GameProject Project = new GameProject();
    public string GameStory = string.Empty;
    public GameWorld[] GameWorlds = new GameWorld[1];

    public GameDesc()
    {
        GameWorlds[0] = new GameWorld();
    }

    public string GetProjectPathName()
    {
        return Path.Combine(Project.Path, Project.Name);
    }


    public static string[] SampleGameDescNames =
    {
        "Load Sample ...",
        "Marine Shooter"
    };

    public static bool GetSampleGameDesc(int sampleIndex, ref GameDesc gameDesc)
    {
        bool result = false;
        switch (sampleIndex)
        {
            case 1:
                result = GetSampleGameDesc1(ref gameDesc);
                break;
            default:
                break;
        }
        return result;
    }

    static bool GetSampleGameDesc1(ref GameDesc gameDesc)
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
        gameDesc.GameWorlds[0].SceneObjects = new SceneObject[5];
        gameDesc.GameWorlds[0].SceneObjects[0] = new SceneObject("MainCamera");
        gameDesc.GameWorlds[0].SceneObjects[0].Components = new ComponentBase[2];
        MainCamera camera = new MainCamera();
        gameDesc.GameWorlds[0].SceneObjects[0].Components[0] = camera;
        return true;
    }
}

public class GameProject
{
    public string Engine = "Unity";
    public string Path = "c:/MyGames";
    public string Name = "MyGame";
}

public class GameWorld
{
    public string Name;
    public SceneObject[] SceneObjects;


    public string Sky;

    public string Ground;
}

public class SceneObject
{
    public string Name;
    public string Tags;
    public float[] Position = new float[3] { 0.0f, 0.0f, 0.0f };
    public float[] Rotation = new float[4] { 0.0f, 0.0f, 0.0f, 1.0f };     //Quertanion
    public float[] Scale = new float[3] { 1.0f, 1.0f, 1.0f };
    public ComponentBase[] Components = null;
    public string[] Scripts = null;
    public SceneObject(string name)
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