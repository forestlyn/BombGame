using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public static class BoxManager
{
    private static List<Box> boxes = new List<Box>();

    public static void Add(Box box)
    {
        boxes.Add(box);
    }
    public static void Remove(Box box) { boxes.Remove(box); }

    public static void Clear()
    {
        boxes.Clear();
    }

    public static bool HasMoving => boxes.Find(b => b.IsMoving) != null;
}

