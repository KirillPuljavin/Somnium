using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static GameObject currentRoom;
    public static DungeonGenerator dungeon;
    public static Player player;
    public static bool roomCleared = false;

    [SerializeField] private GameObject blobEnemyPrefab;
    [SerializeField] private GameObject frogEnemyPrefab;
    [SerializeField] private GameObject spiderEnemyPrefab;
    [SerializeField] private GameObject crafting1Prefab;
    [SerializeField] private GameObject crafting2Prefab;
    [SerializeField] private GameObject chestPrefab;
    private Transform enemyParent;

    private static List<GameObject> Enemies = new List<GameObject>();

    public void Initialize()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        dungeon = GameObject.Find("Dungeon Generator").GetComponent<DungeonGenerator>();
        currentRoom = dungeon.RoomsInDungeon[player.currRoom];
        enemyParent = currentRoom.transform.GetChild(2).transform;

        NewRoom();

        // IF EMPTY
        if (Enemies.Count <= 0)
        {
            roomCleared = true;
            Debug.Log("ROOM CLEARED");
        }
    }

    public void NewRoom()
    {
        roomCleared = false;
        currentRoom = dungeon.RoomsInDungeon[player.currRoom];
        enemyParent = currentRoom.transform.GetChild(2).transform;

        // Spawn Enemies
        for (int enemyType = 0; enemyType < 3; enemyType++)
        {
            Transform enemyTypeParent = currentRoom.transform.GetChild(1).GetChild(enemyType);
            foreach (Transform enemy in enemyTypeParent)
            {
                Enemies.Add(enemy.gameObject);
                switch (enemyType)
                {
                    case 0:
                        Instantiate(blobEnemyPrefab, enemy.position, Quaternion.identity, enemyParent);
                        break;
                    case 1:
                        Instantiate(frogEnemyPrefab, enemy.position, Quaternion.identity, enemyParent);
                        break;
                    case 2:
                        Instantiate(spiderEnemyPrefab, enemy.position, Quaternion.identity, enemyParent);
                        break;
                }
            }
        }

        // Spawn Chest
        if (player.currRoom == dungeon.currentPreset.positionChest1 || player.currRoom == dungeon.currentPreset.positionChest2)
        {
            Instantiate(chestPrefab, currentRoom.transform.GetChild(3).GetChild(0).transform.position, Quaternion.identity, currentRoom.transform);
        }

        // Spawn Crafting Station
        if (player.currRoom == dungeon.currentPreset.positionUpgrade1)
        {
            Instantiate(crafting1Prefab, currentRoom.transform.GetChild(4).GetChild(0).transform.position, Quaternion.identity, currentRoom.transform);
        }
        else if (player.currRoom == dungeon.currentPreset.positionUpgrade2)
        {
            Instantiate(crafting2Prefab, currentRoom.transform.GetChild(4).GetChild(0).transform.position, Quaternion.identity, currentRoom.transform);
        }
    }

    public void EnemyDied() => StartCoroutine(CheckForEnemies());
    public IEnumerator CheckForEnemies()
    {
        yield return new WaitForSeconds(0.1f);

        Enemies.Clear(); foreach (Transform enemy in enemyParent) Enemies.Add(enemy.gameObject);
        if (Enemies.Count <= 0) roomCleared = true;
    }
}
