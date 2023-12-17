using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BossCat : MonoBehaviour
{

    Player Player;

    public GameObject VisualCamera;
    public Rigidbody2D rb;
    public Animator catAnimator;
    public Animator headAnimator;
    public Animator healthBarAnimator;
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
    public int damage;
    public float attackRange;
    public float basicCooldown;


    public float activateRange;
    public bool inRange;
    public float activateZoom;
    private bool triggeredZoom;
    public bool spawned = false;
    public bool inAnimation;
    private float angle;
    private float distance;
    private float basicTimer;
    private float damageTimer;
    private float damageCooldown = 1f;
    private float bossInvisFrames = 1f;

    private bool inPhase2 = false;
    public GameObject Kitty;
    public GameObject Head;
    public GameObject LeftPaw;
    public GameObject RightPaw;
    public GameObject LeftTopPaw;
    public GameObject RightTopPaw;
    public GameObject LeftSweepPaw;
    public GameObject RightSweepPaw;

    public GameObject Potion;

    void Start()
    {
        basicTimer = basicCooldown;

        VisualCamera = GameObject.FindGameObjectWithTag("VirtualCamera");

        Player = GameObject.FindWithTag("Player").GetComponent<Player>();


        healthBar = GameObject.FindGameObjectWithTag("BossHealth");
        healthMask = GameObject.FindGameObjectWithTag("BossMask");
        healthBarAnimator = healthBar.GetComponent<Animator>();

        // Hide all basic attacks
        BasicRight.SetActive(false);
        BasicLeft.SetActive(false);
        BasicUp.SetActive(false);
        BasicDown.SetActive(false);

        // Hide all phase 2 objects
        Head.SetActive(false);
        RightPaw.SetActive(false);
        LeftPaw.SetActive(false);
        RightTopPaw.SetActive(false);
        LeftTopPaw.SetActive(false);
        RightSweepPaw.SetActive(false);
        LeftSweepPaw.SetActive(false);

        // Set Default Position/Scale
        transform.position = new UnityEngine.Vector3(0.45f, 150f, transform.position.z);
        rb.GetComponent<SpriteRenderer>().enabled = false;
        Kitty.SetActive(true);
    }


    UnityEngine.Vector3 previousPosition;
    void Update()
    {
        if (basicTimer <= basicCooldown) basicTimer += Time.deltaTime;
        if (damageTimer <= damageCooldown) damageTimer += Time.deltaTime;
        if (bossInvisFrames > 0) bossInvisFrames -= Time.deltaTime;

        // Zoom Out
        float targetOrthographicSize = triggeredZoom ? 9f : 5f;
        float currentOrthographicSize = VisualCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize;
        float newOrthographicSize = Mathf.MoveTowards(currentOrthographicSize, targetOrthographicSize, Time.deltaTime);
        VisualCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.OrthographicSize = newOrthographicSize;


        calcDistance();
        calcDirection();
        if (distance <= activateRange) { StartCoroutine(Begin()); }
        if (distance <= activateZoom) triggeredZoom = true;

        if (spawned && !inPhase2)
        {
            healthBarAnimator.SetBool("isActive", true);
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
        }
    }

    private bool alpha = false;
    private IEnumerator Begin()
    {
        if (!spawned)
        {
            rb.GetComponent<SpriteRenderer>().enabled = true;
            Kitty.SetActive(false);
            Player.inCutscene = true;
            if (!alpha)
            {
                catAnimator.Play("SpawnAnimation", 0);
                yield return new WaitForSeconds(1.583f);
                alpha = true;
            }
            else if (alpha)
            {
                catAnimator.Play("cat_Idle");
                inAnimation = true;
                yield return new WaitForSeconds(1.334f);
                inAnimation = false;
                spawned = true;
                Player.inCutscene = false;
            }
        }
    }


    IEnumerator ShakeCamera()
    {
        for (int i = 0; i < 3; i++)
        {
            VisualCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.Dutch = -1f;
            yield return new WaitForSeconds(0.1f);
            VisualCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.Dutch = 1f;
            yield return new WaitForSeconds(0.1f);
        }
        VisualCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.Dutch = 0f;
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
        healthMask.transform.localScale = new UnityEngine.Vector3(healthProcent * 8, 0.5f, 1);
    }

    void damagePlayer()
    {
        if (damageTimer >= damageCooldown)
        {
            Player.TakeDamage(damage);
            damageTimer = 0f;
        }
    }

    public void TakeDamage(int amount)
    {
        calcHealth();
        health -= amount;

        StartCoroutine(DamageIndicate());

        if (health <= 0)
        {
            StartCoroutine(BossDeath());
        }
        else if (health <= 100 && health + amount > 100)
        {
            StartCoroutine(Phase2());
        }
        else if (health <= 75 && health + amount > 75)
        {
            StartCoroutine(Phase2());
        }
        else if (health <= 50 && health + amount > 50)
        {
            StartCoroutine(Phase2());
        }
        else if (health <= 25 && health + amount > 25)
        {
            StartCoroutine(Phase2());
        }
    }
    private IEnumerator DamageIndicate()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.2f);
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1);
    }
    private IEnumerator BossDeath()
    {

        healthBar.SetActive(false);
        inAnimation = true;
        catAnimator.Play("Death");
        yield return new WaitForSeconds(5.5f);
        Destroy(gameObject);
    }
    private IEnumerator Phase2()
    {
        // Start Phase 2
        inPhase2 = true;
        transform.position = new UnityEngine.Vector3(0.45f, 170f, transform.position.z);
        Head.SetActive(true);
        RightPaw.SetActive(true);
        LeftPaw.SetActive(true);

        // ==================
        yield return new WaitForSeconds(3f);
        yield return TriggerAttack();
        yield return new WaitForSeconds(3f);
        yield return TriggerAttack();
        yield return new WaitForSeconds(3f);
        yield return TriggerAttack();
        yield return new WaitForSeconds(3f);

        // Return to normal
        transform.position = new UnityEngine.Vector3(0.45f, 150f, transform.position.z);
        Head.SetActive(false);
        RightPaw.SetActive(false);
        LeftPaw.SetActive(false);
        UnityEngine.Quaternion spawnRotation1 = UnityEngine.Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        UnityEngine.Quaternion spawnRotation2 = UnityEngine.Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        Debug.Log("1" + spawnRotation1);
        Debug.Log("2" + spawnRotation2);
        Instantiate(Potion, new UnityEngine.Vector3(5.5f, 150f, transform.position.z), spawnRotation1);
        Instantiate(Potion, new UnityEngine.Vector3(-4.5f, 150f, transform.position.z), spawnRotation2);
        inPhase2 = false;
        // ==================
    }

    private bool isAttacking = false;

    public IEnumerator TriggerAttack()
    {
        if (isAttacking)
        {
            yield return new WaitUntil(() => !isAttacking);
        }

        isAttacking = true;

        switch (Random.Range(0, 3))
        {
            case 0:
                yield return TopPawAttack();
                break;
            case 1:
                yield return SweepRightAttack();
                break;
            case 2:
                yield return SweepLeftAttack();
                break;
        }

        isAttacking = false;
    }
    private IEnumerator TopPawAttack()
    {
        headAnimator.Play("PrepTop");
        yield return new WaitForSeconds(1f);
        RightPaw.SetActive(false);
        RightTopPaw.transform.position = new UnityEngine.Vector3(Player.transform.position.x, 150f, transform.position.z);
        yield return new WaitForSeconds(0.5f);
        RightTopPaw.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        RightTopPaw.GetComponent<Collider2D>().enabled = true;
        StartCoroutine(ShakeCamera());
        yield return new WaitForSeconds(1f);

        headAnimator.Play("PrepTop");
        yield return new WaitForSeconds(1f);
        LeftPaw.SetActive(false);
        LeftTopPaw.transform.position = new UnityEngine.Vector3(Player.transform.position.x, 150f, transform.position.z);
        yield return new WaitForSeconds(0.5f);
        LeftTopPaw.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        LeftTopPaw.GetComponent<Collider2D>().enabled = true;
        StartCoroutine(ShakeCamera());
        yield return new WaitForSeconds(3f);

        RightTopPaw.GetComponent<Collider2D>().enabled = false;
        LeftTopPaw.GetComponent<Collider2D>().enabled = false;
        LeftTopPaw.SetActive(false);
        RightTopPaw.SetActive(false);

        LeftPaw.SetActive(true);
        RightPaw.SetActive(true);
    }
    private IEnumerator SweepRightAttack()
    {
        headAnimator.Play("PrepRight");
        yield return new WaitForSeconds(2f);
        RightPaw.SetActive(false);
        yield return new WaitForSeconds(1f);
        RightSweepPaw.transform.position = new UnityEngine.Vector3(7.25f, 150f, transform.position.z);
        RightSweepPaw.SetActive(true);
        RightSweepPaw.GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(1f);

        RightSweepPaw.SetActive(false);
        RightSweepPaw.GetComponent<Collider2D>().enabled = false;

        LeftPaw.SetActive(true);
        RightPaw.SetActive(true);
    }
    private IEnumerator SweepLeftAttack()
    {
        headAnimator.Play("PrepLeft");
        yield return new WaitForSeconds(2f);
        LeftPaw.SetActive(false);
        yield return new WaitForSeconds(1f);
        LeftSweepPaw.transform.position = new UnityEngine.Vector3(-7.25f, 150f, transform.position.z);
        LeftSweepPaw.SetActive(true);
        LeftSweepPaw.GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(1f);

        LeftSweepPaw.SetActive(false);
        LeftSweepPaw.GetComponent<Collider2D>().enabled = false;

        LeftPaw.SetActive(true);
        RightPaw.SetActive(true);
    }
    private IEnumerator SpawnEnemies()
    {
        Instantiate(Potion, new UnityEngine.Vector3(0.45f, 150f, transform.position.z), UnityEngine.Quaternion.identity);
        yield return new WaitForSeconds(2f);
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            if (!Player.isDashing)
            {
                damagePlayer();
            }
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
