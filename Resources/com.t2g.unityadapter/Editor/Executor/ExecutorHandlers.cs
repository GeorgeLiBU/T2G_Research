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
        HashSet<Instruction> _instructionBuffer = new HashSet<Instruction>();

        public void PostponseInstruction(Instruction instruction)
        {
            if (!_instructionBuffer.Contains(instruction))
            {
                _instructionBuffer.Add(instruction);
            }
        }
    }

    [Execution("CREATE_WORLD")]
    public class ExecutionCreateWorld : ExecutionBase
    {

        public override void HandleExecution(Executor.Instruction instruction)
        {
            Action<string, List<string>> setupWorld = (sceneFile, args) => {
                for (int i = 1; i < args.Count - 1; i += 2)
                {
                    if (args[i].CompareTo("-GRAVITY") == 0 && float.TryParse(args[i + 1], out var gravity))
                    {
                        Physics.gravity = Vector3.up * gravity;
                    }
                    if (args[i].CompareTo("-BOOTSTRAP") == 0)
                    {
                        int startIndex = sceneFile.IndexOf("Assets");
                        if (startIndex > 0)
                        {
                            sceneFile = sceneFile.Substring(startIndex);
                        }
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
            string sceneFile = Path.Combine(scenesPath, instruction.Arguments[0] + ".unity");
            if (File.Exists(sceneFile))
            {
                EditorSceneManager.sceneOpened += (scene, mode) => {
                    setupWorld(sceneFile, instruction.Arguments);
                    Executor.RespondCompletion(true);
                };
                EditorSceneManager.OpenScene(sceneFile, OpenSceneMode.Single);
            }
            else
            {
                EditorSceneManager.newSceneCreated += (scene, setup, mode) =>
                {
                    bool succeeded = EditorSceneManager.SaveScene(scene, sceneFile);
                    setupWorld(sceneFile, instruction.Arguments);
                    Executor.RespondCompletion(succeeded);
                };
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            }
        }
    }

    [Execution("CREATE_OBJECT")]
    public class ExecutionCreateObject : ExecutionBase
    {
        public override void HandleExecution(Executor.Instruction instruction)
        {
            var args = instruction.Arguments;
            string objName = args[0].Trim('"');
            Vector3 pos = Vector3.zero, rot = Vector3.zero, scale = Vector3.one;
            GameObject newObject = null;
            for (int i = 1; i < instruction.Arguments.Count; i += 2)
            {
                if (args[i].CompareTo("-WORLD") == 0)
                {
                    string worldName = args[i].Trim('"');
                    if (EditorSceneManager.GetActiveScene().name.CompareTo(worldName) != 0)
                    {
                        string worldPathFile = Path.Combine(Application.dataPath, worldName + ".unity");
                        if (File.Exists(worldPathFile))
                        {
                            EditorSceneManager.OpenScene(worldPathFile);
                        }
                        else
                        {
                            Executor.Instance.PostponseInstruction(instruction);
                            continue;
                        }
                    }
                }
                if (args[i].CompareTo("-POSITION") == 0)
                {
                    float[] fValues = Executor.ParseFloat3(args[i + 1]);
                    pos = new Vector3(fValues[0], fValues[1], fValues[2]);
                }
                if (args[i].CompareTo("-ROTATION") == 0)
                {
                    float[] fValues = Executor.ParseFloat3(args[i + 1]);
                    rot = new Vector3(fValues[0], fValues[1], fValues[2]);
                }
                if (args[i].CompareTo("-SCALE") == 0)
                {
                    float[] fValues = Executor.ParseFloat3(args[i + 1]);
                    scale = new Vector3(fValues[0], fValues[1], fValues[2]);
                }
            }
            var newObj = new GameObject(objName);
            newObj.transform.position = pos;
            newObj.transform.Rotate(rot);
            newObj.transform.localScale = scale;
            Executor.RespondCompletion(true);
        }
    }

    [Execution("ADDON")]
    public class ExecutionADDON : ExecutionBase
    {
        public override void HandleExecution(Executor.Instruction instruction)
        {
            var args = instruction.Arguments;
            


            Executor.RespondCompletion(true);
        }
    }
}