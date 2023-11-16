using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{

    private GameObject PlayerObj;
    Player player;

    public Text text;

    private int playerComp;

    void Start()
    {
        PlayerObj = GameObject.FindWithTag("Player");
        player = PlayerObj.GetComponent<Player>();
        text = GetComponent<Text>();

        UpdateUI();
    }

    void Update()
    {
        playerComp = player.Components;
    }

    public void UpdateUI()
    {
        text.text = "Components: " + playerComp.ToString() + " Weapon Tier: " + player.WeaponEvo;
    }
}
