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
    float maxDistance = 5f;
    float speed = 1;
    void Update()
    {

        float distance = Vector2.Distance(transform.position, player.transform.position) + 1;
        float t = Mathf.Clamp01(distance / maxDistance) * speed * Time.deltaTime;
        transform.position = Vector2.Lerp(transform.position, player.transform.position, t);
    }
}
