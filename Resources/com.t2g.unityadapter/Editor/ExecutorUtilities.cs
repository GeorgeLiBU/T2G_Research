using UnityEngine;

namespace T2G.UnityAdapter
{
    public partial class Executor
    {
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
    }
}
