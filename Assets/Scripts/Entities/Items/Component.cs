using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Component : MonoBehaviour
{
    private Player player;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.Components++;
            Destroy(gameObject);
        }
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, 0.5f * Time.deltaTime);
    }
}
