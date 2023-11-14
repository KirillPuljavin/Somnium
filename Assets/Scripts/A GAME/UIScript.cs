using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{

    private GameObject PlayerObj;
    Player player;

    public Text text;

    private int playerCompT2;
    private int playerCompT3;

    // Start is called before the first frame update
    void Start()
    {
        PlayerObj = GameObject.FindWithTag("Player");
        player = PlayerObj.GetComponent<Player>();
        text = GetComponent<Text>();

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        playerCompT2 = player.ComponentsTier2;
        playerCompT3 = player.ComponentsTier3;
    }

    public void UpdateUI()
    {
        text.text = "Components Tier 2: " + playerCompT2.ToString() + " Components Tier 3: " + playerCompT3.ToString() + " Weapon Tier: " + player.WeaponEvo;
    }
}
