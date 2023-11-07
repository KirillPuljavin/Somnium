using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonGenerator : MonoBehaviour
{
    // Objects
    private GameObject[,] Rooms;
    private DungeonPreset currentPreset;
    public GameObject passagePrefab;
    public GameObject roomPrefab;

    void Start()
    {
        // Startup
        Rooms = new GameObject[5, 6];
        for (int x = 0; x < Rooms.GetLength(0); x++)
        {
            for (int y = 0; y < Rooms.GetLength(1); y++)
            {
                GameObject room = transform.GetChild(x * Rooms.GetLength(1) + y).gameObject;
                Rooms[x, y] = room;
            }
        }

        currentPreset = new Dungeon1();
        // VisualizeDungeon();

        // Choose rooms
        foreach (var room in currentPreset.roomsAvailable)
        {
            GameObject newRoom = Instantiate(roomPrefab, new Vector3(CalculateRoomPosition(room).x, CalculateRoomPosition(room).y, 0f), Quaternion.identity);
        }
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
    Vector2 CalculateRoomPosition(int roomIndex)
    {
        int rowIndex = roomIndex / Rooms.GetLength(1);
        int columnIndex = roomIndex % Rooms.GetLength(1);

        Vector3 position = Rooms[rowIndex, columnIndex].transform.position;
        return new Vector2(position.x, position.y);
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

}

[System.Serializable]
public class Dungeon1 : DungeonPreset
{
    public Dungeon1()
    {
        positionBossTransitionRoom = 26;
        positionUpgrade1 = 13;
        positionUpgrade2 = 19;

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
        positionUpgrade1 = 0;
        positionUpgrade2 = 0;

        passages.Add(new Passage(0, 0));
        passages.Add(new Passage(0, 0));
        passages.Add(new Passage(0, 0));
        passages.Add(new Passage(0, 0));
        passages.Add(new Passage(0, 0));
        passages.Add(new Passage(0, 0));
        passages.Add(new Passage(0, 0));
        passages.Add(new Passage(0, 0));
        passages.Add(new Passage(0, 0));
        passages.Add(new Passage(0, 0));
        passages.Add(new Passage(0, 0));
        passages.Add(new Passage(0, 0));
        passages.Add(new Passage(0, 0));
        passages.Add(new Passage(0, 0));
    }
}