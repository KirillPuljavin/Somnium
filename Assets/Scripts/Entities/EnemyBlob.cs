using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyBlob : MonoBehaviour
{
    public float speed;
    public GameObject PlayerObj;
    Player player;
    bool notMoving = false;
    float followTimer = 1;

    void Start()
    {
        PlayerObj = GameObject.FindWithTag("Player");
        player = PlayerObj.GetComponent<Player>();
    }

    void Update()
    {
        if (!notMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, PlayerObj.transform.position, speed * Time.deltaTime);
        }
        else if (notMoving)
        {
            followTimer -= Time.deltaTime;
        }
        if (followTimer <= 0)
        {
            notMoving = false;
            Debug.Log(followTimer);
            followTimer = 1;
        }
        

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.TakeDamage(0.5f);
            notMoving = true;
            
        }
    }
}
