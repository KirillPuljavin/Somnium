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
    private int T2Amt = 5;
    private int T3Amt = 5;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (clickable && Input.GetKeyDown(KeyCode.E))
        {
            clickable = false;
            if (player.ComponentsTier2 >= T2Amt)
            {
                player.ComponentsTier2 -= T2Amt;
                player.WeaponEvo++;
                Debug.Log("Can craft with tier 2. You have " + player.ComponentsTier2 + " Left!");
                clickable = true;
            }
            else if (player.ComponentsTier3 >= T3Amt)
            {
                player.ComponentsTier3 -= T3Amt;
                player.WeaponEvo++;
                Debug.Log("Can craft with tier 3. You have " + player.ComponentsTier3 + " Left!");
                clickable = true;
            }
            else
            {
                Debug.Log("Can't Craft! You only have " + player.ComponentsTier2 + " Tier 2 Components and " + player.ComponentsTier3 + " Tier 3 Components!");
                clickable = true;
            }
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
