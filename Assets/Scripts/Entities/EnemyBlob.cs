using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlob : MonoBehaviour
{
    public float speed;
    public GameObject PlayerObj;
    Player player;
    void Start()
    {
        PlayerObj = GameObject.FindWithTag("Player");
        player.TakeDamage(1);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, PlayerObj.transform.position, speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Slime Hit Player");

        }
    }

}
