using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class Grid : MonoBehaviour {

    public int xCount;
    public int yCount;

    public Tile TilePrefab;

    RoomInfos roomInfos;
    TileInfos tileInfos;

    // Use this for initialization
    void Start()
    {
        Populate();


        roomInfos = JsonUtility.FromJson<RoomInfos>(JSON("rooms.json"));
        roomInfos.rooms.ToList().ForEach(x =>
        {
            x.Init(x);
            //Debug.Log(x.id);
        });

        tileInfos = JsonUtility.FromJson<TileInfos>(JSON("tiles.json"));
        tileInfos.Tiles.ToList().ForEach(x =>
        {
            //x.Init(x);
            //Debug.Log(x.Name);
        });

        Room(12, 12, 8, 8, "Saloon");

    }


    private RoomInfo info;

    public string JSON(string name)
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, name);


        StreamReader stream = new StreamReader(filePath);

        return stream.ReadToEnd();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    Dictionary<int, List<Tile>> Tiles = new Dictionary<int, List<Tile>>();

    void Populate()
    {
        for (int i = 0; i < xCount; i++)
        {
            List<Tile> l = new List<Tile>();
            for (int j = 0; j < yCount; j++)
            {
                Tile t = Instantiate(TilePrefab);
                t.transform.position = new Vector2(i - (xCount / 2f), j - (yCount / 2f));
                t.Pos = new Vector2(i, j);
                l.Add(t);
            }

            Tiles.Add(i, l);
        }
    }

    void Room(int x, int y, int xSize, int ySize, string biome, string entry = "S")
    {

        List<RoomInfo> validRooms = roomInfos.rooms.Where(r => r.dimensions[0] == xSize
                                                          && r.dimensions[1] == ySize
                                                          && r.entry == entry
                                                          && r.biomes.ToList()
                                                          .Contains(biome)).ToList();

        Room room = gameObject.AddComponent<Room>();
        RoomInfo info = validRooms[Random.Range(0, validRooms.Count())];
        info.xPos = x;
        info.yPos = y;

        info.df.ForEach(l =>
        {
            l.ForEach(i =>
            {
                if (i == "E")
                {
                    info.doorPosX = info.df.IndexOf(l);
                    info.doorPosY = l.IndexOf(i);
                    Debug.Log(info.df.IndexOf(l) + "_" + l.IndexOf(i));
                    x = x - info.doorPosX;
                    y = y - info.doorPosY;
                }
            });
        });

        //Dictionary<int, List<Tile>> tiles = new Dictionary<int, List<Tile>>();
        List<List<Tile>> tiles = new List<List<Tile>>();


        for (int i = 0; i < info.df.Count(); i++)
        {
            List<Tile> l = new List<Tile>();

            for (int j = 0; j < ySize; j++)
            {
                TileInfo tileInfo = tileInfos.Tiles.Where(t => t.MapKey == info.df[i][j]
                                                          ).ToArray()[0];
                var _x = (x + i);// - xSize / 2;
                var _y = (y + j);// - ySize / 2;
                Tile tile = Get(_x, _y);
                tile.Room = room;
                tile.roomInfo = info;
                tile.Set(tileInfo);
                l.Add(tile);
            }
            tiles.Add(l);
            info.Tiles.Add(l);
        }

        TileInfo wallInfo = tileInfos.Get(key: "1", biome: biome);



        int _xSize = 4;
        int _ySize = 4;

        tiles.ForEach(i =>
        {
            i.ForEach(t =>
            {
                Utils.Dir edge = t.Edge();

                if (Utils.nDirs.Contains(edge)) t.Get(Utils.Dir.N).Set(wallInfo);

                if(t.Info().MapKey == "d")
                {
                    Debug.Log(edge);
                    if (edge == Utils.Dir.E) Room(t.PosX() + 1, t.PosY(), _xSize, _ySize, biome, entry:"W");
                    if (edge == Utils.Dir.S) Room(t.PosX(), t.PosY() - 1, _xSize, _ySize, biome, entry: "N");
                    if (edge == Utils.Dir.W) Room(t.PosX() - 1, t.PosY(), _xSize, _ySize, biome, entry: "E");
                    if (edge == Utils.Dir.N) Room(t.PosX(), t.PosY() + 1, _xSize, _ySize, biome, entry: "S");
                }
            });
        });
    }

    public Dictionary<string, Sprite> Sprites(string tileset)
    {
        List<Sprite> sprites = Resources.LoadAll("Sprites/" + tileset,
                                                 typeof(Sprite)).Cast<Sprite>().ToList();

        Dictionary<string, Sprite> dict = new Dictionary<string, Sprite>();

        sprites.ForEach(x =>
        {
            dict.Add(x.name, x);
        });

        return dict;
    }


    public Tile Get(int x, int y)
    {
        //Debug.Log(x + "_" + y);
        return Tiles[x][y];
    }

    public Tile Get(float x, float y)
    {
        int _x = Mathf.RoundToInt(x);
        int _y = Mathf.RoundToInt(y);
        return Tiles[_x][_y];
    }
}

[System.Serializable]
public class RoomInfos
{
    public RoomInfo[] rooms;
}

[System.Serializable]
public class RoomInfo
{
    public int id;
    public int[] dimensions;
    public string entry;
    public int gameID;
    public string[] biomes;
    public string[] map;

    public int xPos, yPos, doorPosX, doorPosY;

    //public Dictionary<int, List<string>> df = new Dictionary<int, List<string>>();
    public List<List<string>> df = new List<List<string>>();

    public void Init(RoomInfo info)
    {
        for (int i = 0; i < info.map[0].Length; i++)
        {
            df.Add(new List<string>());
            info.map.ToList().ForEach(x => df[i].Add(x[i].ToString()));
        }
        gameID = Mathf.RoundToInt(Random.Range(1000, 9999));
        df.ForEach(x => x.Reverse());

    }

    public List<List<Tile>> Tiles = new List<List<Tile>>();

}

public static class MapKey
{
    public static Dictionary<string, Tile> map;
}
