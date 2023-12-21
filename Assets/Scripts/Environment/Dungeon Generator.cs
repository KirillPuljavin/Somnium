using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class DungeonGenerator : MonoBehaviour
{
    // Objects
    public GameObject passagePrefab;
    public GameObject doorPrefab;
    public Transform doorParent;
    public GameObject[] RoomsInDungeon;
    public DungeonPreset currentPreset;

    private GameObject[,] RoomGrid;
    private GameObject[] roomPrefabsForest;
    private GameObject[] roomPrefabsSnow;
    private GameObject[] roomPrefabsBoss;

    [SerializeField] private Sprite doorSpriteT;
    [SerializeField] private Sprite doorSpriteB;
    [SerializeField] private Sprite doorSpriteL;
    [SerializeField] private Sprite doorSpriteR;
    [SerializeField] private Sprite snowdoorSpriteT;
    [SerializeField] private Sprite snowdoorSpriteB;
    [SerializeField] private Sprite snowdoorSpriteL;
    [SerializeField] private Sprite snowdoorSpriteR;
    [SerializeField] private Sprite bossDoorSprite;


    void Start()
    {
        // Startup
        RoomsInDungeon = new GameObject[30];
        RoomGrid = new GameObject[5, 6];
        for (int x = 0; x < RoomGrid.GetLength(0); x++)
        {
            for (int y = 0; y < RoomGrid.GetLength(1); y++)
            {
                GameObject room = transform.GetChild(x * RoomGrid.GetLength(1) + y).gameObject;
                RoomGrid[x, y] = room;
            }
        }
        int r = Random.Range(0, 2);
        if (r == 0) currentPreset = new Dungeon1();
        else currentPreset = new Dungeon2();

        // Setup Rooms
        roomPrefabsForest = Resources.LoadAll<GameObject>("Prefabs/Map/Rooms/Forest");
        roomPrefabsSnow = Resources.LoadAll<GameObject>("Prefabs/Map/Rooms/Snow");
        roomPrefabsBoss = Resources.LoadAll<GameObject>("Prefabs/Map/Rooms/Boss");
        foreach (var roomIndex in currentPreset.roomsAvailable)
        {
            if (roomIndex == 27)
            {
                int roomX = Random.Range(0, roomPrefabsBoss.Length);
                GameObject newRoom = Instantiate(roomPrefabsBoss[roomX], new Vector3(CalculateRoomPosition(roomIndex).x, CalculateRoomPosition(roomIndex).y, 0f), Quaternion.identity, transform.GetChild(roomIndex).transform);
                RoomsInDungeon[roomIndex] = newRoom;
            }
            else if (roomIndex < 15)
            {
                int roomX = Random.Range(0, roomPrefabsForest.Length);
                GameObject newRoom = Instantiate(roomPrefabsForest[roomX], new Vector3(CalculateRoomPosition(roomIndex).x, CalculateRoomPosition(roomIndex).y, 0f), Quaternion.identity, transform.GetChild(roomIndex).transform);
                RoomsInDungeon[roomIndex] = newRoom;
            }
            else
            {
                int roomX = Random.Range(0, roomPrefabsSnow.Length);
                GameObject newRoom = Instantiate(roomPrefabsSnow[roomX], new Vector3(CalculateRoomPosition(roomIndex).x, CalculateRoomPosition(roomIndex).y, 0f), Quaternion.identity, transform.GetChild(roomIndex).transform);
                RoomsInDungeon[roomIndex] = newRoom;
            }
        }

        // Instantiate Doors
        foreach (var connection in currentPreset.passages)
        {
            bool yes = false;
            int roomIndex1 = connection.roomIndex1;
            int roomIndex2 = connection.roomIndex2;
            if (roomIndex1 == 27 || roomIndex2 == 27) yes = true;

            DoorPlacement doorPlacement1 = GetDoorPlacement(roomIndex1, roomIndex2);
            Vector2 doorPosition1 = CalculateDoorPosition(doorPlacement1, roomIndex1, yes);
            DoorPlacement doorPlacement2 = GetDoorPlacement(roomIndex2, roomIndex1);
            Vector2 doorPosition2 = CalculateDoorPosition(doorPlacement2, roomIndex2, yes);

            DoorMechanics door1 = Instantiate(doorPrefab, doorPosition1, Quaternion.identity, doorParent).GetComponent<DoorMechanics>();
            door1.targetDoorPos = doorPosition2;
            door1.targetRoomIndex = roomIndex2;

            if (connection.roomIndex1 == 27 || connection.roomIndex2 == 27) door1.gameObject.GetComponent<SpriteRenderer>().sprite = bossDoorSprite;
            else switch (doorPlacement1.position)
                {
                    case InRoomPos.Top:
                        door1.gameObject.GetComponent<SpriteRenderer>().sprite = doorSpriteT;
                        break;
                    case InRoomPos.Bottom:
                        door1.gameObject.GetComponent<SpriteRenderer>().sprite = doorSpriteB;
                        break;
                    case InRoomPos.Left:
                        door1.gameObject.GetComponent<SpriteRenderer>().sprite = doorSpriteL;
                        break;
                    case InRoomPos.Right:
                        door1.gameObject.GetComponent<SpriteRenderer>().sprite = doorSpriteR;
                        break;
                }

            DoorMechanics door2 = Instantiate(doorPrefab, doorPosition2, Quaternion.identity, doorParent).GetComponent<DoorMechanics>();
            door2.targetDoorPos = doorPosition1;
            door2.targetRoomIndex = roomIndex1;

            if (connection.roomIndex1 == 27 || connection.roomIndex2 == 27) door2.gameObject.GetComponent<SpriteRenderer>().sprite = bossDoorSprite;
            else switch (doorPlacement2.position)
                {
                    case InRoomPos.Top:
                        door2.gameObject.GetComponent<SpriteRenderer>().sprite = doorSpriteT;
                        break;
                    case InRoomPos.Bottom:
                        door2.gameObject.GetComponent<SpriteRenderer>().sprite = doorSpriteB;
                        break;
                    case InRoomPos.Left:
                        door2.gameObject.GetComponent<SpriteRenderer>().sprite = doorSpriteL;
                        break;
                    case InRoomPos.Right:
                        door2.gameObject.GetComponent<SpriteRenderer>().sprite = doorSpriteR;
                        break;
                }
        }
        GetComponent<RoomManager>().Initialize();
    }

    DoorPlacement GetDoorPlacement(int roomIndex1, int roomIndex2)
    {
        int row1 = roomIndex1 / RoomGrid.GetLength(0);
        int col1 = roomIndex1 % RoomGrid.GetLength(0);
        int row2 = roomIndex2 / RoomGrid.GetLength(0);
        int col2 = roomIndex2 % RoomGrid.GetLength(0);

        if (row1 == row2) // Horizontal passage
        {
            return new DoorPlacement
            {
                position = col1 < col2 ? InRoomPos.Right : InRoomPos.Left,
                roomIndex = col1 < col2 ? roomIndex1 : roomIndex2
            };
        }
        else if (col1 == col2) // Vertical passage
        {
            return new DoorPlacement
            {
                position = row1 < row2 ? InRoomPos.Top : InRoomPos.Bottom,
                roomIndex = row1 < row2 ? roomIndex1 : roomIndex2
            };
        }
        else return new DoorPlacement();
    }
    Vector2 CalculateDoorPosition(DoorPlacement doorPlacement, int roomIndex, bool bossDoor)
    {
        Vector2 roomPosition = CalculateRoomPosition(roomIndex);
        if (bossDoor)
        {
            return RoomsInDungeon[roomIndex].transform.GetChild(0).GetChild(0).transform.position;
        }
        else switch (doorPlacement.position)
            {
                case InRoomPos.Top:
                    return RoomsInDungeon[roomIndex].transform.GetChild(0).GetChild(0).transform.position;
                case InRoomPos.Right:
                    return RoomsInDungeon[roomIndex].transform.GetChild(0).GetChild(1).transform.position;
                case InRoomPos.Bottom:
                    return RoomsInDungeon[roomIndex].transform.GetChild(0).GetChild(2).transform.position;
                case InRoomPos.Left:
                    return RoomsInDungeon[roomIndex].transform.GetChild(0).GetChild(3).transform.position;
                default:
                    return roomPosition;
            }
    }

    Vector2 CalculateRoomPosition(int roomIndex)
    {
        int rowIndex = roomIndex / RoomGrid.GetLength(1);
        int columnIndex = roomIndex % RoomGrid.GetLength(1);

        Vector3 position = RoomGrid[rowIndex, columnIndex].transform.position;
        return new Vector2(position.x, position.y);
    }

    void VisualizeDungeon()
    {
        foreach (var connection in currentPreset.passages)
        {
            int roomIndex1 = connection.roomIndex1;
            int roomIndex2 = connection.roomIndex2;
            Vector2 position1 = CalculateRoomPosition(roomIndex1);
            Vector2 position2 = CalculateRoomPosition(roomIndex2);
            Vector2 midpoint = (position1 + position2) / 2f;

            float angle = Mathf.Atan2(position2.y - position1.y, position2.x - position1.x) * Mathf.Rad2Deg;
            float distance = Vector2.Distance(position1, position2);

            GameObject passage = Instantiate(passagePrefab, midpoint, Quaternion.Euler(0f, 0f, angle));
            passage.transform.localScale = new Vector3(distance, 1f, 1f);
        }
    }
}

