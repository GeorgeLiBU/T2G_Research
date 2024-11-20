using System.Collections.Generic;
using UnityEngine;

namespace T2G.UnityAdapter
{
    public partial class Executor
    {
        public static void RespondCompletion(bool succeeded, int code = 0)   
            //Succeeded=[true: code=0(completed), code=1(postponed)]; [false: failed]
        {
            if (succeeded)
            {
                CommunicatorServer.Instance.SendMessage(code == 0 ? "Done!" : "Postponed!");
            }
            else
            {
                CommunicatorServer.Instance.SendMessage("Failed!");
            }
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

        public static string GetPropertyValue(string propertyName, ref List<string> argList, bool removePropert = true, int startIndex = 0)
        {
            string value = null;
            if(!string.IsNullOrEmpty(propertyName) && argList != null && argList.Count > 0)
            {
                for(int i = startIndex; i < argList.Count - 1; i += 2)
                {
                    if(argList[i].CompareTo(propertyName) == 0)
                    {
                        value = argList[i + 1];
                        if(removePropert)
                        {
                            argList.RemoveRange(i, 2);
                        }
                    }
                }
            }
            return value;
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
    }
}
