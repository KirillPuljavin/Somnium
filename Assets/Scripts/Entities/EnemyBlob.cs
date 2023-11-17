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
    private Vector3 directionToPlayer;
    private Vector3 localScale;

    private float oldSpeed;
    private float attackCooldown = 0;
    private float dashCooldown = 0;
    private bool agro;
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
        oldSpeed = speed;
        rb = GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, Player.gameObject.transform.position);

        if (distance <= agroRange) agro = true; else agro = false;
        if (agro) SetAgentPosition();
        else
        {
            rb.velocity = new Vector2(0, 0);
        }

        // Cooldown
        if (attackCooldown < 1) attackCooldown += Time.deltaTime;
        if (dashCooldown > 0) dashCooldown -= Time.deltaTime;
        if (reverseCooldown <= 0.2) reverseCooldown += Time.deltaTime;
        if (reverseCooldown >= 0.2) speed = 2;

        UnityEngine.Vector3 currentPosition = transform.position;

        float horizontalVelocity = (currentPosition.x - previousPosition.x) / Time.deltaTime;
        float verticalVelocity = (currentPosition.y - previousPosition.y) / Time.deltaTime;

        animator.SetFloat("Horizontal", horizontalVelocity);
        animator.SetFloat("Vertical", verticalVelocity);
        animator.SetFloat("speed", Mathf.Sqrt(horizontalVelocity * horizontalVelocity + verticalVelocity * verticalVelocity));

        previousPosition = currentPosition;

        SetTargetPosition();
        agent.speed = speed;
    }
    // private void MoveEnemy()
    // {
    //     directionToPlayer = (Player.transform.position - transform.position).normalized;
    //     rb.velocity = new Vector2(directionToPlayer.x, directionToPlayer.y) * speed;
    // }

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

    void SetTargetPosition()
    {
        target = Player.transform.position;
    }

    void SetAgentPosition()
    {
        agent.SetDestination(new Vector3(target.x, target.y, transform.position.z));
    }
}
