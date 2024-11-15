using System.Collections.Generic;
using UnityEngine;

namespace T2G.UnityAdapter
{

    [AddAddon("Perspective Camera")]
    public class AddPerspectiveCamera : AddAddonBase
    {
        public override void AddAddon(GameObject gameObject, List<string> properties)
        {
            if(gameObject == null)
            {
                Executor.RespondCompletion(false);
                return;
            }

            var camera = gameObject.AddComponent<Camera>();
            camera.orthographic = false;
            for(int i = 0; i < properties.Count - 1; i += 2)
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
                    Debug.LogWarning($"Rect = {properties[i + 1]}");
                    var values = Executor.ParseFloat4(properties[i + 1]);
                    Debug.LogWarning($"values = {values[0]}, {values[1]}, {values[2]}, {values[3]}");
                    camera.rect = new Rect(values[0], values[1], values[2], values[3]);
                    Debug.LogWarning($"Rect Property value = {values}");
                }
                Executor.RespondCompletion(true);
            }
        }
    }
}
