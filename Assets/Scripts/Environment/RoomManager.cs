using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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

    private List<GameObject> Enemies = new List<GameObject>();

    public void Initialize()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        dungeon = GameObject.Find("Dungeon Generator").GetComponent<DungeonGenerator>();
        currentRoom = dungeon.RoomsInDungeon[player.currRoom];

        NewRoom();
        EnemyDied();
    }

    public void NewRoom()
    {
        currentRoom = dungeon.RoomsInDungeon[player.currRoom];

        // Spawn Enemies
        /* for (int enemyType = 0; enemyType < 3; enemyType++)
        {
            Transform enemyTypeParent = currentRoom.transform.GetChild(1).GetChild(enemyType);
            foreach (Transform enemy in enemyTypeParent)
            {
                Enemies.Add(enemy.gameObject);
                switch (enemyType)
                {
                    case 0:
                        Instantiate(blobEnemyPrefab, enemy.position, Quaternion.identity, currentRoom.transform);
                        break;
                    case 1:
                        Instantiate(frogEnemyPrefab, enemy.position, Quaternion.identity, currentRoom.transform);
                        break;
                    case 2:
                        Instantiate(spiderEnemyPrefab, enemy.position, Quaternion.identity, currentRoom.transform);
                        break;
                }
            }
        } */

        // Spawn Crafting Station
        if (player.currRoom == dungeon.currentPreset.positionUpgrade1)
        {
            Instantiate(crafting1Prefab, currentRoom.transform.GetChild(3).GetChild(0).transform.position, Quaternion.identity, currentRoom.transform);
            Debug.Log("Instantiated crafting 1 in room: " + dungeon.currentPreset.positionUpgrade1);
        }
        else if (player.currRoom == dungeon.currentPreset.positionUpgrade2)
        {
            Instantiate(crafting2Prefab, currentRoom.transform.GetChild(3).GetChild(0).transform.position, Quaternion.identity, currentRoom.transform);
            Debug.Log("Instantiated crafting 2 in room: " + dungeon.currentPreset.positionUpgrade2);
        }
    }

    public IEnumerator CheckForEnemies()
    {
        yield return new WaitForSeconds(0.3f);

        Enemies.RemoveAll(item => item == null);
        Debug.Log("Enemy count: " + Enemies.Count);

        if (Enemies.Count <= 0)
        {
            roomCleared = true;
        }
    }
    public void EnemyDied() => StartCoroutine(CheckForEnemies());
}
