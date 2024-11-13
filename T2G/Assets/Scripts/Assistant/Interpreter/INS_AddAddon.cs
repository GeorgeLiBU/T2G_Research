using SimpleJSON;
using System.Text;

public partial class Interpreter
{
    public static bool INS_AddAddon(JSONObject jsonObj, string worldName, string objectName)
    {
        string addonType = jsonObj.GetValueOrDefault(Defs.k_GameDesc_AddonTypeKey, string.Empty);
        if(string.IsNullOrEmpty(addonType))
        {
            return false;
        }
        StringBuilder sb = new StringBuilder($"ADDON - WORLD \"{worldName}\" -OBJECT \"{objectName}\" -TYPE \"{addonType}\"");
        if (addonType.CompareTo("Perspective Camera") == 0)
        {
            var fovAxis = jsonObj.GetValueOrDefault("FOVAxis", string.Empty).ToString();
            var fieldOfView = jsonObj.GetValueOrDefault("FieldOfView", string.Empty).ToString();
            var nearClipPlane = jsonObj.GetValueOrDefault("NearClipPlane", string.Empty).ToString();
            var farClipPlane = jsonObj.GetValueOrDefault("FarClipPlane", string.Empty).ToString();
            var viewportRect = jsonObj.GetValueOrDefault("ViewportRect", string.Empty).ToString();
            if(string.IsNullOrEmpty(fovAxis))
            {
                sb.Append($" -FOVAXIS {fovAxis}");
            }
            if (string.IsNullOrEmpty(fieldOfView))
            {
                sb.Append($" -FOV {fieldOfView}");
            }
            if (string.IsNullOrEmpty(nearClipPlane))
            {
                sb.Append($" -NEAR {nearClipPlane}");
            }
            if (string.IsNullOrEmpty(farClipPlane))
            {
                sb.Append($" -FAR {farClipPlane}");
            }
            if (string.IsNullOrEmpty(viewportRect))
            {
                sb.Append($" -VIEWPORT_RECT {viewportRect}");
            }
        }
        else if (addonType.CompareTo("Third-person View Controller") == 0)
        {
            var offset = jsonObj.GetValueOrDefault("Offset", string.Empty).ToString();
            var lookAtTarget = jsonObj.GetValueOrDefault("Target", string.Empty).ToString();
            var script = jsonObj.GetValueOrDefault("Script", string.Empty).ToString();
            if (!string.IsNullOrEmpty(offset))
            {
                sb.Append($" -OFFSET {offset}");
            }
            if (!string.IsNullOrEmpty(lookAtTarget))
            {
                sb.Append($" -TARGET {lookAtTarget}");
            }
            if (!string.IsNullOrEmpty(script))
            {
                sb.Append($" -SCRIPT {script}");
            }
        }
        else if (addonType.CompareTo("First-person View Controller") == 0)
        {
            var viewOffset = jsonObj.GetValueOrDefault("ViewOffset", "[0, 0, 0]").ToString();
            var script = jsonObj.GetValueOrDefault("Script", string.Empty).ToString();
            if (!string.IsNullOrEmpty(viewOffset))
            {
                sb.Append($" -VIEW_OFFSET {viewOffset}");
            }
            if (!string.IsNullOrEmpty(script))
            {
                sb.Append($" -SCRIPT {script}");
            }
        }
        else if (addonType.CompareTo("Mixed First- and Third-person View Contoller") == 0)
        {
            var viewOffset = jsonObj.GetValueOrDefault("ViewOffset", "[0, 0, 0]").ToString();
            var offset = jsonObj.GetValueOrDefault("Offset", "[0, 0, 0]").ToString();
            var script = jsonObj.GetValueOrDefault("Script", string.Empty).ToString();
            if (!string.IsNullOrEmpty(viewOffset))
            {
                sb.Append($" -VIEW_OFFSET {viewOffset}");
            }
            if (!string.IsNullOrEmpty(offset))
            {
                sb.Append($" -OFFSET {offset}");
            }
            if (!string.IsNullOrEmpty(script))
            {
                sb.Append($" -SCRIPT {script}");
            }
        }
        else if (addonType.CompareTo("DirecionalLight") == 0)
        {
            var color = jsonObj.GetValueOrDefault("Color", "[1, 1, 1]").ToString();
            var intensity = jsonObj.GetValueOrDefault("Intensity", "1").ToString();
            sb.Append($" -COLOR {color}");
            sb.Append($" -INTENSITY {intensity}");
        }
        else if (addonType.CompareTo("Primitive") == 0)
        {
            var primitiveType = jsonObj.GetValueOrDefault("PrimitiveType", "sphere").ToString();
            var sizeScale = jsonObj.GetValueOrDefault("SizeScale", "1").ToString();
            sb.Append($" -PRIMITIVE_TYPE {primitiveType}");
            sb.Append($" -SIZE_SCALE {sizeScale}");
        }


        _instructions.Add(sb.ToString());
        return true;            
    }
}