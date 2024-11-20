using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditor;

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
                AddonAssetPostprocessor.CompletedCallback = () =>
                {
                    var scriptClassName = Executor.GetScriptClassName(scriptName);
                    Debug.LogWarning($"5. scriptClassName: {scriptClassName}");
                    var type = Type.GetType(scriptClassName);
                    var component = gameObject.AddComponent(type);
                    var properties = type.GetProperties();
                    foreach (var property in properties)
                    {
                        var propertyValue = Executor.GetPropertyValue(property.Name, ref argList);
                        Debug.LogWarning($"6. property: {property.Name}; property type: {property.PropertyType.ToString()}");
                        if (!string.IsNullOrEmpty(propertyValue))
                        {
                            if (property.PropertyType == typeof(string))
                            {
                                property.SetValue(gameObject, propertyValue);
                            }
                            else if (property.PropertyType == typeof(float))
                            {
                                property.SetValue(gameObject, float.Parse(propertyValue));
                            }
                            else if (property.PropertyType == typeof(int))
                            {
                                property.SetValue(gameObject, int.Parse(propertyValue));
                            }
                            else if (property.PropertyType == typeof(bool))
                            {
                                property.SetValue(gameObject, bool.Parse(propertyValue));
                            }
                            else if (property.PropertyType == typeof(Vector3))
                            {
                                    //property.SetValue(gameObject, int.Parse(propertyValue));
                                }
                            else if (property.PropertyType == typeof(Vector2))
                            {
                                    //property.SetValue(gameObject, int.Parse(propertyValue));
                                }
                            else if (property.PropertyType == typeof(Vector4))
                            {
                                    //property.SetValue(gameObject, int.Parse(propertyValue));
                                }
                            else if (property.PropertyType == typeof(Color))
                            {
                                    //property.SetValue(gameObject, int.Parse(propertyValue));
                                }
                        }
                    }
                    AddonAssetPostprocessor.CompletedCallback = null;
                    Executor.RespondCompletion(true);
                };

                if (!ContentLibrary.ImportScript(scriptName, dependencies))
                {
                    Executor.RespondCompletion(false);
                }
            }
        }
    }

    [AddAddon("Prefab")]
    public class AddPrefab : AddAddonBase
    {
        public override void AddAddon(GameObject gameObject, List<string> properties)
        {
            if (gameObject == null)
            {
                Executor.RespondCompletion(false);
                return;
            }

            Executor.RespondCompletion(true);
        }
    }

}