using SimpleJSON;

public partial class Interpreter
{
    public static bool INS_CreateGameWorld(JSONObject gameWorld, ref string worldName)
    {
        worldName = gameWorld.GetValueOrDefault(Defs.k_GameDesc_NameKey, string.Empty);
        if(string.IsNullOrEmpty(worldName))
        {
            return false;
        }
        _instructions.Add($"CREATE_WORLD \"{worldName}\"");
        return true;
    }
}
