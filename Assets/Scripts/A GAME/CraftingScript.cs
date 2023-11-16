using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CraftingScript : MonoBehaviour
{
    public Animator animator;

    private bool clickable = false;
    private Player player;
    private int T2Amount = 5;
    private int T3Amount = 5;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        if (clickable && Input.GetKeyDown(KeyCode.E))
        {
            if (player.ComponentsTier2 >= T2Amount)
            {
                player.ComponentsTier2 -= T2Amount;
                player.WeaponEvo++;
                Debug.Log("Can craft with tier 2. You have " + player.ComponentsTier2 + " Left!");
            }
            else if (player.ComponentsTier3 >= T3Amount)
            {
                player.ComponentsTier3 -= T3Amount;
                player.WeaponEvo++;
                Debug.Log("Can craft with tier 3. You have " + player.ComponentsTier3 + " Left!");
            }
            else Debug.Log("Can't Craft! You only have " + player.ComponentsTier2 + " Tier 2 Components and " + player.ComponentsTier3 + " Tier 3 Components!");
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "dashHitbox")
        {
            clickable = true;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "dashHitbox")
        {
            clickable = false;
        }
    }
}
