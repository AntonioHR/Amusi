using UnityEngine;

namespace AntonioHR
{
    public static class RectExtensions
    {

        public static Rect MinusMargin(this Rect r, Vector2 margins)
        {
            return new Rect(r.position + margins / 2, r.size - margins);
        }
        public static Rect Resized(this Rect r, Vector2 resize)
        {
            Vector2 newSize = r.size.Apply(resize);
            Vector2 delta = r.size - newSize;
            return r.MinusMargin(delta);
        }
        public static Rect AtOrigin(this Rect r)
        {
            return new Rect(Vector2.zero, r.size);
        }
        public static Rect At(this Rect r, Vector2 position)
        {
            return new Rect(position, r.size);
        }

        public static Rect ShrinkToLeft(this Rect r, float ratio)
        {
            return new Rect(r.position, new Vector2(r.width * ratio, r.height));
        }
        public static Rect ShrinkToRight(this Rect r, float ratio)
        {
            return new Rect(r.position + Vector2.right * r.width * (1- ratio), new Vector2(r.width * ratio, r.height));
        }

        public static Rect ShrinkToLeftAbsolute(this Rect r, float offset)
        {
            return new Rect(r.position, new Vector2(r.width - offset, r.height));
        }
        public static Rect ShrinkToRightAbsolute(this Rect r, float offset)
        {
            return new Rect(r.position + Vector2.right * offset, new Vector2(r.width - offset, r.height));
        }

        public static Rect[] HorizontalBorders(this Rect r, float width)
        {
            var leftBrdr = r.ShrinkToLeftAbsolute(r.width - width);
            var rightBrdr= r.ShrinkToRightAbsolute(r.width - width);
            return new Rect[] { leftBrdr, rightBrdr };
        }
        public static Rect LeftBorder(this Rect r, float width)
        {
            return r.ShrinkToLeftAbsolute(r.width - width);
        }
        public static Rect RightBorder(this Rect r, float width)
        {
            return r.ShrinkToRightAbsolute(r.width - width);
        }
        public static Rect MinusHorizontalBorders(this Rect r, float borderWidth)
        {
            return r.ShrinkToLeftAbsolute(borderWidth).ShrinkToRightAbsolute(borderWidth);
        }

        public static Rect Translated(this Rect r, Vector2 position)
        {
            return new Rect(r.position + position, r.size);
        }
        public static Rect CenteredAt(this Rect r, Vector2 position)
        {
            return new Rect(position - r.size/2, r.size);
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
