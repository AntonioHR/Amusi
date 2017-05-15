using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


[CreateAssetMenu()]
public class Grid: ScriptableObject
{
    public Vector2 drag_debug;
    [Serializable]
    public class Box
    {
        public Vector2 position;
        public string name;
    }

    public Grid()
    {
        boxes = new List<Box>();
    }
    public List<Box> boxes;
    public Vector2 object_size;

    public Box GetByClick(Vector2 click)
    {
        var r = new Rect(Vector2.zero, object_size);
        return boxes.Find(x =>
            {
                r.position = x.position;
                return r.Contains(click);
            });
    }
    public void MoveForward(Box box)
    {
        boxes.Remove(box);
        boxes.Add(box);
    }

    public Vector2 GetDefaultPosition()
    {
        if(boxes.Count == 0)
            return Vector2.zero;
        float d = 1.0f/boxes.Count;
        Vector2 result = Vector2.zero;
        foreach (var box in boxes)
        {
            result += box.position * d;
        }
        //var result = boxes.Aggregate(Vector2.zero, (totalVec, Box) => 
        //    {
        //        Debug.LogFormat("Added {0}, total: {1}", Box.position * d, totalVec);
        //        return totalVec + Box.position * d;
        //    });
        //result.y = -result.y;
        return - result;
    }
}

