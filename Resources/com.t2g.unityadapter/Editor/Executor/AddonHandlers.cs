using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;
using System.Threading.Tasks;

namespace T2G.UnityAdapter
{

    [AddAddon("Perspective Camera")]
    public class AddPerspectiveCamera : AddAddonBase
    {
        public override void AddAddon(GameObject gameObject, List<string> properties)
        {
            if (gameObject == null)
            {
                Executor.RespondCompletion(false);
                return;
            }

            var camera = gameObject.AddComponent<Camera>();
            gameObject.AddComponent<AudioListener>();
            camera.orthographic = false;
            for (int i = 0; i < properties.Count - 1; i += 2)
            {
                if (properties[i].CompareTo("-FOV") == 0 && float.TryParse(properties[i + 1], out var fov))
                {
                    camera.fieldOfView = fov;
                }
                if (properties[i].CompareTo("-NEAR") == 0 && float.TryParse(properties[i + 1], out var near))
                {
                    camera.nearClipPlane = near;
                }
                if (properties[i].CompareTo("-FAR") == 0 && float.TryParse(properties[i + 1], out var far))
                {
                    camera.farClipPlane = far;
                }
                if (properties[i].CompareTo("-VIEWPORT_RECT") == 0)
                {
                    var values = Executor.ParseFloat4(properties[i + 1]);
                    camera.rect = new Rect(values[0], values[1], values[2], values[3]);
                }
            }
            Executor.RespondCompletion(true);
        }
    }

    [AddAddon("Directional Light")]
    public class AddDirectionalLight : AddAddonBase
    {
        public override void AddAddon(GameObject gameObject, List<string> properties)
        {
            if (gameObject == null)
            {
                Executor.RespondCompletion(false);
                return;
            }

            var light = gameObject.AddComponent<Light>();
            light.type = LightType.Directional;

            for (int i = 0; i < properties.Count - 1; i += 2)
            {
                if (properties[i].CompareTo("-COLOR") == 0)
                {
                    var values = Executor.ParseFloat3(properties[i + 1]);
                    light.color = new Color(values[0], values[1], values[2]);
                }
                if (properties[i].CompareTo("-INTENSITY") == 0 && float.TryParse(properties[i + 1], out var intensity))
                {
                    light.intensity = intensity;
                }
            }
            Executor.RespondCompletion(true);
        }
    }

    [AddAddon("Primitive")]
    public class AddPrimitive : AddAddonBase
    {
        public override void AddAddon(GameObject gameObject, List<string> properties)
        {
            if (gameObject == null)
            {
                Executor.RespondCompletion(false);
                return;
            }

            float sizeScale = 1.0f;
            GameObject primitiveObject = null;
            for (int i = 0; i < properties.Count - 1; i += 2)
            {
                if (properties[i].CompareTo("-PRIMITIVE_TYPE") == 0)
                {
                    var primitiveTypeName = properties[i + 1];
                    PrimitiveType primitiveType = PrimitiveType.Sphere;

                    if (primitiveTypeName.CompareTo("plane") == 0)
                    {
                        primitiveType = PrimitiveType.Plane;
                    }
                    else if (primitiveTypeName.CompareTo("cube") == 0)
                    {
                        primitiveType = PrimitiveType.Cube;
                    }
                    else if (primitiveTypeName.CompareTo("sphere") == 0)
                    {
                        primitiveType = PrimitiveType.Sphere;
                    }
                    else if (primitiveTypeName.CompareTo("quad") == 0)
                    {
                        primitiveType = PrimitiveType.Quad;
                    }
                    primitiveObject = GameObject.CreatePrimitive(primitiveType);

                }
                if (properties[i].CompareTo("-SIZE_SCALE") == 0 && float.TryParse(properties[i + 1], out var scale))
                {
                    sizeScale = scale;
                }
            }
            primitiveObject.name = gameObject.name;
            primitiveObject.transform.position = gameObject.transform.position;
            primitiveObject.transform.rotation = gameObject.transform.rotation;
            primitiveObject.transform.localScale = gameObject.transform.localScale * sizeScale;
            ExecutionBase.SetCurrentObject(primitiveObject);
            GameObject.DestroyImmediate(gameObject);
            Executor.RespondCompletion(true);
        }
    }

