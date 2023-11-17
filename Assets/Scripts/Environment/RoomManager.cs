using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager
{
    public static GameObject currentRoom;
    public static List<GameObject> enemies = new List<GameObject>();
    public static bool roomCleared = false;

    public static Player player;

    public static void Initialize()
    {
        for (int enemyType = 0; enemyType < 3; enemyType++)
        {
            Transform enemyTypeParent = currentRoom.transform.GetChild(1).GetChild(enemyType);
            foreach (GameObject enemy in enemyTypeParent)
            {
                // Spawn enemies
            }
        }

        // Then open Doors
    }
}
