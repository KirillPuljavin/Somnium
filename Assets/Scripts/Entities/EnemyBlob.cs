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
    public int damageHearts;
    public float agroRange;

    private Player Player;
    private bool agro;
    private float oldSpeed;
    private float attackCooldown = 0;
    private float dashCooldown = 0;

    // Pathfinding
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
            agent.speed = 0;
            animator.Play("Idle");
        }

        animator.SetFloat("Horizontal", horizontalVelocity);
        animator.SetFloat("Vertical", verticalVelocity);
        animator.SetFloat("Speed", Mathf.Sqrt(horizontalVelocity * horizontalVelocity + verticalVelocity * verticalVelocity));

        // Cooldown
        if (attackCooldown < 1) attackCooldown += Time.deltaTime;
        if (dashCooldown > 0) dashCooldown -= Time.deltaTime;
    }
    void FollowPlayer()
    {
        animator.SetBool("Hitting", false);
        agent.SetDestination(new Vector3(Player.transform.position.x, Player.transform.position.y, transform.position.z));
        agent.speed = speed;

        currentPosition = transform.position;
        horizontalVelocity = (currentPosition.x - previousPosition.x) / Time.deltaTime;
        verticalVelocity = (currentPosition.y - previousPosition.y) / Time.deltaTime;
        previousPosition = currentPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") agent.speed = 0.005f;
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        // Attack Player
        if (collider.gameObject.tag == "Player" && attackCooldown >= 1)
        {
            if (!Player.isDashing)
            {
                Player.TakeDamage(damageHearts);
                attackCooldown = 0;
                if (Player.transform.position.x < transform.position.x)
                {
                    animator.SetBool("Hitting", true);
                    animator.Play("Attack_Left");
                }
                else if (Player.transform.position.x > transform.position.x)
                {
                    animator.SetBool("Hitting", true);
                    animator.Play("Attack_Right");
                }
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
            GameObject.Find("Dungeon Generator").GetComponent<RoomManager>().EnemyDied();
            Death();
        }
        else StartCoroutine(DamageIndicate());
    }
    private IEnumerator DamageIndicate()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.2f);
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }

    private void Death()
    {
        ParticleSystem deathParticle = Instantiate(GetComponentInChildren<ParticleSystem>(), transform.position, Quaternion.identity);
        if (deathParticle != null)
        {
            deathParticle.Play();
            Destroy(deathParticle.gameObject, deathParticle.main.duration);
        }
        Destroy(gameObject);
    }
}
