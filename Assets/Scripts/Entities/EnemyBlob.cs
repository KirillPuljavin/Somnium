using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyBlob : MonoBehaviour
{
    Player Player;
    public float speed;
    public float enemyHP = 8;

    private float oldSpeed;
    float attackCooldown = 0;
    float dashCooldown = 0;
    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
        oldSpeed = speed;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector2(Player.gameObject.transform.position.x, Player.gameObject.transform.position.y + 0.5f), speed * Time.deltaTime);

        // Cooldown
        if (attackCooldown < 1) attackCooldown += Time.deltaTime;
        if (dashCooldown > 0) dashCooldown -= Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") speed = 0.005f;
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        // Attack Player
        if (collider.gameObject.tag == "Player" && attackCooldown >= 1)
        {
            if (!Player.isDashing)
            {
                Player.TakeDamage(1);
                attackCooldown = 0;
            }
        }
        // Dash Take Damage
        if (collider.gameObject.tag == "dashHitbox" && dashCooldown <= 0 && Player.isDashing)
        {
            TakeDamage();
            dashCooldown = 0.5f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") speed = oldSpeed;
    }

    public void TakeDamage()
    {
        enemyHP -= Player.damage;

        if (enemyHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
