using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BeatFW.Util
{
    public static class RectExtensions
    {

        public static Rect MinusMargin(this Rect r, Vector2 margins)
        {
            return new Rect(r.position + margins / 2, r.size - margins);
        }
        public static Rect AtOrigin(this Rect r)
        {
            return new Rect(Vector2.zero, r.size);
        }
    }
}
