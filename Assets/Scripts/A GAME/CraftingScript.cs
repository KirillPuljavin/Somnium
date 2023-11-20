using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftingScript : MonoBehaviour
{
    public Animator animator;
    public int craftingLvl;

    private Player player;
    private bool clickable = false;
    public readonly static int[] UpgradeCosts = new int[] { 3, 3, 5, 7, 15 };

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (RoomManager.roomCleared)
            if (clickable && Input.GetKeyDown(KeyCode.E) && player.WeaponEvo < 5)
            {
                int amount;
                amount = UpgradeCosts[player.WeaponEvo];
                if (craftingLvl == 1)
                {
                    if (player.Components >= amount && player.WeaponEvo < 3)
                    {
                        player.UpgradeEvo();
                        player.Components -= amount;
                    }
                    else if (player.WeaponEvo >= 3) player.Alert("You need lvl 2 station now.");
                    else player.Alert("Can't Craft! You need " + amount + " Components");
                }
                else
                {
                    if (player.Components >= amount)
                    {
                        player.UpgradeEvo();
                        player.Components -= amount;
                    }
                    else player.Alert("Can't Craft! You need " + amount + " Components");
                }
            }
            else if (clickable && Input.GetKeyDown(KeyCode.E) && player.WeaponEvo >= 5) player.Alert("You have maxed all upgrades.");
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "dashHitbox") clickable = true;
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "dashHitbox") clickable = false;
    }
}
