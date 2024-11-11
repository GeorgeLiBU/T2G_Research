using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Profile;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace T2G.UnityAdapter
{
    public partial class Executor
    {
        private void HandleCreateWorld(ScriptCommand command)
        {
            Action<string, List<string>> setupWorld = (sceneFile, args) => { 
                for(int i = 1; i < args.Count - 1; i += 2)
                {
                    Debug.Log($"{i}: {args[i]}, {args[i + 1]}");
                    if(args[i].CompareTo("-GRAVITY") == 0 && float.TryParse(args[i + 1], out var gravity))
                    {
                        Physics.gravity = Vector3.up * gravity;
                    }
                    if (args[i].CompareTo("-BOOTSTRAP") == 0)
                    {
                        Debug.Log($"Process: {sceneFile}");
                        int startIndex = sceneFile.IndexOf("Assets");
                        if (startIndex > 0)
                        {
                            sceneFile = sceneFile.Substring(startIndex);
                        }
                        Debug.Log($"Process Trimed: {sceneFile}");
                        var sceneList = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
                        if (args[i + 1].ToLower().CompareTo("true") == 0)
                        {
                            sceneList.Insert(0, new EditorBuildSettingsScene(sceneFile, true));
                        }
                        else
                        {
                            sceneList.Add(new EditorBuildSettingsScene(sceneFile, true));
                        }
                        EditorBuildSettings.scenes = sceneList.ToArray();

                        var buildProfile = BuildProfile.GetActiveBuildProfile();
                        if (buildProfile != null)
                        {
                            var profileSceneList = new List<EditorBuildSettingsScene>(buildProfile.scenes);
                            if (args[i + 1].ToLower().CompareTo("true") == 0)
                            {
                                profileSceneList.Insert(0, new EditorBuildSettingsScene(sceneFile, true));
                            }
                            else
                            {
                                profileSceneList.Add(new EditorBuildSettingsScene(sceneFile, true));
                            }
                            buildProfile.scenes = profileSceneList.ToArray();
                        }
                    }
                }
            };

            string scenesPath = Path.Combine(Application.dataPath, "Scenes");
            if (!Directory.Exists(scenesPath))
            {
                Directory.CreateDirectory(scenesPath);
            }
            string sceneFile = Path.Combine(scenesPath, command.Arguments[0] + ".unity");
            if (File.Exists(sceneFile))
            {
                EditorSceneManager.sceneOpened += (scene, mode) => {
                    setupWorld(sceneFile, command.Arguments);
                    Executor.RespondCompletion(true);
                };
                EditorSceneManager.OpenScene(sceneFile, OpenSceneMode.Single);
            }
            else
            {
                EditorSceneManager.newSceneCreated += (scene, setup, mode) =>
                {
                    bool succeeded = EditorSceneManager.SaveScene(scene, sceneFile);
                    setupWorld(sceneFile, command.Arguments);
                    Executor.RespondCompletion(succeeded);
                };
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            }
        }

        private void HandleCreateObject(ScriptCommand command)
        {

        }

        private void HandleAddOn(ScriptCommand command)
        {

        }
    }

}