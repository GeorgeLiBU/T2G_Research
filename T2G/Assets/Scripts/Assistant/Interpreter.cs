using System;
using System.Collections.Generic;
using System.Text;
using SimpleJSON;

public class Interpreter 
{
    static List<string> _instructions = new List<string>();

    public static string[] Interpret(string gameDescJson)
    {
        _instructions.Clear();
        if (gameDescJson != null)
        {
            JSONObject gameDescObj = JSON.Parse(gameDescJson).AsObject;
            
            //Fake to deserialize to GameDesc instance temporarily.
            //TODO: Use universal parser later
            FakeInterpret(gameDescObj);

            //ParseInterpret(gameDescObj);
        }

        return _instructions.ToArray();
    }

    static void FakeInterpret(JSONObject gameDescObj)
    {
        GameDesc gameDesc = JsonParser.Deseialialize(gameDescObj);
        foreach(var gameWorld in gameDesc.GameWorlds)
        {
            //Create scene
            _instructions.Add($"CREATE_SCENE {gameWorld.Name}");

            //Create game objects
            foreach(var gameObj in gameWorld.SceneObjects)
            {
                if(gameObj == null)
                {
                    continue;
                }

                StringBuilder sb = new StringBuilder($"ADD_OBJECT {gameObj.Name} TO_SCENE {gameWorld.Name}");
                sb.Append($" AT ({gameObj.Position[0]},{gameObj.Position[1]},{gameObj.Position[2]}");
                sb.Append($" ROTATION ({gameObj.Rotation[0]}, {gameObj.Rotation[1]}, {gameObj.Rotation[2]})");
                sb.Append($" SCALE ({gameObj.Scale[0]}, {gameObj.Scale[1]}, {gameObj.Scale[2]})");
                _instructions.Add(sb.ToString());
                foreach(var component in gameObj.Components)
                {
                    if (component.ComponentType.CompareTo("Third-person View") == 0)
                    {

                    }
                    else if (component.ComponentType.CompareTo("First-person View") == 0)
                    {

                    }
                    else if (component.ComponentType.CompareTo("Mixed First- and Third-person View") == 0)
                    {

                    }
                }
            }



        }
    }

    static void ParseInterpret(JSONObject jsonObj)
    {
        if (jsonObj == null)
        {
            return;
        }

        if(jsonObj.IsArray)
        {

        }
        else if(jsonObj.IsBoolean)
        {

        }
        else if (jsonObj.IsNull)
        {

        }
        else if (jsonObj.IsNumber)
        {

        }
        else if (jsonObj.IsObject)
        {

        }
        else if (jsonObj.IsString)
        {

        }
        else
        {

        }


        var enumerator = jsonObj.GetEnumerator();
        
        while(enumerator.MoveNext())
        {
            var field = enumerator.Current;
            if(field.Key == "GameWords")
            {
                _instructions.Add(field.Key + ":" + field.Value);
            }
        }

      
    }
}
