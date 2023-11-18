using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFrog : MonoBehaviour
{
    public GameObject flyPrefab;
    public Rigidbody2D rb;
    public Animator animator;
    public float speed;
    public float enemyHP;
    public float agroRange;

    private Player Player;
    private Vector3 directionToPlayer;
    private Vector3 localScale;

    private bool agro;
    private float closeRange = 4;
    private float shootCooldown = 0;
    private float dashCooldown = 0;
    private float reverseCooldown = 0.2f;

    //Pathfinding
    private Vector3 target;
    NavMeshAgent agent;

    UnityEngine.Vector3 previousPosition;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, Player.gameObject.transform.position);

        if (distance <= agroRange) agro = true; else agro = false;
        if (reverseCooldown >= 0.2)
        {
            speed = 1;
            if (distance >= closeRange && agro) SetAgentPosition();
            else
            {
                rb.velocity = new Vector2(0, 0);
                animator.Play("Idle");
            }
        }

        // Cooldown
        if (shootCooldown <= 4 && agro) shootCooldown += Time.deltaTime;
        if (dashCooldown > 0) dashCooldown -= Time.deltaTime;
        if (reverseCooldown <= 0.2) reverseCooldown += Time.deltaTime;
        if (reverseCooldown >= 0.2) speed = 2;

        if (shootCooldown >= 4 && agro) Shoot();

        UnityEngine.Vector3 currentPosition = transform.position;

        float horizontalVelocity = (currentPosition.x - previousPosition.x) / Time.deltaTime;
        float verticalVelocity = (currentPosition.y - previousPosition.y) / Time.deltaTime;

        animator.SetFloat("Horizontal", horizontalVelocity);
        // animator.SetFloat("Vertical", verticalVelocity);
        animator.SetFloat("Speed", Mathf.Sqrt(horizontalVelocity * horizontalVelocity + verticalVelocity * verticalVelocity));

        previousPosition = currentPosition;

        SetTargetPosition();
        agent.speed = speed;
    }
    // private void MoveEnemy()
    // {
    //     directionToPlayer = (Player.transform.position - transform.position).normalized;
    //     rb.velocity = new Vector2(directionToPlayer.x, directionToPlayer.y) * speed;
    // }
    // private void LateUpdate()
    // {
    //     if (rb.velocity.x > 0)
    //     {
    //         transform.localScale = new Vector3(localScale.x, localScale.y, localScale.z);
    //     }
    //     else if (rb.velocity.x < 0)
    //     {
    //         transform.localScale = new Vector3(-localScale.x, localScale.y, localScale.z);
    //     }
    // }

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
            TakeDamage(Player.dashDamage);
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

    public void TakeDamage(int amount)
    {
        enemyHP -= amount;
        reverseCooldown = 0;
        speed = -2;

        if (enemyHP <= 0)
        {
            GameObject.Find("Dungeon Generator").GetComponent<RoomManager>().EnemyDied();
            Death();
        }
    }
    public void Death()
    {
        Debug.Log("ENEMY DIED");
        Destroy(gameObject);
    }

    void SetTargetPosition()
    {
        target = Player.transform.position;
    }

    void SetAgentPosition()
    {
        agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
    }
}
