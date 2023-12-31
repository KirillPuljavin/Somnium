using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sceneManagerScript : MonoBehaviour
{
    public string scene1;
    public string scene2;
    public string scene3;
    public string scene4;

    [SerializeField] private FloatSO PlayerSO;
    private Player PlayerPrefab;

    void Start()
    {
        PlayerPrefab = Resources.Load<Player>("Prefabs/Entities/Characters/Player");
        if (SceneManager.GetActiveScene().name == scene1)
        {
            setPlayerStats();
        }
    }

    private void setPlayerStats()
    {
        PlayerSO.Hearts = PlayerPrefab.Hearts;
        PlayerSO.MaxHearts = PlayerPrefab.MaxHearts;
        PlayerSO.Vision = PlayerPrefab.Vision;

        PlayerSO.Damage = PlayerPrefab.damage;
        PlayerSO.DashDamage = PlayerPrefab.dashDamage;
        PlayerSO.AttackRange = PlayerPrefab.attackRange;
        PlayerSO.WeaponEvo = PlayerPrefab.WeaponEvo;

        PlayerSO.Card1 = false;
        PlayerSO.Card2 = false;
        PlayerSO.Card3 = false;
        PlayerSO.Card4 = false;
    }
    public void SwitchToDungeon1()
    {
        SceneManager.LoadScene(scene2);
    }
}
