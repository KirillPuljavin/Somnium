using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BossCat : MonoBehaviour
{

    Player Player;
    public Rigidbody2D rb;
    public Animator animator;

    public GameObject BasicRight;
    public GameObject BasicLeft;
    public GameObject BasicUp;
    public GameObject BasicDown;

    public int health;
    public int speed;
    public float attackRange;
    public float basicCooldown;


    public bool inRange;
    public bool spawned = false;
    public bool inAnimation;
    private float angle;
    private float distance;
    private float basicTimer;
    private float damageTimer;
    private float damageCooldown = 1f;
    private float bossInvisFrames = 1f;

    void Start()
    {
        basicTimer = basicCooldown;

        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
        StartCoroutine(Begin());

        // Hide all basic attacks
        BasicRight.SetActive(false);
        BasicLeft.SetActive(false);
        BasicUp.SetActive(false);
        BasicDown.SetActive(false);
    }



    UnityEngine.Vector3 previousPosition;
    void Update()
    {
        if (basicTimer <= basicCooldown) basicTimer += Time.deltaTime;
        if (damageTimer <= damageCooldown) damageTimer += Time.deltaTime;
        if (bossInvisFrames > 0) bossInvisFrames -= Time.deltaTime;

        if (spawned == true)
        {
            if (!inRange && !inAnimation)
            {
                transform.position = UnityEngine.Vector3.MoveTowards(transform.position, new UnityEngine.Vector2(Player.gameObject.transform.position.x, Player.gameObject.transform.position.y), speed * Time.deltaTime);
            }
            else
            {
                transform.position = UnityEngine.Vector3.MoveTowards(transform.position, new UnityEngine.Vector2(Player.gameObject.transform.position.x, Player.gameObject.transform.position.y), 0);
            }
            StartCoroutine(Animations());
            calcDistance();
            calcDirection();
        }
    }

    private IEnumerator Begin()
    {
        animator.SetBool("spawned", true);
        yield return new WaitForSeconds(0.833f);
        spawned = true;
    }


    private IEnumerator Animations()
    {
        UnityEngine.Vector3 currentPosition = transform.position;

        float horizontalVelocity = (currentPosition.x - previousPosition.x) / Time.deltaTime;
        float verticalVelocity = (currentPosition.y - previousPosition.y) / Time.deltaTime;

        animator.SetFloat("Horizontal", verticalVelocity);
        animator.SetFloat("Vertical", horizontalVelocity);
        animator.SetFloat("speed", Mathf.Sqrt(horizontalVelocity * horizontalVelocity + verticalVelocity * verticalVelocity));

        previousPosition = currentPosition;

        if (distance <= attackRange)
        {
            inRange = true;
            if (basicTimer >= basicCooldown)
            {
                if (angle < 55 && angle > 0)
                {
                    inAnimation = true;
                    BasicRight.SetActive(true);
                    animator.Play("BasicRight");
                    Collider2D hitEnemy = Physics2D.OverlapCircle(BasicRight.transform.position, 1, LayerMask.GetMask("Player"));
                    if (hitEnemy != null)
                    {
                        damagePlayer();
                    }
                    basicTimer = 0;
                    yield return new WaitForSeconds(0.917f);
                    BasicRight.SetActive(false);
                    inAnimation = false;
                }
                else if (angle > 130 && angle < 180 || angle > -180 && angle < -130)
                {
                    inAnimation = true;
                    BasicLeft.SetActive(true);
                    animator.Play("BasicLeft");
                    Collider2D hitEnemy = Physics2D.OverlapCircle(BasicLeft.transform.position, 1, LayerMask.GetMask("Player"));
                    if (hitEnemy != null)
                    {
                        damagePlayer();
                    }
                    basicTimer = 0;
                    yield return new WaitForSeconds(0.917f);
                    BasicLeft.SetActive(false);
                    inAnimation = false;
                }
                else if (angle < 130 && angle > 55)
                {
                    inAnimation = true;
                    BasicRight.SetActive(true);
                    animator.Play("BasicUp");
                    yield return new WaitForSeconds(0.5835f);
                    BasicRight.SetActive(false);
                    BasicUp.SetActive(true);
                    Collider2D hitEnemy = Physics2D.OverlapCircle(BasicUp.transform.position, 1, LayerMask.GetMask("Player"));
                    if (hitEnemy != null)
                    {
                        damagePlayer();
                    }
                    basicTimer = 0;
                    yield return new WaitForSeconds(0.5835f);
                    BasicUp.SetActive(false);

                    BasicLeft.SetActive(true);
                    animator.Play("BasicUpL");
                    yield return new WaitForSeconds(0.5835f);
                    BasicLeft.SetActive(false);
                    BasicUp.SetActive(true);
                    hitEnemy = Physics2D.OverlapCircle(BasicDown.transform.position, 1, LayerMask.GetMask("Player"));
                    if (hitEnemy != null)
                    {
                        damagePlayer();
                    }
                    basicTimer = 0;
                    yield return new WaitForSeconds(0.5835f);
                    BasicUp.SetActive(false);

                    inAnimation = false;
                }
                else if (angle > -130 && angle < 0)
                {
                    inAnimation = true;
                    BasicLeft.SetActive(true);
                    animator.Play("BasicDown");
                    yield return new WaitForSeconds(0.5835f);
                    BasicLeft.SetActive(false);
                    BasicDown.SetActive(true);
                    Collider2D hitEnemy = Physics2D.OverlapCircle(BasicDown.transform.position, 1, LayerMask.GetMask("Player"));
                    if (hitEnemy != null)
                    {
                        damagePlayer();
                    }
                    basicTimer = 0;
                    yield return new WaitForSeconds(0.5835f);
                    BasicDown.SetActive(false);

                    BasicRight.SetActive(true);
                    animator.Play("BasicDownL");
                    yield return new WaitForSeconds(0.5835f);
                    BasicRight.SetActive(false);
                    BasicDown.SetActive(true);
                    hitEnemy = Physics2D.OverlapCircle(BasicDown.transform.position, 1, LayerMask.GetMask("Player"));
                    if (hitEnemy != null)
                    {
                        damagePlayer();
                    }
                    basicTimer = 0;
                    yield return new WaitForSeconds(0.5835f);
                    BasicDown.SetActive(false);

                    inAnimation = false;
                }
                else
                {
                    Debug.Log("Error");
                }
            }
        }
        else
        {
            inRange = false;
        }
    }

    void calcDistance()
    {
        distance = UnityEngine.Vector2.Distance(transform.position, Player.gameObject.transform.position);
    }


    void calcDirection()
    {
        UnityEngine.Vector3 direction = new UnityEngine.Vector3(Player.gameObject.transform.position.x - transform.position.x, Player.gameObject.transform.position.y - transform.position.y + 0.5f, 0);
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    void damagePlayer()
    {
        if (damageTimer >= damageCooldown)
        {
            Player.TakeDamage(1);
            damageTimer = 0f;
        }
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (!Player.isDashing)
            {
                damagePlayer();
            }
        }

        // Dash Take Damage
        if (collider.gameObject.tag == "dashHitbox" && bossInvisFrames <= 0 && Player.isDashing)
        {
            TakeDamage(Player.dashDamage);
            bossInvisFrames = 0.5f;
        }
    }

}
