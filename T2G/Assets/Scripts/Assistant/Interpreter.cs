using System;
using System.Collections.Generic;
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
            Interpret(gameDescObj);
        }

        return _instructions.ToArray();
    }

    static void Interpret(JSONObject jsonObj)
    {
        if (jsonObj == null)
        {
            return;
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

        /*

        foreach (var field in fields)
        {
            if (jsonObj.HasKey(field.Name))
            {
                if (field.FieldType.IsArray)
                {
                    var elementType = field.FieldType.GetElementType();
                    JSONArray jsonArray = jsonObj[field.Name].AsArray;
                    if (jsonArray == null)
                    {
                        field.SetValue(obj, null);
                    }
                    else
                    {
                        int arrLen = jsonArray.Count;
                        var fieldArr = Array.CreateInstance(elementType, arrLen);
                        field.SetValue(obj, fieldArr);
                        for (int i = 0; i < arrLen; ++i)
                        {
                            if (elementType == typeof(string))
                            {
                                fieldArr.SetValue(jsonArray[i].Value, i);
                            }
                            else if (elementType.IsPrimitive)
                            {
                                if (elementType == typeof(float))
                                {
                                    fieldArr.SetValue(jsonArray[i].AsFloat, i);
                                }
                                else if (elementType == typeof(double))
                                {
                                    fieldArr.SetValue(jsonArray[i].AsDouble, i);
                                }
                                else if (elementType == typeof(int))
                                {
                                    fieldArr.SetValue(jsonArray[i].AsInt, i);
                                }
                                else if (elementType == typeof(long))
                                {
                                    fieldArr.SetValue(jsonArray[i].AsLong, i);
                                }
                                else if (elementType == typeof(ulong))
                                {
                                    fieldArr.SetValue(jsonArray[i].AsULong, i);
                                }
                                else if (elementType == typeof(bool))
                                {
                                    fieldArr.SetValue(jsonArray[i].AsBool, i);
                                }
                            }
                            else if (elementType.IsClass)
                            {
                                var jsonElement = jsonArray[i].AsObject;
                                if (jsonElement != null)
                                {
                                    var elementObj = Activator.CreateInstance(elementType);
                                    Interpret(elementObj, jsonElement.AsObject);
                                    fieldArr.SetValue(elementObj, i);
                                }
                                else
                                {
                                    fieldArr.SetValue(null, i);
                                }
                            }
                        }
                    }
                }
                else if (field.FieldType == typeof(string))
                {
                    var strVal = jsonObj[field.Name];
                    field.SetValue(obj, strVal.Value);
                }
                else if (field.FieldType.IsPrimitive)
                {
                    if (field.FieldType == typeof(float))
                    {
                        field.SetValue(obj, jsonObj[field.Name].AsFloat);
                    }
                    else if (field.FieldType == typeof(double))
                    {
                        field.SetValue(obj, jsonObj[field.Name].AsDouble);
                    }
                    else if (field.FieldType == typeof(int))
                    {
                        field.SetValue(obj, jsonObj[field.Name].AsInt);
                    }
                    else if (field.FieldType == typeof(long))
                    {
                        field.SetValue(obj, jsonObj[field.Name].AsLong);
                    }
                    else if (field.FieldType == typeof(ulong))
                    {
                        field.SetValue(obj, jsonObj[field.Name].AsULong);
                    }
                    else if (field.FieldType == typeof(bool))
                    {
                        field.SetValue(obj, jsonObj[field.Name].AsBool);
                    }
                }
                else if (field.FieldType.IsClass)
                {
                    var jsonObject = jsonObj[field.Name].AsObject;
                    if (jsonObj != null)
                    {
                        var fieldObject = Activator.CreateInstance(field.FieldType);
                        Interpret(fieldObject, jsonObj[field.Name].AsObject);
                        field.SetValue(obj, fieldObject);
                    }
                    else
                    {
                        field.SetValue(obj, null);
                    }
                }
            }
        }
        */
    }
}
