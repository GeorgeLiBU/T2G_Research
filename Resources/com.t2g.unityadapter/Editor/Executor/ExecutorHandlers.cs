using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
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
            s_currentObject = null;
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
            s_currentObject = null;
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
            s_currentObject = new GameObject(objName);
            s_currentObject.transform.position = pos;
            s_currentObject.transform.Rotate(rot);
            s_currentObject.transform.localScale = scale;
            Executor.RespondCompletion(true);
        }
    }

    [Execution("ADDON")]
    public class ExecutionADDON : ExecutionBase
    {
        static Dictionary<string, AddAddonBase> s_addonProcessor = new Dictionary<string, AddAddonBase>();

        public static void RegisterAddAddonExecutions()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var executionClasses = assembly.GetTypes()
                .Where(type => type.IsClass && type.GetCustomAttributes(typeof(AddAddonAttribute), false).Any());
            foreach (var executionClass in executionClasses)
            {
                var attribute = executionClass.GetCustomAttribute<AddAddonAttribute>();
                var execution = Activator.CreateInstance (executionClass) as AddAddonBase;
                s_addonProcessor.Add(attribute.AddonType, execution);
            }
        }

        public override void HandleExecution(Executor.Instruction instruction)
        {
            var argList = new List<string>(instruction.Arguments);
            string addonType = GetAddonType(ref argList);
            string worldName = GetWorldName(ref argList);
            string objectName = GetObjectName(ref argList);
            if (!string.IsNullOrEmpty(worldName) && 
                EditorSceneManager.GetActiveScene().name.CompareTo(worldName) != 0)
            {
                EditorSceneManager.OpenScene($"Assets/Scenes/{worldName}.unity");
            }
            if (string.IsNullOrEmpty(objectName))
            {
                Executor.RespondCompletion(false);
                return;
            }
            else
            {
                if (s_currentObject == null || s_currentObject.name.CompareTo(objectName) != 0)
                {
                    s_currentObject = GameObject.Find(objectName);
                    if (s_currentObject == null)
                    {
                        Executor.Instance.PostponseInstruction(instruction);
                        Executor.RespondCompletion(true, 1);
                        return;
                    }
                }
            }

            Debug.Log($"Add addon: {addonType}");
            if(s_addonProcessor.ContainsKey(addonType))
            {
                s_addonProcessor[addonType].AddAddon(s_currentObject, argList);
            }

            Executor.RespondCompletion(true);
        }

        string GetWorldName(ref List<string> argList)
        {
            string worldName = string.Empty;
            for (int i = 0; i < argList.Count - 1; i += 2)
            {
                if(argList[i].CompareTo("-WORLD") == 0)
                {
                    worldName = argList[i + 1];
                    argList.RemoveRange(i, 2);
                    break;
                }
            }
            return worldName;
        }

        string GetObjectName(ref List<string> argList)
        {
            string objectName = string.Empty;
            for (int i = 0; i < argList.Count - 1; i += 2)
            {
                if (argList[i].CompareTo("-OBJECT") == 0)
                {
                    objectName = argList[i + 1];
                    argList.RemoveRange(i, 2);
                    break;
                }
            }
            return objectName;
        }

        string GetAddonType(ref List<string> argList)
        {
            string addonType = string.Empty;
            for (int i = 0; i < argList.Count - 1; i += 2)
            {
                if (argList[i].CompareTo("-TYPE") == 0)
                {
                    addonType = argList[i + 1];
                    argList.RemoveRange(i, 2);
                    break;
                }
            }
            return addonType;
        }
    }
}