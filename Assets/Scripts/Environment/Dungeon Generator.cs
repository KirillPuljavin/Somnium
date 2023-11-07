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

    // Variables


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

        // Choose passage preset
        currentPreset = new Dungeon1();
        VisualizeDungeon();

        // Choose rooms
    }

    void VisualizeDungeon()
    {
        foreach (var connection in currentPreset.connections)
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
public class Connection
{
    public int roomIndex1;
    public int roomIndex2;

    public Connection(int roomIndex1, int roomIndex2)
    {
        this.roomIndex1 = roomIndex1;
        this.roomIndex2 = roomIndex2;
    }
}
[System.Serializable]
public abstract class DungeonPreset
{
    public List<Connection> connections = new List<Connection>();
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

        connections.Add(new Connection(2, 3));
        connections.Add(new Connection(2, 7));
        connections.Add(new Connection(7, 6));
        connections.Add(new Connection(6, 5));
        connections.Add(new Connection(6, 11));
        connections.Add(new Connection(7, 12));
        connections.Add(new Connection(11, 12));
        connections.Add(new Connection(12, 13));
        connections.Add(new Connection(11, 16));
        connections.Add(new Connection(16, 17));
        connections.Add(new Connection(17, 22));
        connections.Add(new Connection(22, 21));
        connections.Add(new Connection(21, 20));
        connections.Add(new Connection(22, 23));
        connections.Add(new Connection(23, 24));
        connections.Add(new Connection(24, 19));
        connections.Add(new Connection(21, 26));
        connections.Add(new Connection(26, 27));
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

        connections.Add(new Connection(0, 0));
        connections.Add(new Connection(0, 0));
        connections.Add(new Connection(0, 0));
        connections.Add(new Connection(0, 0));
        connections.Add(new Connection(0, 0));
        connections.Add(new Connection(0, 0));
        connections.Add(new Connection(0, 0));
        connections.Add(new Connection(0, 0));
        connections.Add(new Connection(0, 0));
        connections.Add(new Connection(0, 0));
        connections.Add(new Connection(0, 0));
        connections.Add(new Connection(0, 0));
        connections.Add(new Connection(0, 0));
        connections.Add(new Connection(0, 0));
    }
}