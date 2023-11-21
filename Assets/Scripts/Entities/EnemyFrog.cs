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
    public int damageHearts;
    public float agroRange;

    private Player Player;
    private Vector3 directionToPlayer;
    private Vector3 localScale;

    private bool agro;
    private float closeRange = 4;
    private float shootCooldown = 0;
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
        rb = GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, Player.gameObject.transform.position);
        if (distance <= agroRange) agro = true; else agro = false;
        {
            if (distance >= closeRange && agro) FollowPlayer();
            else
            {
                agent.speed = 0;
                animator.SetBool("Shooting", true);
                animator.Play("Idle");
            }
        }

        animator.SetFloat("Horizontal", horizontalVelocity);
        animator.SetFloat("Vertical", verticalVelocity);
        animator.SetFloat("Speed", Mathf.Sqrt(horizontalVelocity * horizontalVelocity + verticalVelocity * verticalVelocity));

        // Cooldown
        if (shootCooldown <= 4 && agro) shootCooldown += Time.deltaTime;
        if (dashCooldown > 0) dashCooldown -= Time.deltaTime;
        if (shootCooldown >= 4 && agro) Shoot();
    }
    void FollowPlayer()
    {
        animator.SetBool("Shooting", false);
        agent.SetDestination(new Vector3(Player.transform.position.x, Player.transform.position.y, transform.position.z));
        agent.speed = speed;

        currentPosition = transform.position;
        horizontalVelocity = (currentPosition.x - previousPosition.x) / Time.deltaTime;
        verticalVelocity = (currentPosition.y - previousPosition.y) / Time.deltaTime;
        previousPosition = currentPosition;
    }

    void Shoot()
    {
        shootCooldown = 0;

        // Instantiate a fly
        GameObject fly = Instantiate(flyPrefab, transform.position, Quaternion.identity);
        fly.GetComponent<FrogFly>().damageHearts = damageHearts;
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
