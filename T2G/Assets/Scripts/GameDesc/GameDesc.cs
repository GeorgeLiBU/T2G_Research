using UnityEngine;
using SimpleJSON;
using System.Collections.Generic;
using System;
using System.IO;
using T2G.UnityAdapter;

public class SampleGameDescLibrary
{

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
        gameDesc.GameStory = "Create a third-person shooter game. The player controls a marine swat to fight at a millitary zone on a secret island. " +
            "The player character carries an AR15 to attack and kill all enemies on the way to destroy the enemy's base building. This mission requires " +
            "the player to acomplish the task within 10 minutes.";

#region Project settings
        gameDesc.Project = new GameProject();
        gameDesc.Project.Path = "C:/MyGames";
        gameDesc.Project.Name = "MarineShooter";
        gameDesc.Project.Engine = "Unity";
#endregion Project settings

#region Game worlds
        gameDesc.GameWorlds = new GameWorld[1];

#region GameWorld 1
        gameDesc.GameWorlds[0] = new GameWorld();
        gameDesc.GameWorlds[0].Name = "IlandBattleField";
        gameDesc.GameWorlds[0].Objects = new WorldObject[5];

        //Camera
        gameDesc.GameWorlds[0].Objects[0] = new WorldObject();
        gameDesc.GameWorlds[0].Objects[0].Name = "MainCamera";
        gameDesc.GameWorlds[0].Objects[0].Attributes = new Attribute[2];
        PerspectiveCamera camera = new PerspectiveCamera();
        camera.NearClipPlane = 1.0f;
        camera.FarClipPlane = 10000.0f;
        camera.FieldOfView = 50.0f;
        gameDesc.GameWorlds[0].Objects[0].Attributes[0] = camera;
        ThirdPersonCameraController cameraController = new ThirdPersonCameraController();
        cameraController.LookAtTarget = "PlayerCharacter";
        gameDesc.GameWorlds[0].Objects[0].Attributes[1] = cameraController;

        //Sky or background

        //light

        //Background music


        //Play character

        //Camera


        #endregion GameWorld 1
        #endregion Game worlds



        return true;
    }
}

public class GameDesc : System.Object
{
    public string Category = Defs.k_GameDescCategory;
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
}

public class GameProject
{
    public string Engine = "Unity";
    public string Path = "c:/MyGames";
    public string Name = "MyGame";
}

public class GameWorld
{
    public string Category = Defs.k_GameWorldCategory;
    public string Name;
    public WorldObject[] Objects;

    public string Sky;

    public string Ground;

    public UI UI;
}

public class WorldObject
{
    public string Catogory = Defs.k_WorldObjectCategory;
    public string Name = string.Empty;
    public string Tags = string.Empty;
    public float[] Position = new float[3] { 0.0f, 0.0f, 0.0f };
    public float[] Rotation = new float[4] { 0.0f, 0.0f, 0.0f, 1.0f };     //Quertanion
    public float[] Scale = new float[3] { 1.0f, 1.0f, 1.0f };
    public Attribute[] Attributes = null;
}

public class Attribute
{
    public string Catogory = Defs.k_ObjectAttributeCategory;
    public string AttributeType = string.Empty;
    public string Script = string.Empty;
}

public class ThirdPersonCameraController : Attribute
{
    public float[] RelativeOffset = new float[3] { 0.0f, 3.0f, 5.0f }; 
    public string LookAtTarget = string.Empty;

    public ThirdPersonCameraController()
    {
        AttributeType = "Third-person View Controller";
        Script = "ThirdPersonCameraController.cs";
    }
}

public class FirstPersonCameraController : Attribute
{
    public float[] ViewOffset = new float[3] { 0.0f, 1.8f, 0.0f };

    public FirstPersonCameraController()
    {
        AttributeType = "First-person View Controller";
        Script = "FirstPersonCameraController.cs";
    }
}

public class MixedFirstAndThirdPersonCameraController : Attribute
{
    public float[] RelativeOffset = new float[3] { 0.0f, 3.0f, 5.0f };
    public float[] ViewOffset = new float[3] { 0.0f, 1.8f, 0.0f };
    public string LookAtTarget;

    public MixedFirstAndThirdPersonCameraController()
    {
        AttributeType = "Mixed First- and Third-person View Controller";
        Script = "MixedFirstAndThirdPersonCameraController.cs";
    }
}

public class PerspectiveCamera : Attribute
{
    public string FOVAxis = "Vertical";
    public float FieldOfView = 60.0f;
    public float NearClipPlane = 0.1f;
    public float FarClipPlane = 1000.0f;
    public float[] ViewportRect = new float[4] { 0.0f, 0.0f, 1.0f, 1.0f };
    
    public string LookAtTarget;

    public PerspectiveCamera()
    {
        AttributeType = "Perspective Camera";
    }
}

public class UI
{
    public class HUD
    {

    }

    public class Menu
    {
        public string Name;
        public string[] MenuItems;
    }

    public class MenuItem
    {
        public string Name;
        public string Action;

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