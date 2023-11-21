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

    float maxDistance = 4f;
    float speed = 0.8f;
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= 10)
        {
            float t = Mathf.Clamp01(distance / maxDistance) * speed * Time.deltaTime;
            transform.position = Vector2.Lerp(transform.position, player.transform.position, t);
        }
    }
}
