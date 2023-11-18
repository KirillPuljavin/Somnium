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
    public GameObject healthBar;
    public GameObject healthMask;

    public GameObject BasicRight;
    public GameObject BasicLeft;
    public GameObject BasicUp;
    public GameObject BasicDown;

    public int health;
    public int maxHealth;
    public float healthProcent;
    public int speed;
    public float attackRange;
    public float basicCooldown;


    public float activateRange;

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
        /* healthBar = GameObject.FindGameObjectWithTag("BossHealth");
        healthMask = GameObject.FindGameObjectWithTag("BossMask");
        healthBar.SetActive(false); */

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

        calcDistance();
        calcDirection();
        if (distance <= activateRange) { StartCoroutine(Begin()); }

        if (spawned == true)
        {
            if (!inRange && !inAnimation)
            {
                UnityEngine.Vector3 targetPosition = new UnityEngine.Vector3(Player.gameObject.transform.position.x, Player.gameObject.transform.position.y, transform.position.z);
                transform.position = UnityEngine.Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            }
            else
            {
                transform.position = UnityEngine.Vector3.MoveTowards(transform.position, new UnityEngine.Vector2(Player.gameObject.transform.position.x, Player.gameObject.transform.position.y), 0);
            }
            StartCoroutine(Animations());
            /*          calcHealth();  */
        }

    }

    private IEnumerator Begin()
    {
        if (!spawned)
        {
            //Debug.Log("Begin");
            animator.SetBool("activate", true);
            yield return new WaitForSeconds(0.833f);
            /*      healthBar.SetActive(true); */
            spawned = true;
        }
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
                    animator.Play("BasicRight");
                    yield return new WaitForSeconds(0.4f);
                    BasicRight.SetActive(true);
                    Collider2D hitEnemy = Physics2D.OverlapCircle(BasicRight.transform.position, 1, LayerMask.GetMask("Player"));
                    if (hitEnemy != null)
                    {
                        damagePlayer();
                    }
                    basicTimer = 0;
                    yield return new WaitForSeconds(0.517f);
                    BasicRight.SetActive(false);
                    inAnimation = false;
                }
                else if (angle > 130 && angle < 180 || angle > -180 && angle < -130)
                {
                    inAnimation = true;
                    animator.Play("BasicLeft");
                    yield return new WaitForSeconds(0.4f);
                    BasicLeft.SetActive(true);
                    Collider2D hitEnemy = Physics2D.OverlapCircle(BasicLeft.transform.position, 1, LayerMask.GetMask("Player"));
                    if (hitEnemy != null)
                    {
                        damagePlayer();
                    }
                    basicTimer = 0;
                    yield return new WaitForSeconds(0.517f);
                    BasicLeft.SetActive(false);
                    inAnimation = false;
                }
                else if (angle < 130 && angle > 55)
                {
                    inAnimation = true;
                    animator.Play("BasicUp");
                    BasicRight.SetActive(true);
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

                    animator.Play("BasicUpL");
                    BasicLeft.SetActive(true);
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
                    animator.Play("BasicDown");
                    BasicLeft.SetActive(true);
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

                    animator.Play("BasicDownL");
                    BasicRight.SetActive(true);
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

    void calcHealth()
    {
        healthProcent = (float)health / (float)maxHealth;
        //healthMask.transform.localPosition = new UnityEngine.Vector3(healthProcent * -2, -0.3f, 0);
        healthMask.transform.localScale = new UnityEngine.Vector3(healthProcent * 4, 0.5f, 1);
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
        if ((maxHealth / health) == 2)
        {
            //StartCoroutine(Phase2());
        }
        else if (health <= 0)
        {
            //healthBar.SetActive(false);
            Destroy(gameObject);
        }
    }

    private IEnumerator Phase2()
    {
        yield return new WaitForSeconds(0.5835f);
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
