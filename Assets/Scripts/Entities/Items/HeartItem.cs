using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartItem : MonoBehaviour
{
    private HeartUpdate heartsHUD;
    private Player player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        heartsHUD = GameObject.FindWithTag("HeartsUI").GetComponent<HeartUpdate>();
    }

    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            player.Hearts += 2;
            Destroy(gameObject);
            if (player.Hearts > player.MaxHearts) player.Hearts = player.MaxHearts;
            heartsHUD.UpdateHearts();
        }
    }
}
