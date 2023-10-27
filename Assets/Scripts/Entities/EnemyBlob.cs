using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBlob : MonoBehaviour
{
    public float speed;
    public GameObject player;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Slime Hit Player");
        }
    }

}
