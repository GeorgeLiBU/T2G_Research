using System;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace T2G.UnityAdapter
{

    public partial class Executor
    {
        private void HandleCreateWorld(ScriptCommand command)
        {
            string scenesPath = Path.Combine(Application.dataPath, "Scenes");
            if (!Directory.Exists(scenesPath))
            {
                Directory.CreateDirectory(scenesPath);
            }
            string sceneFile = Path.Combine(scenesPath, command.Arguments[0], "unity");
            EditorSceneManager.newSceneCreated += (scene, swtup, mode) =>
            {
                bool succeeded = EditorSceneManager.SaveScene(scene, sceneFile);
                Executor.RespondCompletion(succeeded);
            };
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        }

        private void HandleCreateObject(ScriptCommand command)
        {

        }

        private void HandleAddOn(ScriptCommand command)
        {

        }
    }

}