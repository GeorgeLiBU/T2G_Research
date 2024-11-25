using System.Collections.Generic;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace T2G.UnityAdapter
{
    public partial class Executor
    {
        public static void RespondCompletion(bool succeeded, string message = null)
            //Succeeded=[true: code=0(completed), code=1(postponed)]; [false: failed]
        {
            string response = succeeded ? "Done!" : "Failed!";
            if(!string.IsNullOrEmpty(message))
            {
                response += $" {message}";
            }
            CommunicatorServer.Instance.SendMessage(response);
        }

        public static float[] ParseFloat2(string float3String)
        {
            float[] fValue = new float[2] { 0.0f, 0.0f };
            if (float3String.Substring(0, 1).CompareTo("[") == 0 &&
                float3String.Substring(float3String.Length - 1, 1).CompareTo("]") == 0)
            {
                float3String = float3String.Substring(1, float3String.Length - 2);
                var elements = float3String.Split(',');
                if (elements.Length == 2)
                {
                    for (int i = 0; i < 2; ++i)
                    {
                        float.TryParse(elements[i], out fValue[i]);
                    }
                }
            }
            return fValue;
        }

        public static float[] ParseFloat3(string float3String)
        {
            float[] fValue = new float[3] { 0.0f, 0.0f, 0.0f };
            if (float3String.Substring(0, 1).CompareTo("[") == 0 &&
                float3String.Substring(float3String.Length - 1, 1).CompareTo("]") == 0)
            {
                float3String = float3String.Substring(1, float3String.Length - 2);
                var elements = float3String.Split(',');
                if (elements.Length == 3)
                {
                    for (int i = 0; i < 3; ++i)
                    {
                        float.TryParse(elements[i], out fValue[i]);
                    }
                }
            }
            return fValue;
        }

        public static float[] ParseFloat4(string float4String)
        {
            float[] fValue = new float[4] { 0.0f, 0.0f, 0.0f, 0.0f };
            if (float4String.Substring(0, 1).CompareTo("[") == 0 &&
                float4String.Substring(float4String.Length - 1, 1).CompareTo("]") == 0)
            {
                float4String = float4String.Substring(1, float4String.Length - 2);
                var elements = float4String.Split(',');
                if (elements.Length == 4)
                {
                    for (int i = 0; i < 4; ++i)
                    {
                        float.TryParse(elements[i], out fValue[i]);
                    }
                }
            }
            return fValue;
        }

        public static string GetPropertyValue(string propertyName, ref List<string> argList, bool removeProperty = true, int startIndex = 0)
        {
            string value = null;
            if(!string.IsNullOrEmpty(propertyName) && argList != null && argList.Count > 0)
            {
                for(int i = startIndex; i < argList.Count - 1; i += 2)
                {
                    if(argList[i].CompareTo(propertyName) == 0)
                    {
                        value = argList[i + 1];
                        if(removeProperty)
                        {
                            argList.RemoveRange(i, 2);
                        }
                    }
                }
            }
            return value.Trim('"');
        }

        public static string GetScriptClassName(string scriptName)
        {
            int idx = scriptName.IndexOf(".cs");
            if(idx > 0)
            {
                scriptName = scriptName.Substring(0, idx);
            }
            return scriptName;
        }

        public static bool FileExistsInProject(string fileName)
        {
            string[] founds = Directory.GetFiles(Application.dataPath, fileName, SearchOption.AllDirectories);
            return (founds != null && founds.Length > 0);    
        }
        public static bool FilesExistInProject(string[] fileNames)
        {
            bool retVal = true;
            for (int i = 0; i < fileNames.Length; ++i)
            {
                string[] founds = Directory.GetFiles(Application.dataPath, fileNames[i], SearchOption.AllDirectories);
                if(founds == null || founds.Length == 0)
                {
                    retVal = false;
                    break;
                }
            }
            return retVal;
        }

        public static bool OpenWorld(string worldName)
        {
            bool retVal = false;
            if (!string.IsNullOrEmpty(worldName))
            {
                if (EditorSceneManager.GetActiveScene().name.CompareTo(worldName) == 0)
                {
                    retVal = true;
                }
                else
                {
                    //check existance
                    string filePath = Path.Combine(Application.dataPath, "Scenes", $"{worldName}.unity");
                    if (File.Exists(filePath))
                    {
                        EditorSceneManager.OpenScene($"Assets/Scenes/{worldName}.unity");
                    }
                }
            }
            return retVal;
        }

        public static void SetPropertyValue(object component, System.Reflection.PropertyInfo property, string value)
        {
            value = value.Trim('"');
            if(property.PropertyType == typeof(string))
            {
                property.SetValue(component, value);
            }
            else if (property.PropertyType == typeof(float))
            {
                property.SetValue(component, float.Parse(value));
            }
            else if (property.PropertyType == typeof(int))
            {
                property.SetValue(component, int.Parse(value));
            }
            else if (property.PropertyType == typeof(bool))
            {
                property.SetValue(component, bool.Parse(value));
            }
            else if (property.PropertyType == typeof(Vector2))
            {
                var float2 = Executor.ParseFloat2(value);
                Vector2 vector2 = new Vector2(float2[0], float2[1]);
                property.SetValue(component, vector2);
            }
            else if (property.PropertyType == typeof(Vector3))
            {
                var float3 = Executor.ParseFloat3(value);
                Vector3 vector3 = new Vector3(float3[0], float3[1], float3[2]);
                property.SetValue(component, vector3);
            }
            else if (property.PropertyType == typeof(Vector4))
            {
                var float4 = Executor.ParseFloat4(value);
                Vector3 vector4 = new Vector4(float4[0], float4[1], float4[2], float4[3]);
                property.SetValue(component, vector4);
            }
            else if (property.PropertyType == typeof(Color))
            {
                var float4 = Executor.ParseFloat4(value);
                Color color = new Color(float4[0], float4[1], float4[2], float4[3]);
                property.SetValue(component, color);
            }
        }
    }
}
