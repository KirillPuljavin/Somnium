using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.Windows;

public class RoomManager : MonoBehaviour
{
    public static GameObject currentRoom;
    public static DungeonGenerator dungeon;
    public static Player player;
    public static bool roomCleared = false;
    public static int difficulty;

    [SerializeField] private GameObject blobEnemyPrefab;
    [SerializeField] private GameObject frogEnemyPrefab;
    [SerializeField] private GameObject spiderEnemyPrefab;
    [SerializeField] private GameObject catBossPrefab;
    [SerializeField] private GameObject crafting1Prefab;
    [SerializeField] private GameObject crafting2Prefab;
    [SerializeField] private GameObject chestPrefab;
    [SerializeField] private GameObject componentPrefab;
    [SerializeField] private GameObject healPrefab;
    [SerializeField] private Text difficultyText;
    [SerializeField] private Text roomClearText;
    private Transform enemyParent;

    private static List<GameObject> Enemies = new List<GameObject>();
    private static List<int> clearedRooms = new List<int>();

    private static bool spawnedChest1 = false;
    private static bool spawnedChest2 = false;
    private static bool spawnedCraft1 = false;
    private static bool spawnedCraft2 = false;

    public void Initialize()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        dungeon = GameObject.Find("Dungeon Generator").GetComponent<DungeonGenerator>();
        currentRoom = dungeon.RoomsInDungeon[player.currRoom];
        enemyParent = currentRoom.transform.GetChild(2).transform;
        clearedRooms.Clear();
        doubleKillPrevention = true;
        spawnedChest1 = false;
        spawnedChest2 = false;
        spawnedCraft1 = false;
        spawnedCraft2 = false;
        NewRoom();
    }

    public void NewRoom()
    {
        roomCleared = false;
        currentRoom = dungeon.RoomsInDungeon[player.currRoom];
        enemyParent = currentRoom.transform.GetChild(2).transform;

        Debug.Log("currentRoom = " + player.currRoom);
        if (player.currRoom != 27)
        {
            Enemies.Clear();
            roomClearText.text = "";

            // Difficulty
            if (player.currRoom >= 0 && player.currRoom <= 4) difficulty = 1;
            else if (player.currRoom >= 5 && player.currRoom <= 9) difficulty = 2;
            else if (player.currRoom >= 10 && player.currRoom <= 14) difficulty = 3;
            else if (player.currRoom >= 15 && player.currRoom <= 19) difficulty = 4;
            else if (player.currRoom >= 20 && player.currRoom <= 24) difficulty = 5;
            player.Alert("Difficulty: " + difficulty);
            if (player.currRoom != 28) difficultyText.text = "Difficulty: " + difficulty; else difficultyText.text = "Difficulty: BOSS";

            // Spawn Enemies with Scaling
            if (!clearedRooms.Contains(player.currRoom))
            {
                for (int enemyType = 0; enemyType < 3; enemyType++)
                {
                    Transform enemyTypeParent = currentRoom.transform.GetChild(1).GetChild(enemyType);
                    foreach (Transform enemy in enemyTypeParent)
                    {
                        Enemies.Add(enemy.gameObject);
                        switch (enemyType)
                        {
                            case 0:
                                EnemyBlob spawnedBlob = Instantiate(blobEnemyPrefab, enemy.position, Quaternion.identity, enemyParent).GetComponent<EnemyBlob>();
                                if (difficulty == 2)
                                {
                                    spawnedBlob.enemyHP += 2;
                                }
                                else if (difficulty == 3)
                                {
                                    spawnedBlob.enemyHP += 2;
                                    spawnedBlob.damageHearts += 1;
                                }
                                else if (difficulty == 4)
                                {
                                    spawnedBlob.enemyHP += 3;
                                    spawnedBlob.damageHearts += 2;
                                }
                                else if (difficulty == 5)
                                {
                                    spawnedBlob.enemyHP += 4;
                                    spawnedBlob.damageHearts += 2;
                                }
                                break;
                            case 1:
                                EnemyFrog spawnedFrog = Instantiate(frogEnemyPrefab, enemy.position, Quaternion.identity, enemyParent).GetComponent<EnemyFrog>();
                                if (difficulty == 2)
                                {
                                    spawnedFrog.damageHearts += 1;
                                }
                                else if (difficulty == 3)
                                {
                                    spawnedFrog.enemyHP += 2;
                                    spawnedFrog.damageHearts += 1;
                                }
                                else if (difficulty == 4)
                                {
                                    spawnedFrog.enemyHP += 3;
                                    spawnedFrog.damageHearts += 2;
                                }
                                else if (difficulty == 5)
                                {
                                    spawnedFrog.enemyHP += 4;
                                    spawnedFrog.damageHearts += 2;
                                }
                                break;
                            case 2:
                                EnemySpider spawnedSpider = Instantiate(spiderEnemyPrefab, enemy.position, Quaternion.identity, enemyParent).GetComponent<EnemySpider>();
                                if (difficulty == 2)
                                {
                                    spawnedSpider.enemyHP += 2;
                                }
                                else if (difficulty == 3)
                                {
                                    spawnedSpider.enemyHP += 2;
                                    spawnedSpider.damageHearts += 1;
                                }
                                else if (difficulty == 4)
                                {
                                    spawnedSpider.enemyHP += 3;
                                    spawnedSpider.damageHearts += 2;
                                }
                                else if (difficulty == 5)
                                {
                                    spawnedSpider.enemyHP += 4;
                                    spawnedSpider.damageHearts += 2;
                                }
                                break;
                        }
                    }
                }
            }
            if (Enemies.Count <= 0) { roomCleared = true; roomClearText.text = "Room is Clear!"; }

            // Spawn Chest
            if (player.currRoom == dungeon.currentPreset.positionChest1 && !spawnedChest1) { Instantiate(chestPrefab, currentRoom.transform.GetChild(3).GetChild(0).transform.position, Quaternion.identity, currentRoom.transform); spawnedChest1 = true; }
            else if (player.currRoom == dungeon.currentPreset.positionChest2 && !spawnedChest2) { Instantiate(chestPrefab, currentRoom.transform.GetChild(3).GetChild(0).transform.position, Quaternion.identity, currentRoom.transform); spawnedChest2 = true; }

            // Spawn Crafting Station
            if (player.currRoom == dungeon.currentPreset.positionUpgrade1 && !spawnedCraft1) { Instantiate(crafting1Prefab, currentRoom.transform.GetChild(4).GetChild(0).transform.position, Quaternion.identity, currentRoom.transform); spawnedCraft1 = true; }
            else if (player.currRoom == dungeon.currentPreset.positionUpgrade2 && !spawnedCraft2) { Instantiate(crafting2Prefab, currentRoom.transform.GetChild(4).GetChild(0).transform.position, Quaternion.identity, currentRoom.transform); spawnedCraft2 = true; }

            return;
        }
        bossRoom();
    }


    public void EnemyDied() => StartCoroutine(CheckForEnemies());
    public IEnumerator CheckForEnemies()
    {
        yield return new WaitForSeconds(0.01f);
        if (player.currRoom != 27)
        {
            Enemies.Clear(); foreach (Transform enemy in enemyParent) Enemies.Add(enemy.gameObject);
            if (Enemies.Count <= 0) { roomCleared = true; ClearedRoom(); }
        }
    }

    public bool doubleKillPrevention = true;
    public void ClearedRoom()
    {
        roomClearText.text = "Room is Clear!";
        if (doubleKillPrevention)
        {
            doubleKillPrevention = false;
            clearedRooms.Add(player.currRoom);
            player.Alert("Room cleared.");

            // Spawn Components & Heal
            int componentAmount;
            int healAmount;
            switch (difficulty)
            {
                default:
                case 2:
                    componentAmount = Random.Range(1, 2);
                    healAmount = Random.Range(1, 2);
                    break;
                case 3:
                    componentAmount = Random.Range(1, 3);
                    healAmount = Random.Range(2, 3);
                    break;
                case 4:
                    componentAmount = Random.Range(2, 5);
                    healAmount = Random.Range(2, 4);
                    break;
                case 5:
                    componentAmount = Random.Range(3, 5);
                    healAmount = Random.Range(3, 5);
                    break;
            }

            while (componentAmount >= 1)
            {
                componentAmount--;

                int x = Random.Range(0, 2); int y = Random.Range(0, 2);
                float offsetX;
                float offsetY;
                if (x == 0) offsetX = Random.Range(1f, 3f); else offsetX = Random.Range(-1f, -3f);
                if (y == 0) offsetY = Random.Range(1f, 3f); else offsetY = Random.Range(-1f, -3f);

                Vector3 spawnPosition = new Vector3(player.transform.position.x + offsetX, player.transform.position.y + offsetY, player.transform.position.z);
                Quaternion spawnRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

                Instantiate(componentPrefab, spawnPosition, spawnRotation);
            }

            while (healAmount >= 1)
            {
                healAmount--;

                int x = Random.Range(0, 2); int y = Random.Range(0, 2);
                float offsetX;
                float offsetY;
                if (x == 0) offsetX = Random.Range(1f, 3f); else offsetX = Random.Range(-1f, -3f);
                if (y == 0) offsetY = Random.Range(1f, 3f); else offsetY = Random.Range(-1f, -3f);

                Vector3 spawnPosition = new Vector3(player.transform.position.x + offsetX, player.transform.position.y + offsetY, player.transform.position.z);
                Quaternion spawnRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

                Instantiate(healPrefab, spawnPosition, spawnRotation);
            }
        }
    }

    public void bossRoom()
    {
        GameObject catBoss = Instantiate(catBossPrefab, currentRoom.transform.GetChild(1).GetChild(0).transform.position, Quaternion.identity);
        catBoss.transform.localScale = catBossPrefab.transform.localScale;
        catBoss.transform.SetParent(currentRoom.transform);
    }

}

