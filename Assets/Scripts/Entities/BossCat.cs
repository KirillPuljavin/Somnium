using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BossCat : MonoBehaviour
{

    Player Player;
    public Rigidbody2D rb;
    public Animator catAnimator;
    public Animator headAnimator;
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

    private bool inPhase2 = false;
    public GameObject Head;
    public GameObject LeftPaw;
    public GameObject RightPaw;
    public GameObject LeftTopPaw;
    public GameObject RightTopPaw;

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

        //transform.localScale = new UnityEngine.Vector3(1, 1, 1);
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

        if (spawned && !inPhase2)
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
            catAnimator.SetBool("activate", true);
            //transform.localScale = new UnityEngine.Vector3(3, 3, 3);
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

        catAnimator.SetFloat("Horizontal", verticalVelocity);
        catAnimator.SetFloat("Vertical", horizontalVelocity);
        catAnimator.SetFloat("speed", Mathf.Sqrt(horizontalVelocity * horizontalVelocity + verticalVelocity * verticalVelocity));

        previousPosition = currentPosition;

        if (distance <= attackRange)
        {
            inRange = true;
            if (basicTimer >= basicCooldown)
            {
                if (angle < 55 && angle > 0)
                {
                    inAnimation = true;
                    catAnimator.Play("BasicRight");
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
                    catAnimator.Play("BasicLeft");
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
                    catAnimator.Play("BasicUp");
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

                    catAnimator.Play("BasicUpL");
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
                    catAnimator.Play("BasicDown");
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

                    catAnimator.Play("BasicDownL");
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
            StartCoroutine(Phase2());
        }
        else if (health <= 0)
        {
            //healthBar.SetActive(false);
            Destroy(gameObject);
        }
    }

    private IEnumerator Phase2()
    {
        //Start Phase 2
        inPhase2 = true;
        transform.position = new UnityEngine.Vector3(5f, 20f, transform.position.z);
        Head.SetActive(true);
        RightPaw.SetActive(true);
        LeftPaw.SetActive(true);
        // ==================

        yield return new WaitForSeconds(3f);

        //Top Paw Attack
        headAnimator.Play("PrepTop");
        RightPaw.SetActive(false);
        yield return new WaitForSeconds(1f);
        float randomValue = Random.Range(6.5f, 12f);
        RightTopPaw.transform.position = new UnityEngine.Vector3(randomValue, 0.5f, transform.position.z);
        RightTopPaw.SetActive(true);
        yield return new WaitForSeconds(1f);

        LeftPaw.SetActive(false);
        yield return new WaitForSeconds(1f);
        randomValue = Random.Range(-2.5f, 2.5f);
        LeftTopPaw.transform.position = new UnityEngine.Vector3(randomValue, 0.5f, transform.position.z);
        LeftTopPaw.SetActive(true);
        yield return new WaitForSeconds(3f);

        LeftTopPaw.SetActive(false);
        RightTopPaw.SetActive(false);
        LeftPaw.SetActive(true);
        RightPaw.SetActive(true);
        // ==================

        yield return new WaitForSeconds(3f);

        //Sweep Attack
        //Head.animator.play("PrepTop");
            


        //===================

        //Return to normal
        transform.position = new UnityEngine.Vector3(4.6f, 0f, transform.position.z);
        Head.SetActive(false);
        RightPaw.SetActive(false);
        LeftPaw.SetActive(false);
        inPhase2 = false;
        // ==================

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
