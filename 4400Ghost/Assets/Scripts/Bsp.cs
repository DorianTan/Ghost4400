using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Bsp : MonoBehaviour
{
    //Option 
    [Range(0, 50)] [SerializeField] int sizeX;
    [Range(0, 50)] [SerializeField] int sizeY;

    [Range(0, 50)] [SerializeField] int roomSizeX;
    [Range(0, 50)] [SerializeField] int roomSizeY;

    public Tilemap tilemap;
    public Tilemap tilemapWall;
    public Tile tile;

    private List<Room> leafRoom;
    private List<Vector3Int> doors;

    private List<Color> colors;

    [SerializeField] GameObject Object;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject NPC;



    struct Cell
    {
        public int room;
        public bool isNotFree;
        public Vector2Int position;

    }

    private Cell[,] cells;
    //Struct
    struct Room
    {
        public Vector2 center;
        public Vector2 extends;
        public Vector2 doorPosition;
        public List<Room> children;
        public List<Cell> cell;
        public string name;
    }

    Room _RootRoom;

    public void Generate()
    {
        leafRoom = new List<Room>();
        doors = new List<Vector3Int>();
        _RootRoom.extends = new Vector2(sizeX * 2, sizeY * 2);
        _RootRoom.center = Vector2.zero;
        _RootRoom.children = new List<Room>();
        _RootRoom.cell = new List<Cell>();

        _RootRoom.children.AddRange(CheckDivision(_RootRoom));
        _RootRoom = CreateDoor(_RootRoom);
        
        
    }

    void GenerateRoom(Room rootRoom)
    {
        if (rootRoom.children.Count > 0)
        {
            foreach (Room room in rootRoom.children)
            {
                GenerateRoom(room);
            }
        }
        else
        {
            leafRoom.Add(rootRoom);
        }

        doors.Add(Vector3Int.FloorToInt(rootRoom.doorPosition));

    }

    public void GenerateObjectInteract()
    {
        foreach (Room leaf in leafRoom)
        {
            Vector2Int freePosition = GetSpawn(leaf.cell);
            Debug.Log(freePosition);
            Instantiate(Object,new Vector3(freePosition.x+0.5f,freePosition.y+0.5f),Quaternion.identity);
            cells[freePosition.x, freePosition.y].isNotFree = true;

            freePosition = GetSpawn(leaf.cell);
            Debug.Log(freePosition);
            Instantiate(Object, new Vector3(freePosition.x + 0.5f, freePosition.y + 0.5f), Quaternion.identity);
            cells[freePosition.x, freePosition.y].isNotFree=true;
        }
    }

    private Vector2Int GetSpawn(List<Cell>LeafCell)
    {
        for (int i = 0; i < 10000; i++) // if 10000 turn around and don't find any spawn
        {
            Cell cell = LeafCell[Random.Range(0, LeafCell.Count)];
            BoundsInt bounds = new BoundsInt(-1, -1, 0, 3, 3, 1);
            int freecell = 0;
            foreach (Vector3Int b in bounds.allPositionsWithin)
            {
                if (cell.position.x + b.x < 0 || cell.position.x - b.x > sizeX*2)
                    break;
                if (cell.position.y + b.y < 0 || cell.position.y - b.y > sizeY*2)
                    break;
                if (cells[cell.position.x+b.x,cell.position.y+b.y].isNotFree|| cells[cell.position.x + b.x, cell.position.y + b.y].room<0)
                break;
                freecell++;
            }
            if (freecell==9)return cell.position;
            
        }
        Debug.Log("Spawn not found");
        return Vector2Int.zero; //Spawn not found
        
    }

    [SerializeField] private int Seed;


    private void Start()
    {
        cells = new Cell[sizeX * 2, sizeY * 2];
        for (int x = 0; x < sizeX * 2; x++)
        {
            for (int y = 0; y < sizeY * 2; y++)
            {
                cells[x, y].room = -1;
            }
        }

        if (Seed==0)
        {
            Seed = Random.Range(1, 30000);
        }
        Random.InitState(Seed);


        colors = new List<Color> {
            new Color(1, 0, 0, 0.2f),
            new Color(0, 0, 1, 0.2f),
            new Color(0, 1, 0, 0.2f),
            new Color(1, 0, 1, 0.2f),
            new Color(0, 1, 1, 0.2f),
            new Color(1, 1, 0, 0.2f),
            new Color(0.5f, 1, 1, 0.2f),
            new Color(1, 0.5f, 0, 0.2f),
            new Color(0, 0, 0.5f, 0.2f),
            new Color(0, 0.5f, 0, 0.2f),
            new Color(0.5f, 0, 1, 0.2f),
            new Color(0, 0.5f, 1, 0.2f),
            new Color(1, 1, 0.5f, 0.2f),
            new Color(0.5f, 0.5f, 1, 0.2f),
            new Color(0.5f, 0.5f, 0, 0.2f),
            new Color(0, 0.5f, 0.5f, 0.2f),
            new Color(0, 0.5f, 0.5f, 0.2f),
            new Color(0.5f, 0, 0.5f, 0.2f),
            new Color(0.5f, 0.5f, 1, 0.2f),
            new Color(1, 0.5f, 0.5f, 0.2f),
            new Color(0.5f, 0.5f, 0.5f, 0.2f),
        };
        //tilemap.gameObject.transform.position = Vector3Int.RoundToInt(_RootRoom.extends * -1);
        Generate();
        GenerateRoom(_RootRoom);
        GenerateCells();
        GenerateTile();
        GenerateObjectInteract();
        GeneratePlayerAndNPC();
    }


    public void GenerateCells()
    {
        int currentRoom = 0;
        foreach (Room leaf in leafRoom)
        {
            BoundsInt bounds = new BoundsInt(Mathf.RoundToInt(leaf.center.x - leaf.extends.x * 0.5f), Mathf.RoundToInt(leaf.center.y - leaf.extends.y * 0.5f), 0, Mathf.RoundToInt(leaf.extends.x), Mathf.RoundToInt(leaf.extends.y), 1);

            foreach (Vector3Int b in bounds.allPositionsWithin)
            {
                Vector3Int floor = Vector3Int.RoundToInt(new Vector3(b.x, b.y, 0));
                floor -= Vector3Int.RoundToInt(_RootRoom.extends * -0.5f);

                if ((floor.x >= 0 && floor.x < sizeX * 2) && (floor.y >= 0 && floor.y < sizeY * 2))
                {
                    cells[floor.x, floor.y].room = currentRoom;
                    cells[floor.x, floor.y].position = new Vector2Int(floor.x, floor.y);
                }
            }
            for (int i = 0; i < 100; i++)
            {
                Vector3Int wall = new Vector3Int();
                wall = Vector3Int.RoundToInt(Vector2.Lerp(new Vector2(leaf.center.x + leaf.extends.x * 0.5f, leaf.center.y + leaf.extends.y * 0.5f), new Vector2(leaf.center.x + leaf.extends.x * 0.5f, leaf.center.y - leaf.extends.y * 0.5f), i / 100f));
                wall -= Vector3Int.RoundToInt(_RootRoom.extends * -0.5f);
                if (wall.x < 0) wall.x = 0;
                if (wall.y < 0) wall.y = 0;
                if (wall.x >= sizeX * 2) wall.x = sizeX * 2 - 1;
                if (wall.y >= sizeY * 2) wall.y = sizeY * 2 - 1;
                cells[wall.x, wall.y].isNotFree = true;
                cells[wall.x, wall.y].position = new Vector2Int(wall.x, wall.y);
                wall = Vector3Int.RoundToInt(Vector2.Lerp(new Vector2(leaf.center.x + leaf.extends.x * 0.5f, leaf.center.y + leaf.extends.y * 0.5f), new Vector2(leaf.center.x - leaf.extends.x * 0.5f, leaf.center.y + leaf.extends.y * 0.5f), i / 100f));
                wall -= Vector3Int.RoundToInt(_RootRoom.extends * -0.5f);
                if (wall.x < 0) wall.x = 0;
                if (wall.y < 0) wall.y = 0;
                if (wall.x >= sizeX * 2) wall.x = sizeX * 2 - 1;
                if (wall.y >= sizeY * 2) wall.y = sizeY * 2 - 1;
                cells[wall.x, wall.y].isNotFree = true;
                cells[wall.x, wall.y].position = new Vector2Int(wall.x, wall.y);
                wall = Vector3Int.RoundToInt(Vector2.Lerp(new Vector2(leaf.center.x - leaf.extends.x * 0.5f, leaf.center.y - leaf.extends.y * 0.5f), new Vector2(leaf.center.x + leaf.extends.x * 0.5f, leaf.center.y - leaf.extends.y * 0.5f), i / 100f));
                wall -= Vector3Int.RoundToInt(_RootRoom.extends * -0.5f);
                if (wall.x < 0) wall.x = 0;
                if (wall.y < 0) wall.y = 0;
                if (wall.x >= sizeX * 2) wall.x = sizeX * 2 - 1;
                if (wall.y >= sizeY * 2) wall.y = sizeY * 2 - 1;
                cells[wall.x, wall.y].isNotFree = true;
                cells[wall.x, wall.y].position = new Vector2Int(wall.x, wall.y);
                wall = Vector3Int.RoundToInt(Vector2.Lerp(new Vector2(leaf.center.x - leaf.extends.x * 0.5f, leaf.center.y - leaf.extends.y * 0.5f), new Vector2(leaf.center.x - leaf.extends.x * 0.5f, leaf.center.y + leaf.extends.y * 0.5f), i / 100f));
                wall -= Vector3Int.RoundToInt(_RootRoom.extends * -0.5f);
                if (wall.x < 0) wall.x = 0;
                if (wall.y < 0) wall.y = 0;
                if (wall.x >= sizeX * 2) wall.x = sizeX * 2 - 1;
                if (wall.y >= sizeY * 2) wall.y = sizeY * 2 - 1;
                cells[wall.x, wall.y].isNotFree = true;
                cells[wall.x, wall.y].position = new Vector2Int(wall.x, wall.y);

            }
            currentRoom++;
        }
        foreach (Vector3Int door in doors)
        {
            BoundsInt newBounds = new BoundsInt(-1, -1, 0, 3, 3, 1);
            foreach (Vector2Int b in newBounds.allPositionsWithin)
            {
                Vector3Int newDoor = door - Vector3Int.RoundToInt(_RootRoom.extends * -0.5f);

                if ((newDoor.x + b.x >= 0 && newDoor.x + b.x < sizeX * 2) && (newDoor.y + b.y >= 0 && newDoor.y + b.y < sizeY * 2))
                {
                    cells[newDoor.x + b.x, newDoor.y + b.y].room = -1;
                    cells[newDoor.x + b.x, newDoor.y + b.y].isNotFree = false;
                }
            }
        }

        foreach (Cell cell in cells)
        {
            if (cell.room>=0)
            {
                leafRoom[cell.room].cell.Add(cell);
            }
        }

    }

    public void GenerateTile()
    {
        for (int x = 0; x < sizeX * 2; x++)
        {
            for (int y = 0; y < sizeY * 2; y++)
            {
                if (cells[x, y].isNotFree)
                {
                    tile.color = new Color(0, 0, 0);
                    tilemapWall.SetTile(new Vector3Int(x, y, 0), tile);
                }
                else
                {
                    //Debug.Log(cells[x, y].room);
                    tile.color = cells[x, y].room < 0 ? Color.white : colors[cells[x, y].room % colors.Count];
                    tilemap.SetTile(new Vector3Int(x, y, 0), tile);
                }
            }
        }

    }
    public void Clear()
    {
        _RootRoom = new Room();
        tilemap.ClearAllTiles();
    }

    List<Room> CheckDivision(Room room)
    {
        List<Room> childrenList = new List<Room>();

        if (room.extends.x > roomSizeX * 2 && room.extends.y > roomSizeY * 2)
        {
            childrenList.AddRange(DivideByProbability(room));
        }
        else if (room.extends.x > roomSizeX * 2)
        {
            childrenList.AddRange(DivideByX(room));
        }
        else if (room.extends.y > roomSizeY * 2)
        {
            childrenList.AddRange(DivideByY(room));
        }

        return childrenList;
    }

    List<Room> DivideByProbability(Room room)
    {
        float probability = Random.Range(0f, 1f);
        //float probability = RandomSeed.GetValue();

        return probability > 0.5 ? DivideByX(room) : DivideByY(room);
    }

    List<Room> DivideByX(Room room)
    {
        List<Room> rooms = new List<Room>();

        Room roomLeft = new Room();
        Room roomRight = new Room();

        //Value for cut
        float posX = Random.Range(0 + roomSizeX * 0.5f, room.extends.x - roomSizeX * 0.5f);
        //float posX = RandomSeed.GetValue() * (room.extends.x - roomSizeX * 0.5f) + roomSizeX * 0.5f;

        //Extends
        roomRight.extends = new Vector2(posX, room.extends.y);
        roomLeft.extends = new Vector2(room.extends.x - posX, room.extends.y);

        //Center
        roomRight.center = new Vector2(room.center.x + room.extends.x * 0.5f - roomRight.extends.x * 0.5f, room.center.y);
        roomLeft.center = new Vector2(room.center.x - room.extends.x * 0.5f + roomLeft.extends.x * 0.5f, room.center.y);

        //Children
        roomRight.children = new List<Room>();
        roomRight.cell = new List<Cell>();
        roomLeft.children = new List<Room>();
        roomLeft.cell = new List<Cell>();

        //Add children
        room.children.Add(roomRight);
        room.children.Add(roomLeft);

        roomRight.children.AddRange(CheckDivision(roomRight));
        roomLeft.children.AddRange(CheckDivision(roomLeft));

        return rooms;
    }

    List<Room> DivideByY(Room room)
    {
        List<Room> rooms = new List<Room>();

        Room roomUp = new Room();
        Room roomDown = new Room();

        //Value for cut
        float posY = Random.Range(0 + roomSizeY * 0.5f, room.extends.y - roomSizeY * 0.5f);
        //float posY = RandomSeed.GetValue() * (room.extends.y - roomSizeY * 0.5f) + roomSizeY * 0.5f;

        //Extends
        roomDown.extends = new Vector2(room.extends.x, posY);
        roomUp.extends = new Vector2(room.extends.x, room.extends.y - posY);

        //Center
        roomDown.center = new Vector2(room.center.x, room.center.y - room.extends.y * 0.5f + roomDown.extends.y * 0.5f);
        roomUp.center = new Vector2(room.center.x, room.center.y + room.extends.y * 0.5f - roomUp.extends.y * 0.5f);

        //Children
        roomDown.children = new List<Room>();
        roomDown.cell = new List<Cell>();
        roomUp.children = new List<Room>();
        roomUp.cell = new List<Cell>();

        //Add children
        room.children.Add(roomUp);
        room.children.Add(roomDown);

        roomDown.children.AddRange(CheckDivision(roomDown));
        roomUp.children.AddRange(CheckDivision(roomUp));

        return rooms;
    }

    Room CreateDoor(Room room)
    {
        Vector2 doorPosition = new Vector2();
        if (room.children == null) return room;
        if (room.children.Count <= 0) return room;
        if (room.children[0].center.y - room.children[1].center.y <= 0.1f)
        {
            if (room.children[0].center.x - room.children[0].extends.x * 0.5f - room.children[1].extends.x * 0.5f - room.children[1].center.x <= 0.1f)
            {
                doorPosition.Set(room.children[0].center.x - room.children[0].extends.x * 0.5f, room.children[0].center.y);
            }
            else
            {
                doorPosition.Set(room.children[0].center.x + room.children[0].extends.x * 0.5f, room.children[0].center.y);
            }
        }
        else if (room.children[0].center.x - room.children[1].center.x <= 0.1f)
        {
            if (room.children[0].center.y - room.children[0].extends.y * 0.5f - room.children[1].extends.y * 0.5f - room.children[1].center.y <= 0.1f)
            {
                doorPosition.Set(room.children[0].center.x, room.children[0].center.y - room.children[0].extends.y * 0.5f);
            }
            else
            {
                doorPosition.Set(room.children[0].center.x, room.children[0].center.y + room.children[0].extends.y * 0.5f);
            }
        }
        else
        {
            Debug.Log("erreur room not align");
        }

        room.doorPosition = doorPosition;
        for (int i = 0; i < room.children.Count; i++)
        {
            room.children[i] = CreateDoor(room.children[i]);
        }

        return room;
    }

    void OnDrawGizmos()
    {
        DrawRoom(_RootRoom);

    }

    static void DrawRoom(Room room)
    {
        Gizmos.DrawWireCube(room.center, room.extends);

        if (room.children == null) return;

        if (room.children.Count > 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawCube(room.doorPosition, Vector3.one);
        }
        foreach (Room roomChild in room.children)
        {
            Gizmos.color = Color.cyan;
            DrawRoom(roomChild);
        }

    }

   public void  GeneratePlayerAndNPC()
   {
       Vector2Int PlayerPosition = Vector2Int.zero;
       Vector2Int NPCPosition= Vector2Int.zero;

       for (int i = 0; i < 10000; i++)
       {
           Room room = leafRoom[Random.Range(0, leafRoom.Count)];
           PlayerPosition = GetSpawn(room.cell);
           if (PlayerPosition!=Vector2Int.zero)
           {
               break;
           }
       }
       Player.transform.position = new Vector3(PlayerPosition.x + 0.5f, PlayerPosition.y + 0.5f);


        for (int i = 0; i < 10000; i++)
        {
           Room room = leafRoom[Random.Range(0, leafRoom.Count)];
           NPCPosition = GetSpawn(room.cell);
           if (NPCPosition != Vector2Int.zero)
           {
               break;
           }
        }
           Instantiate(NPC, new Vector3(NPCPosition.x + 0.5f, NPCPosition.y + 0.5f), Quaternion.identity);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Bsp))]
public class BspSeedEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Bsp myTarget = (Bsp)target;

        if (GUILayout.Button("Generate"))
        {
            myTarget.Generate();
            myTarget.GenerateCells();
            myTarget.GenerateTile();
        }

        if (GUILayout.Button("Clear"))
        {
            myTarget.Clear();
        }

        DrawDefaultInspector();
    }
}
#endif