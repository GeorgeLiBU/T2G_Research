using System;
using System.Collections.Generic;
using System.Text;
using SimpleJSON;

public partial class Interpreter 
{
    static List<string> _instructions = new List<string>();
    static string _currentWorldName;
    static string _currentObjectName;

    public static string CurrentWorldName => _currentWorldName;
    public static string CurrentObjectName => _currentObjectName;

    public static string[] Interpret(string gameDescJson)
    {
        _instructions.Clear();
        if (gameDescJson != null)
        {
            JSONNode gameDescObj = JSON.Parse(gameDescJson);
            Interpret(gameDescObj);
        }

        return _instructions.ToArray();
    }

    static void Interpret(JSONNode jsonNode)
    {
        if (jsonNode.IsNull)
        {
            return;
        }

        if (jsonNode.IsObject)
        {
            if(jsonNode.HasKey(Defs.k_GameDesc_CategoryKeyName))
            {
                InterpretByCategoryName(jsonNode.AsObject);
            }
        }
        if (jsonNode.IsArray)
        {
            var arr = jsonNode.AsArray;
            for(int i = 0; i < arr.Count; ++i)
            {
                Interpret(arr[i]);
            }
        }
        else if(jsonNode.IsBoolean)
        {

        }
        else if(jsonNode.IsNumber)
        {

        }
        else if(jsonNode.IsString)
        {

        }
    }

    static bool InterpretByCategoryName(JSONObject jsonObj)
    {
        var categoryName = jsonObj.GetValueOrDefault(Defs.k_GameDesc_CategoryKeyName, string.Empty);
        if(!categoryName.IsString)
        {
            return false;
        }
        string category = categoryName.Value;

        if (category.CompareTo(Defs.k_GameWorldCategory) == 0)
        {
            if(INS_CreateGameWorld(jsonObj, ref _currentWorldName))
            {
                TODO: CmdCreateProject objects

                var key = jsonObj.Keys.GetEnumerator();
                while (key.MoveNext())
                {
                    Interpret(jsonObj.GetValueOrDefault(key.Current, null));
                    //cal directly InterpretByCategoryName?
                }
            }
        }
        else if (category.CompareTo(Defs.k_WorldObjectCategory) == 0)
        {
            if(INS_CreateObject(jsonObj, ref _currentObjectName))
            {
                TODO:  Add attributes

                var key = jsonObj.Keys.GetEnumerator();
                while (key.MoveNext())
                {
                    Interpret(jsonObj.GetValueOrDefault(key.Current, null));
                }
            }
        }
        else if (category.CompareTo(Defs.k_ObjectAttributeCategory) == 0)
        {

        }
        else if (category.CompareTo(Defs.k_GameDescCategory) == 0)
        {
            JSONNode gameWorlds = jsonObj.GetValueOrDefault(Defs.k_GameDesc_GameWorldsKey, null);
            Interpret(gameWorlds);
        }


        return true;
    }
}
