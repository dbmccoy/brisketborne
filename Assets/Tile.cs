using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Tile : MonoBehaviour {

    SpriteRenderer sp;
    Dictionary<string, Sprite> sprites;
    Obj obj;
    TileInfo info;
    public RoomInfo roomInfo;
    public Room Room = null;

    public enum Type
    {
        ground,
        floor
    }

    public Type type;

    public Vector2 Pos;

    public int PosX()
    {
        return Mathf.RoundToInt(Pos.x);
    }
    public int PosY()
    {
        return Mathf.RoundToInt(Pos.y);
    }

    // Use this for initialization
    void Awake() {
        sp = GetComponent<SpriteRenderer>();
    }

    public Sprite Sprite(Sprite s = null)
    {
        if (s) sp.sprite = s;
        return sp.sprite;
    }

    public Obj Obj(Obj o = null)
    {
        if (o) obj = o;
        return obj;
    }

    public void Set(TileInfo _info)
    {
        info = _info;
        sprites = Camera.main.GetComponent<Grid>().Sprites(info.TileSet);
        Sprite(sprites[info.Default]);
        type = Type.floor;
        StartCoroutine(Checks());
    }

    public TileInfo Info()
    {
        return info;
    }

    IEnumerator Checks()
    {
        yield return new WaitForEndOfFrame();
        if (info.MapKey == "d")
        {
            if (Edge() == Utils.Dir.N)
            {
                Get(Edge()).gameObject.AddComponent<Door>();
            }
            else gameObject.AddComponent<Door>();
        }
    }

    bool Match(Tile other)
    {
        if (other.roomInfo.gameID == roomInfo.gameID) return true;
        else return false;
    }

    public Utils.Dir Edge()
    {
        Utils.Dir dir = Utils.Direction()[Vector2.zero];
        var vec = Vector2.zero;

        //Debug.Log(Room.transform == Get(Utils.Dir.N).Room.transform);
        if(!Match(Get(Utils.Dir.N)))
        {
            dir = Utils.Dir.N;
            if (!Match(Get(Utils.Dir.W)))
            {
                dir = Utils.Dir.NW;
                info.traversableTop 
                Sprite(sprites[info.LeftWall]);
            }
            if (!Match(Get(Utils.Dir.E)))
            {
                dir = Utils.Dir.NE;
                Sprite(sprites[info.RightWall]);
            }
            return dir;
        }
        if (!Match(Get(Utils.Dir.S)))
        {
            dir = Utils.Dir.S;
            Sprite(sprites[info.LowerWall]);

            if (!Match(Get(Utils.Dir.W)))
            {
                dir = Utils.Dir.SW;
                Sprite(sprites[info.RightLowerWall]);
            }
            if (!Match(Get(Utils.Dir.E)))
            {
                dir = Utils.Dir.SE;
                Sprite(sprites[info.LeftLowerWall]);
            }
            return dir;
        }

        if (!Match(Get(Utils.Dir.W)))
        {
            dir = Utils.Dir.W;
            Sprite(sprites[info.LeftWall]);
        }

        if (!Match(Get(Utils.Dir.E)))
        {
            dir = Utils.Dir.E;
            Sprite(sprites[info.RightWall]);
        }

        //Utils.VecDirections(cardinal:true).ForEach(x =>
        //{
        //    if (Get(x).Room != Room)
        //    {
        //        vec += x;
        //        Debug.Log(vec);
        //    }
        //    else
        //    {
        //    }
        //});
        //dir = Utils.Direction()[vec];

        return dir;
    }

    public Tile Get(Vector2 v)
    {
        return Camera.main.GetComponent<Grid>().Get(Pos.x + v.x
                                                   ,Pos.y + v.y);
    }

    public Tile Get(Utils.Dir d)
    {
        Vector2 v = Utils.Dir2Vec()[d];
        try
        {
            return Camera.main.GetComponent<Grid>().Get(Pos.x + v.x, Pos.y + v.y);
        }
        catch
        {
            return null;
        }
    }

	
	// Update is called once per frame
	void Update () {
		
	}
}

public class Neighbor
{
    Tile tile;
    int position;

}

[System.Serializable]
public class TileInfo
{
    public string Name;
    public string MapKey;
    public string Biome;
    public string TileSet;
    public string Default;
    public string LeftWall;
    public string RightWall;
    public string LeftUpperWall;
    public string RightUpperWall;
    public string LeftLowerWall;
    public string RightLowerWall;
    public string LowerWall;

    public bool traversableLeft;
    public bool traversableRight;
    public bool traversableTop;
    public bool traversableBottom;

    public void Init(TileInfo info)
    {
        //TileSets Sets = JsonUtility.FromJson<TileSets>(Camera.main.GetComponent<Grid>()
        //                                               .JSON("tiles.json"));
    }
}

[System.Serializable]
public class TileInfos
{
    public TileInfo[] Tiles;

    public TileInfo Get(string key, string biome)
    {
        return Tiles.ToList().Where(x => x.MapKey == key && x.Biome == biome).ToArray()[0];
    }
}

