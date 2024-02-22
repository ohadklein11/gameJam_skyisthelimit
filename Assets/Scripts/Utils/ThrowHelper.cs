using UnityEngine;

namespace Utils
{
    public abstract class ThrowHelper
    {
        public static float MagnitudeToReachXYInGravityAtAngle(float x, float y, float g, float ang)
        {
            float sin2Theta = Mathf.Sin(2 * ang * Mathf.Deg2Rad);
            float cosTheta = Mathf.Cos(ang * Mathf.Deg2Rad);
            float inner = (x * x * g) / (x * sin2Theta - 2 * y * cosTheta * cosTheta);
            if (inner < 0)
            {
                return float.NaN;
            }
            float res = Mathf.Sqrt(inner);
            return res;
        }
    }
}