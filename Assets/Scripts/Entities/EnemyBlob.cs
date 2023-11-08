using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyBlob : MonoBehaviour
{
    public GameObject PlayerObj;
    Player player;
    float attackCooldown = 0;
    float damageCooldown = 0;
    public float speed;
    public float enemyHP = 3;

    void Start()
    {
        PlayerObj = GameObject.FindWithTag("Player");
        player = PlayerObj.GetComponent<Player>();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector2(PlayerObj.transform.position.x, PlayerObj.transform.position.y + 0.5f), speed * Time.deltaTime);

        // Cooldown
        if (attackCooldown < 1) attackCooldown += Time.deltaTime;
        if (damageCooldown > 0) damageCooldown -= Time.deltaTime;

        if (enemyHP <= 0)
        {
            Destroy(gameObject);
        }
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
        if (collider.gameObject.tag == "attackHitbox")
        {
            TakeDamage();
        }

        if (collider.gameObject.tag == "Player" && attackCooldown >= 1)
        {
            if (!player.isDashing)
            {
                player.TakeDamage(0.5f);
                attackCooldown = 0;
            }
        }
        if (collider.gameObject.tag == "dashHitbox" && damageCooldown <= 0 && player.isDashing)
        {
            TakeDamage();
            damageCooldown = 0.5f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            speed = 1.3f;
        }

    }
    public void TakeDamage()
    {
        enemyHP -= player.damage;
    }

}
