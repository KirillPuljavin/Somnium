using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    [SerializeField] private GameObject componentPrefab;
    private Transform enemyParent;

    private static List<GameObject> Enemies = new List<GameObject>();

    public void Initialize()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        dungeon = GameObject.Find("Dungeon Generator").GetComponent<DungeonGenerator>();
        currentRoom = dungeon.RoomsInDungeon[player.currRoom];
        enemyParent = currentRoom.transform.GetChild(2).transform;

        NewRoom();
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
        if (Enemies.Count <= 0) roomCleared = true;

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
        yield return new WaitForSeconds(0.01f);

        Enemies.Clear(); foreach (Transform enemy in enemyParent) Enemies.Add(enemy.gameObject);
        if (Enemies.Count <= 0) { roomCleared = true; SpawnComponents(); }
    }

    public void SpawnComponents()
    {
        // Calculate quantity
        int quantity = 1;
        float minOffset = -3f;
        float maxOffset = 3f;

        while (quantity >= 1)
        {
            quantity--;

            float offsetX = Random.Range(minOffset, maxOffset);
            float offsetY = Random.Range(minOffset, maxOffset);
            Vector3 spawnPosition = new Vector3(player.transform.position.x + offsetX, player.transform.position.y + offsetY, player.transform.position.z);

            if (true)
            {
                Instantiate(componentPrefab, spawnPosition, Quaternion.identity);
                Debug.Log("Component Spawned");
            }
        }
    }
    /* private bool IsWithinPlayableBounds(Vector3 position)
    {
        Tilemap tilemap = currentRoom.GetComponent<Tilemap>();
        if (tilemap != null)
        {
            BoundsInt bounds = tilemap.cellBounds;
            TileBase[] allTiles = tilemap.GetTilesBlock(bounds);

            Vector3Int cellPosition = tilemap.WorldToCell(position);

            if (bounds.Contains(cellPosition) && allTiles[cellPosition.x + cellPosition.y * bounds.size.x] != null) return true;
        }
        return false;
    } */
}
