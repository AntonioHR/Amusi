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
                Divisions = new Rect[size-1];
                isValid = new bool[size];
            }
            public Rect[] Slots;
            public Rect[] Divisions;
            public bool[] isValid;
        }



        public static Rect MinusMargin(this Rect r, Vector2 margins)
        {
            return new Rect(r.position + margins / 2, r.size - margins);
        }
        public static Rect MinusMargin(this Rect r, float x, float y)
        {
            return MinusMargin(r, new Vector2(x, y));
        }
        public static Rect MinusMargin(this Rect r, float left, float top, float right, float bottom)
        {
            return new Rect(r.position + new Vector2(left, top), r.size - new Vector2(left+right, top+bottom));
        }

        public static Rect Intersection(this Rect r1, Rect r2)
        {
            float xMin = Mathf.Max(r1.xMin, r2.xMin);
            float yMin = Mathf.Max(r1.yMin, r2.yMin);

            float xSize = Mathf.Min(r1.xMax, r2.xMax) - xMin;
            float ySize = Mathf.Min(r1.yMax, r2.yMax) - yMin;

            return new Rect(xMin, yMin, xSize, ySize);
        }

        public static bool Drawable(this Rect r)
        {
            return r.width > 0 && r.height > 0;
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

        public static HorizontalGrid GetHorizontalGridInside(this Rect rect, int length, float spacing, Vector2 margin)
        {
            var result = new HorizontalGrid(length);

            var outside = rect;
            var inside = rect.MinusMargin(margin);

            float totalBlankSpace = (spacing * (length - 1));

            var slotSize = (inside.size - totalBlankSpace * Vector2.right).Apply(1.0f / length, 1);
            var divisionSize = new Vector2(spacing, outside.height);

            Vector2 delta = Vector2.right * (slotSize.x + spacing);
            Vector2 slotPos = inside.position;
            Vector2 divPos = outside.position + Vector2.right * slotSize.x;
            for (int i = 0; i < length; i++)
            {
                result.isValid[i] = true;
                result.Slots[i] = new Rect(slotPos, slotSize);
                if (i < length - 1)
                {
                    result.Divisions[i] = new Rect(divPos, divisionSize);
                }
                slotPos += delta;
                divPos += delta;
            }
            return result;
        }
        public static HorizontalGrid GetMaskedHorizontalGridInside(this Rect rect, float offset, int length, float slotWidth, float spacing, Vector2 margin, float minWidth = 10)
        {
            var result = new HorizontalGrid(length);

            var outside = rect.At(rect.position + Vector2.left * offset);
            var inside = outside.MinusMargin(margin);
            var view = rect.MinusMargin(margin);

            float totalBlankSpace = (spacing * (length - 1));

            //var slotSize = (inside.size - totalBlankSpace * Vector2.right).Apply(1.0f / length, 1);
            var slotSize = new Vector2(slotWidth, inside.height);
            var divisionSize = new Vector2(spacing, outside.height);

            Vector2 delta = Vector2.right * (slotSize.x + spacing);
            Vector2 slotPos = inside.position;
            Vector2 divPos = outside.position + Vector2.right * slotSize.x;
            for (int i = 0; i < length; i++)
            {
                result.Slots[i] = new Rect(slotPos, slotSize).Intersection(view);
                result.isValid[i] = result.Slots[i].width >= minWidth;
                if (i < length - 1)
                {
                    result.Divisions[i] = new Rect(divPos, divisionSize).Intersection(view);
                }
                slotPos += delta;
                divPos += delta;
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
