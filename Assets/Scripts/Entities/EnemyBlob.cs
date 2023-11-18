using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBlob : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public float speed;
    public float enemyHP;
    public float agroRange;

    private Player Player;

    private bool agro;
    private float oldSpeed;
    private float attackCooldown = 0;
    private float dashCooldown = 0;
    private float reverseCooldown = 0.2f;

    //Pathfinding
    private NavMeshAgent agent;
    private Vector3 previousPosition;
    private Vector3 currentPosition;
    private float horizontalVelocity;
    private float verticalVelocity;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }
    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
        oldSpeed = speed;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, Player.gameObject.transform.position);
        if (distance <= agroRange) agro = true; else agro = false;
        if (agro) FollowPlayer();
        else
        {
            rb.velocity = new Vector2(0, 0);
        }

        animator.SetFloat("Horizontal", horizontalVelocity);
        animator.SetFloat("Vertical", verticalVelocity);
        animator.SetFloat("Speed", Mathf.Sqrt(horizontalVelocity * horizontalVelocity + verticalVelocity * verticalVelocity));

        // Cooldown
        if (attackCooldown < 1) attackCooldown += Time.deltaTime;
        if (dashCooldown > 0) dashCooldown -= Time.deltaTime;
        if (reverseCooldown <= 0.2) reverseCooldown += Time.deltaTime;
        if (reverseCooldown >= 0.2) speed = 2;
    }
    void FollowPlayer()
    {
        agent.SetDestination(new Vector3(Player.transform.position.x, Player.transform.position.y, transform.position.z));
        agent.speed = speed;

        currentPosition = transform.position;
        horizontalVelocity = (currentPosition.x - previousPosition.x) / Time.deltaTime;
        verticalVelocity = (currentPosition.y - previousPosition.y) / Time.deltaTime;
        previousPosition = currentPosition;

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
            GameObject.Find("Dungeon Generator").GetComponent<RoomManager>().EnemyDied();
            Death();
        }
    }
    public void Death()
    {
        Debug.Log("ENEMY DIED");
        Destroy(gameObject);
    }
}
