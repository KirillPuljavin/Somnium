using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompT3 : MonoBehaviour
{

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.ComponentsTier3++;
            Destroy(gameObject);
        }
    }
}