    class AddonAssetPostprocessor : AssetPostprocessor
    {
        public static Action CompletedCallback = null;

        static void OnPostprocessAllAssets(string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths,
            bool didDomainReload)
        {
            //This function may be called multiple times.
            //Check didDomainReload to ensure it is fully completed
            if (didDomainReload)
            {
                CompletedCallback?.Invoke();
            }
        }
    }

    [AddAddon("Script")]
    public class AddScript : AddAddonBase
    {
        [InitializeOnLoadMethod]
        static async void ProcessPendingInsrtuction()
        {
            var scriptName = EditorPrefs.GetString(Defs.k_Pending_AddonScript, null);
            if(string.IsNullOrEmpty(scriptName))
            {
                return;
            }
            EditorPrefs.SetString(Defs.k_Pending_AddonScript, string.Empty);

            //Wait for re-connection from the client
            float timer = 300.0f;  //Wait for 5 minutes 
            while (!CommunicatorServer.Instance.IsConnected)
            {
                await Task.Delay(100);
                timer -= 0.1f;
                if(timer <= 0.0f)
                {
                    Debug.LogError("[AddScipt.ProcessPendingInsrtuction] Timeout waiting for client re-connection.");
                    return; 
                }
            }

            var args = EditorPrefs.GetString(Defs.k_Pending_Arguments, null);
            string[] argsArr = args.Split(",");
            List<string> argList = new List<string>(argsArr);
            
            //Process: Add script addon to gameobject
            var worldName = Executor.GetPropertyValue("-WORLD", ref argList);
            if (!Executor.OpenWorld(worldName))
            {
                Executor.RespondCompletion(false, $"World {worldName} doesn't exist!");
                return;
            }
            var objectName = Executor.GetPropertyValue("-OBJECT", ref argList);
            if (string.IsNullOrEmpty(objectName))
            {
                Executor.RespondCompletion(false, $"Invalid object name!");
                return;
            }
            GameObject gameObject = GameObject.Find(objectName);
            if (gameObject == null)
            {
                Executor.RespondCompletion(false, $"Missiong {objectName} object!");
                return;
            }
            ExecutionBase.SetCurrentObject(gameObject);
            var scriptClassName = Executor.GetScriptClassName(scriptName);
            var type = Type.GetType(scriptClassName);
            var component = gameObject.AddComponent(type);
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var propertyValue = Executor.GetPropertyValue(property.Name, ref argList);
                if(!String.IsNullOrEmpty(propertyValue))
                {
                    Executor.SetPropertyValue(component, property, propertyValue);
                }
            }
        }

        public override void AddAddon(GameObject gameObject, List<string> argList)
        {
            if (gameObject == null)
            {
                Executor.RespondCompletion(false);
            }
            else
            {
                var scriptName = Executor.GetPropertyValue("-SCRIPT", ref argList);
                var dependencies = Executor.GetPropertyValue("-DEPENDENCIES", ref argList);
                bool allReady = Executor.FileExistsInProject(scriptName + ".cs");
                if(allReady)
                {
                    var dependencyFiles = dependencies.Split(",");
                    allReady = Executor.FilesExistInProject(dependencyFiles);
                }

                if (allReady)     //All the script files are already inside the project
                {
                    var scriptClassName = Executor.GetScriptClassName(scriptName);
                    var type = Type.GetType(scriptClassName);
                    var component = gameObject.AddComponent(type);
                    var properties = type.GetProperties();
                    foreach (var property in properties)
                    {
                        var propertyValue = Executor.GetPropertyValue(property.Name, ref argList);
                        if (!String.IsNullOrEmpty(propertyValue))
                        {
                            Executor.SetPropertyValue(component, property, propertyValue);
                        }
                    }
                }
                else
                {
                    string args = argList.Count == 0 ? string.Empty : argList[0];
                    for (int i = 1; i < argList.Count; ++i)
                    {
                        args += "," + argList[i];
                    }
                    EditorPrefs.SetString(Defs.k_Pending_AddonScript, scriptName);
                    EditorPrefs.SetString(Defs.k_Pending_Arguments, args);
                    if (!ContentLibrary.ImportScript(scriptName, dependencies))
                    {
                        Executor.RespondCompletion(false);
                    }
                }
            }
        }
    }
}
