using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BeatFW.Util
{
    public static class VectorExtensions
    {
        public static Vector2 Apply(this Vector2 v1, float v2x, float v2y)
        {
            return new Vector2(v1.x * v2x, v1.y * v2y);
        }
        public static Vector2 Apply(this Vector2 v1, Vector2 v2)
        {
            return Apply(v1, v2.x, v2.y);
        }

        public static Vector3 Clamped(this Vector3 v, Vector3 min, Vector3 max)
        {
            Vector3 result = v;
            result.x = Mathf.Clamp(result.x, min.x, max.x);
            result.y = Mathf.Clamp(result.y, min.y, max.y);
            result.z = Mathf.Clamp(result.z, min.z, max.z);
            return result;
        }
        public static Vector2 Clamped(this Vector2 v, Vector2 min, Vector2 max)
        {
            Vector2 result = v;
            result.x = Mathf.Clamp(result.x, min.x, max.x);
            result.y = Mathf.Clamp(result.y, min.y, max.y);
            return result;
        }
    }
}
