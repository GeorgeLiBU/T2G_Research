using System.Collections.Generic;
using UnityEngine;

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
                    Debug.LogWarning($"values = {values[0]}, {values[1]}, {values[2]}, {values[3]}");
                    camera.rect = new Rect(values[0], values[1], values[2], values[3]);
                    Debug.LogWarning($"Rect Property value = {values}");
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
                Debug.LogWarning($"Primitive{i}: {properties[i]}={properties[i+1]}");
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

    [AddAddon("Script")]
    public class AddScript : AddAddonBase
    {
        public override void AddAddon(GameObject gameObject, List<string> properties)
        {
            if (gameObject == null)
            {
                Executor.RespondCompletion(false);
                return;
            }

            //var light = gameObject.AddComponent<Light>();
            //light.type = LightType.Directional;

            //for (int i = 0; i < properties.Count - 1; i += 2)
            //{
            //    if (properties[i].CompareTo("-COLOR") == 0)
            //    {
            //        var values = Executor.ParseFloat3(properties[i + 1]);
            //        Debug.LogWarning($"values = {values[0]}, {values[1]}, {values[2]}, {values[3]}");
            //        light.color = new Color(values[0], values[1], values[2]);
            //    }
            //    if (properties[i].CompareTo("-INTENSITY") == 0 && float.TryParse(properties[i + 1], out var intensity))
            //    {
            //        light.intensity = intensity;
            //    }
            //}
            Executor.RespondCompletion(true);
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

            //var light = gameObject.AddComponent<Light>();
            //light.type = LightType.Directional;

            //for (int i = 0; i < properties.Count - 1; i += 2)
            //{
            //    if (properties[i].CompareTo("-COLOR") == 0)
            //    {
            //        var values = Executor.ParseFloat3(properties[i + 1]);
            //        Debug.LogWarning($"values = {values[0]}, {values[1]}, {values[2]}, {values[3]}");
            //        light.color = new Color(values[0], values[1], values[2]);
            //    }
            //    if (properties[i].CompareTo("-INTENSITY") == 0 && float.TryParse(properties[i + 1], out var intensity))
            //    {
            //        light.intensity = intensity;
            //    }
            //}
            Executor.RespondCompletion(true);
        }
    }

}