using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyBlob : MonoBehaviour
{
    public GameObject PlayerObj;
    Player player;
    float cooldown = 0;
    public float speed;

    void Start()
    {
        PlayerObj = GameObject.FindWithTag("Player");
        player = PlayerObj.GetComponent<Player>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector2(PlayerObj.transform.position.x, PlayerObj.transform.position.y - 0f), speed * Time.deltaTime);

        // Cooldown
        if (cooldown < 1) cooldown += Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            speed = 0.005f;
        }
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        Debug.Log(cooldown);

        if (collider.gameObject.tag == "Player" && cooldown >= 1)
        {
            player.TakeDamage(0.5f);
            cooldown = 0;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            speed = 1.3f;
        }
    }
}
