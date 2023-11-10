using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyBlob : MonoBehaviour
{
    public Rigidbody2D rb;
    private Player player;
    private Vector3 directionToPlayer;
    private Vector3 localScale;
    public float speed;
    public float enemyHP = 8;
    public Animator animator;
    private float oldSpeed;
    float attackCooldown = 0;
    float dashCooldown = 0;
    
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        oldSpeed = speed;
        rb = GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
    }

    void Update()
    {

        // Cooldown
        if (attackCooldown < 1) attackCooldown += Time.deltaTime;
        if (dashCooldown > 0) dashCooldown -= Time.deltaTime;

        

        // animator.SetFloat("Horizontal", rb.velocity.x);
        // animator.SetFloat("Vertical", rb.velocity.y);
        // animator.SetFloat("Speed", rb.velocity.sqrMagnitude);
        MoveEnemy();
        Debug.Log(rb.velocity);
    }
    private void MoveEnemy()
    {
        directionToPlayer = (player.transform.position - transform.position).normalized;
        rb.velocity = new Vector2(directionToPlayer.x, directionToPlayer.y) * speed;
    }
    private void LateUpdate()
    {
        if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(localScale.x, localScale.y, localScale.z);
        }
        else if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
        }
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
            if (!player.isDashing)
            {
                player.TakeDamage(1);
                attackCooldown = 0;
                animator.Play("Attack_Right");
            }
        }
        // Dash Take Damage
        if (collider.gameObject.tag == "dashHitbox" && dashCooldown <= 0 && player.isDashing)
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
        enemyHP -= player.damage;

        if (enemyHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
