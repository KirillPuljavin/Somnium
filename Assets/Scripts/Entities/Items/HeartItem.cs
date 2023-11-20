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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.Hearts += 2;
            if (player.Hearts > player.MaxHearts) player.Hearts = player.MaxHearts;
            heartsHUD.UpdateHearts();
            Destroy(gameObject);
        }
    }

    void Update()
    {
        Vector3.MoveTowards(transform.position, player.transform.position, 1f * Time.deltaTime);
    }
}
