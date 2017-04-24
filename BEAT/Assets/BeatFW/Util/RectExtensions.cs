using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BeatFW.Util
{
    public static class RectExtensions
    {
        public class HorizontalGrid
        {
            public HorizontalGrid(int size)
            {
                Slots = new Rect[size];
                Divisions = new Rect[size];
            }
            public Rect[] Slots;
            public Rect[] Divisions;
        }
        public static Rect MinusMargin(this Rect r, Vector2 margins)
        {
            return new Rect(r.position + margins / 2, r.size - margins);
        }
        public static Rect MinusMargin(this Rect r, float x, float y)
        {
            return MinusMargin(r, new Vector2(x, y));
        }
        public static Rect Resized(this Rect r, Vector2 resize)
        {
            Vector2 newSize = r.size.Apply(resize);
            Vector2 delta = r.size - newSize;
            return r.MinusMargin(delta);
        }
        public static Rect Resized(this Rect r, float x, float y)
        {
            return Resized(r, new Vector2(x, y));
        }
        public static Rect AtOrigin(this Rect r)
        {
            return new Rect(Vector2.zero, r.size);
        }
        public static Rect At(this Rect r, Vector2 position)
        {
            return new Rect(position, r.size);
        }

        public static Rect[] GetHorGridInside(this Rect rect, int length, float spacing)
        {

            Rect[] result = new Rect[length];
            float totalBlankSpace = (spacing * (length - 1));
            var barSize = (rect.size - totalBlankSpace * Vector2.right).Apply(1.0f/length, 1);
            Vector2 currentPos = rect.position;
            float delta  = barSize.x + spacing;
            for (int i = 0; i < length; i++)
            {
                result[i] = new Rect(currentPos, barSize);
                currentPos += Vector2.right * delta;
            }
            return result;
        }
        public static HorizontalGrid GetHorGridInsideWithSpaces(this Rect rect, int length, float spacing)
        {
            var result = new HorizontalGrid(length);
            float totalBlankSpace = (spacing * (length - 1));
            var barSize = (rect.size - totalBlankSpace * Vector2.right).Apply(1.0f / length, 1);
            Vector2 currentPos = rect.position;
            Vector2 delta1 = Vector2.right * barSize.x;
            Vector2 delta2 = Vector2.right * spacing;
            for (int i = 0; i < length; i++)
            {
                result.Slots[i] = new Rect(currentPos, delta1);
                currentPos += delta1;
                result.Divisions[i] = new Rect(currentPos, delta2);
                currentPos += delta2;
            }
            return result;
        }
        static Vector2 Apply(this Vector2 v1, float v2x, float v2y)
        {
            return new Vector2(v1.x * v2x, v1.y * v2y);
        }
        static Vector2 Apply(this Vector2 v1, Vector2 v2)
        {
            return Apply(v1, v2.x, v2.y);
        }
    }
}
