using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftingScript : MonoBehaviour
{
    public Animator animator;

    private Player player;
    private int craftingLvl = 1;
    private bool clickable = false;
    private int[] UpgradeCosts;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        UpgradeCosts = new int[] { 3, 3, 5, 7, 10 };
    }

    void Update()
    {
        if (clickable && Input.GetKeyDown(KeyCode.E))
        {
            int amount = UpgradeCosts[player.WeaponEvo + 1];
            if (craftingLvl == 1)
            {
                if (player.Components >= amount && player.WeaponEvo < 3)
                {
                    player.UpgradeEvo();
                    player.Components -= amount;
                    Debug.Log("You have " + player.Components + " Left!");
                }
                else Debug.Log("Can't Craft! You need " + amount + " Components");
            }
            else
            {
                if (player.Components >= amount)
                {
                    player.UpgradeEvo();
                    player.Components -= amount;
                    Debug.Log("You have " + player.Components + " Left!");
                }
                else Debug.Log("Can't Craft! You need " + amount + " Components");
            }
        }
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
