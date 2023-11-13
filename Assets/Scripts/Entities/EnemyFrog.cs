using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class EnemyFrog : MonoBehaviour
{
    public GameObject flyPrefab;
    Player Player;
    public float speed;
    public float enemyHP;
    public float agroRange;


    private bool agro;
    private float closeRange = 4;
    private float shootCooldown = 0;
    private float dashCooldown = 0;

    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, Player.gameObject.transform.position);

        if (distance <= agroRange) agro = true; else agro = false;
        if (distance >= closeRange && agro)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector2(Player.gameObject.transform.position.x, Player.gameObject.transform.position.y + 0.5f), speed * Time.deltaTime);
        }

        // Cooldown
        if (shootCooldown <= 4) shootCooldown += Time.deltaTime;
        if (dashCooldown > 0) dashCooldown -= Time.deltaTime;

        if (shootCooldown >= 1 && agro) Shoot();
    }

    void Shoot()
    {
        shootCooldown = 0;

        // Instantiate a fly
        GameObject fly = Instantiate(flyPrefab, transform.position, Quaternion.identity);
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
        // Dash Take Damage
        if (collider.gameObject.tag == "dashHitbox" && dashCooldown <= 0 && Player.isDashing)
        {
            TakeDamage();
            dashCooldown = 0.5f;
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
        enemyHP -= Player.damage;

        if (enemyHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
