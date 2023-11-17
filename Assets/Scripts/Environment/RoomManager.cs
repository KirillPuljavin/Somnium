using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager
{
    public static GameObject currentRoom;
    public static DungeonGenerator dungeon;
    public static Player player;
    public static bool roomCleared = false;

    public static List<GameObject> _enemies = new List<GameObject>();
    private static List<GameObject> Enemies
    {
        get { CheckForEnemies(); return _enemies; }
        set
        {
            _enemies.AddRange(value);
            CheckForEnemies();
        }
    }
    private static void AddEnemy(GameObject enemy)
    {
        _enemies.Add(enemy);
        CheckForEnemies();
    }

    public static void Initialize()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        dungeon = GameObject.Find("Dungeon Generator").GetComponent<DungeonGenerator>();
        currentRoom = dungeon.RoomsInDungeon[player.currRoom];
    }

    public static void RoomUpdate()
    {
        for (int enemyType = 0; enemyType < 3; enemyType++)
        {
            Transform enemyTypeParent = currentRoom.transform.GetChild(1).GetChild(enemyType);
            foreach (GameObject enemy in enemyTypeParent)
            {
                // Spawn enemies
                AddEnemy(enemy);
            }
        }
    }
    public static void CheckForEnemies()
    {
        _enemies.RemoveAll(item => item == null);

        if (Enemies.Count <= 0)
        {

        }
        // Then open Doors
    }
}
