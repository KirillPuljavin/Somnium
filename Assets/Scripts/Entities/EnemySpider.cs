using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpider : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public float speed;
    public float enemyHP;
    public float agroRange;
    public GameObject SpiderWebArea;

    private Player Player;
    private Vector3 directionToPlayer;
    private Vector3 localScale;

    private float oldSpeed;
    private float attackCooldown = 0;
    private float dashCooldown = 0;
    private bool agro;
    private float reverseCooldown = 0.2f;
    private float webCooldown = 5;
    private bool isShooting = false;
    private float isShootingCooldown = 0;
    private float shootingDir;

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
        if (agro && !isShooting) MoveEnemy();
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
        if (agro && webCooldown >= 10) SpiderWebAttack();

        // Cooldown
        if (attackCooldown < 1) attackCooldown += Time.deltaTime;
        if (dashCooldown > 0) dashCooldown -= Time.deltaTime;
        if (reverseCooldown <= 0.2) reverseCooldown += Time.deltaTime;
        if (webCooldown <= 10) webCooldown += Time.deltaTime;
        if (isShootingCooldown <= 0.5f) isShootingCooldown += Time.deltaTime;
        if (reverseCooldown >= 0.2) speed = 2;

        animator.SetFloat("Horizontal", rb.velocity.x);
        animator.SetFloat("Speed", rb.velocity.sqrMagnitude);

        if (isShootingCooldown >= 0.5) isShooting = false;
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
        reverseCooldown = 0;
        speed = -2;

        if (enemyHP <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void SpiderWebAttack()
    {
        isShootingCooldown = 0;
        isShooting = true;
        //Add Shoot Animation
        shootingDir = rb.velocity.x;
        rb.velocity = new Vector2(0, 0);
        if (shootingDir <= 0) animator.Play("Attack_Ranged_Left");
        if (shootingDir >= 0) animator.Play("Attack_Ranged_Right");
        webCooldown = 0;
        Instantiate(SpiderWebArea, Player.transform.position, Quaternion.identity);
    }
}
