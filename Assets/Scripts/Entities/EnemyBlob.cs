using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyBlob : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public float speed;
    public float enemyHP;
    public float agroRange;

    private Player Player;
    private Vector3 directionToPlayer;
    private Vector3 localScale;

    private float oldSpeed;
    private float attackCooldown = 0;
    private float dashCooldown = 0;
    private bool agro;

    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
        oldSpeed = speed;
        rb = GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, Player.gameObject.transform.position);

        if (distance <= agroRange) agro = true; else agro = false;
        if (agro) MoveEnemy();
        else
        {
            rb.velocity = new Vector2(0, 0);
        }

        // Cooldown
        if (attackCooldown < 1) attackCooldown += Time.deltaTime;
        if (dashCooldown > 0) dashCooldown -= Time.deltaTime;

        animator.SetFloat("Horizontal", rb.velocity.x);
        animator.SetFloat("Vertical", rb.velocity.y);
        animator.SetFloat("Speed", rb.velocity.sqrMagnitude);
    }
    private void MoveEnemy()
    {
        directionToPlayer = (Player.transform.position - transform.position).normalized;
        rb.velocity = new Vector2(directionToPlayer.x, directionToPlayer.y) * speed;
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
                animator.Play("Attack_Right");
            }
        }
        // Dash Take Damage
        if (collider.gameObject.tag == "dashHitbox" && dashCooldown <= 0 && Player.isDashing)
        {
            TakeDamage(Player.dashDamage);
            dashCooldown = 0.5f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") speed = oldSpeed;
    }

    public void TakeDamage(int amount)
    {
        enemyHP -= amount;

        if (enemyHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
