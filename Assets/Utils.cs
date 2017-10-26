using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {
    public enum Dir
    {
        N,
        S,
        E,
        W,
        NE,
        NW,
        SE,
        SW,
        C
    }

    public static List<Dir> nDirs = new List<Dir>
    {
        Dir.N,
        Dir.NW,
        Dir.NE
    };


    public static List<Dir> DirDirections()
    {
        return new List<Dir>
        {
            Dir.N, Dir.S, Dir.E, Dir.W, Dir.NE, Dir.NW, Dir.SE, Dir.SW, Dir.C
        };
    }

    public static List<Vector2> VecDirections(bool cardinal = false)
    {
        if (!cardinal)
        {

            return new List<Vector2>
            {
                new Vector2(0,1), new Vector2(1,1), new Vector2(1,0),
                new Vector2(1,-1), new Vector2(0,-1), new Vector2(-1,-1),
                new Vector2(-1,0), new Vector2(-1,1)
            };
        }
        else
        {
            return new List<Vector2>
            {
                new Vector2(0,1),
                new Vector2(1,0),
                new Vector2(0,-1),
                new Vector2(-1,0)
            };
        }
    }

    public static Dictionary<Vector2, Dir> Direction()
    {
        Dictionary<Vector2, Dir> d = new Dictionary<Vector2, Dir> {
            { new Vector2(0,1), Dir.N },
            { new Vector2(0,-1), Dir.S },
            { new Vector2(-1,0), Dir.W },
            { new Vector2(1,0), Dir.E },
            { new Vector2(1,1), Dir.NE },
            { new Vector2(-1,1), Dir.NW },
            { new Vector2(-1,-1), Dir.SW },
            { new Vector2(1,-1), Dir.SE },
            { Vector2.zero, Dir.C }
        };

        return d;
    }

    public static Dictionary<Dir, Vector2> Dir2Vec()
    {
            Dictionary<Dir, Vector2> d = new Dictionary<Dir, Vector2> {
            { Dir.N , Vector2.up },
            { Dir.S, Vector2.down },
            { Dir.W, Vector2.left },
            { Dir.E, Vector2.right },
            { Dir.NE, new Vector2(1,1) },
            { Dir.NW, new Vector2(-1,1) },
            { Dir.SW, new Vector2(-1,-1) },
            { Dir.SE, new Vector2(1,-1) },
            { Dir.C, Vector2.zero }
        };

        return d;
    }

}

