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

    private List<GameObject> _enemies = new List<GameObject>();
    private List<GameObject> Enemies
    {
        get { return _enemies; }
        set
        {
            _enemies.AddRange(value);
            EnemyDied();
        }
    }
    private void AddEnemy(GameObject enemy)
    {
        _enemies.Add(enemy);
        EnemyDied();
    }

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
        for (int enemyType = 0; enemyType < 3; enemyType++)
        {
            Transform enemyTypeParent = currentRoom.transform.GetChild(1).GetChild(enemyType);
            foreach (Transform enemy in enemyTypeParent)
            {
                AddEnemy(enemy.gameObject);
                switch (enemyType)
                {
                    case 0:
                        Instantiate(blobEnemyPrefab, enemy.position, Quaternion.identity, enemy);
                        break;
                    case 1:
                        Instantiate(frogEnemyPrefab, enemy.position, Quaternion.identity, enemy);
                        break;
                    case 2:
                        Instantiate(spiderEnemyPrefab, enemy.position, Quaternion.identity, enemy);
                        break;
                }
            }
        }
    }
    public IEnumerator CheckForEnemies()
    {
        yield return new WaitForSeconds(0.3f);

        _enemies.RemoveAll(item => item == null);
        Debug.Log("ENEMY LIST UPDATED");
        Debug.Log("Enemy count: " + Enemies.Count);

        if (Enemies.Count <= 0)
        {
            roomCleared = true;
        }
    }
    public void EnemyDied() => StartCoroutine(CheckForEnemies());
}
