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
                if (elements.Length == 3)
                {
                    for (int i = 0; i < 4; ++i)
                    {
                        float.TryParse(elements[i], out fValue[i]);
                    }
                }
            }
            return fValue;
        }
    }
}
