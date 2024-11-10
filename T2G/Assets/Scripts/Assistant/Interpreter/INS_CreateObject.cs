using System.Text;
using SimpleJSON;

public partial class Interpreter
{
    public static bool INS_CreateObject(JSONObject jsonObj, ref string objectName)
    {
        objectName = jsonObj.GetValueOrDefault(Defs.k_GameDesc_NameKey, "NoNameObject").ToString();
        var position = jsonObj.GetValueOrDefault(Defs.k_GameDesc_PositionKey, string.Empty).ToString();
        var rotation = jsonObj.GetValueOrDefault(Defs.k_GameDesc_RotationKey, string.Empty).ToString();
        var scale = jsonObj.GetValueOrDefault(Defs.k_GameDesc_ScaleKey, string.Empty).ToString();
        var prefab = jsonObj.GetValueOrDefault(Defs.k_GameDesc_PrefabKey, string.Empty).ToString();
        string world = Interpreter.CurrentWorldName;

        StringBuilder sb = new StringBuilder($"CREATE_OBJECT {objectName} -WORLD \"{world}\"");
        if(IsNotEmptyString(position))
        {
            sb.Append($" -POSITION {position}");
        }
        if (IsNotEmptyString(rotation))
        {
            sb.Append($" -ROTATION {rotation}");
        }
        if (IsNotEmptyString(scale))
        {
            sb.Append($" -SCALE {scale}");
        }
        if (IsNotEmptyString(prefab))
        {
            sb.Append($" -PREFAB {prefab}");
        }
        _instructions.Add(sb.ToString());
        objectName = objectName.Trim('"');
        return true;
    }
   
}
