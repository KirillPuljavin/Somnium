using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogFly : MonoBehaviour
{
    Player Player;
    public int speed;

    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector2(Player.gameObject.transform.position.x, Player.gameObject.transform.position.y + 0.5f), speed * Time.deltaTime);
    }
}
