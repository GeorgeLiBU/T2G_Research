using System.Text;
using SimpleJSON;

public partial class Interpreter
{
    public static bool INS_CreateObject(JSONObject jsonObj, ref string objectName)
    {
        objectName = jsonObj.GetValueOrDefault(Defs.k_GameDesc_NameKey, "WorldObject");
        var position = jsonObj.GetValueOrDefault(Defs.k_GameDesc_PositionKey, "[0, 0, 0]");
        var rotation = jsonObj.GetValueOrDefault(Defs.k_GameDesc_RotationKey, "[0, 0, 0]");
        var scale = jsonObj.GetValueOrDefault(Defs.k_GameDesc_ScaleKey, "[1, 1, 1]");
        string world = Interpreter.CurrentWorldName;

        string instruction = $"CREATE_OBJECT \"{objectName}\" -WORLD {world} -POSITION {position} -ROTATION {rotation} -SCALE {scale}";
        _instructions.Add(instruction);

        var attribs = jsonObj.GetValueOrDefault(Defs.k_GameDesc_AddonsKey, null);
        if (attribs != null)
        {
            Interpret(attribs);
        }
        return true;
    }
   
}