[System.Serializable]
public class Passage
{
    public int roomIndex1;
    public int roomIndex2;

    public Passage(int roomIndex1, int roomIndex2)
    {
        this.roomIndex1 = roomIndex1;
        this.roomIndex2 = roomIndex2;
    }
}
[System.Serializable]
public abstract class DungeonPreset
{
    public List<Passage> passages = new List<Passage>();
    public HashSet<int> roomsAvailable = new HashSet<int>();
    public int positionBossRoom = 27;
    public int positionBossTransitionRoom;
    public int positionUpgrade1;
    public int positionUpgrade2;
    public int positionChest1;
    public int positionChest2;
}

[System.Serializable]
public class Dungeon1 : DungeonPreset
{
    public Dungeon1()
    {
        positionBossTransitionRoom = 26;
        positionUpgrade1 = 13;
        positionUpgrade2 = 19;
        positionChest1 = 5;
        positionChest2 = 20;

        passages.Add(new Passage(2, 3));
        passages.Add(new Passage(2, 7));
        passages.Add(new Passage(7, 6));
        passages.Add(new Passage(6, 5));
        passages.Add(new Passage(6, 11));
        passages.Add(new Passage(7, 12));
        passages.Add(new Passage(11, 12));
        passages.Add(new Passage(12, 13));
        passages.Add(new Passage(11, 16));
        passages.Add(new Passage(16, 17));
        passages.Add(new Passage(17, 22));
        passages.Add(new Passage(22, 21));
        passages.Add(new Passage(21, 20));
        passages.Add(new Passage(22, 23));
        passages.Add(new Passage(23, 24));
        passages.Add(new Passage(24, 19));
        passages.Add(new Passage(21, 26));
        passages.Add(new Passage(26, 27));

        foreach (var connection in passages)
        {
            roomsAvailable.Add(connection.roomIndex1);
            roomsAvailable.Add(connection.roomIndex2);
        }
    }
}
[System.Serializable]
public class Dungeon2 : DungeonPreset
{
    public Dungeon2()
    {
        positionBossTransitionRoom = 28;
        positionUpgrade1 = 5;
        positionUpgrade2 = 15;
        positionChest1 = 8;
        positionChest2 = 23;

        passages.Add(new Passage(2, 1));
        passages.Add(new Passage(2, 7));
        passages.Add(new Passage(7, 6));
        passages.Add(new Passage(6, 5));
        passages.Add(new Passage(7, 8));
        passages.Add(new Passage(6, 11));
        passages.Add(new Passage(8, 13));
        passages.Add(new Passage(11, 16));
        passages.Add(new Passage(13, 18));
        passages.Add(new Passage(16, 15));
        passages.Add(new Passage(16, 17));
        passages.Add(new Passage(17, 18));
        passages.Add(new Passage(18, 19));
        passages.Add(new Passage(17, 22));
        passages.Add(new Passage(22, 23));
        passages.Add(new Passage(23, 28));
        passages.Add(new Passage(28, 27));

        foreach (var connection in passages)
        {
            roomsAvailable.Add(connection.roomIndex1);
            roomsAvailable.Add(connection.roomIndex2);
        }
    }
}