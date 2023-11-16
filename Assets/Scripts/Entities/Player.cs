using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // References
    public Animator animator;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private HeartUpdate heartsHUD;
    [SerializeField] private UIScript uiScript;
    [SerializeField] private GameObject staminaBar;
    [SerializeField] private GameObject staminaMask;
    public LayerMask enemyLayers;
    public Transform attackPoint;
    public Light2D myLight;

    // Variables
    public bool inDungeon = false;
    public int currRoom = 2;
    public int Hearts = 10;
    public int MaxHearts = 10;
    public float stamina;
    public float staminaProcent = 0;
    public int Components;
    public int WeaponEvo = 0;
    public int Vision = 2;

    public float speed;
    public bool isDashing = false;
    public int damage;
    public int dashDamage;
    public float attackCooldown;
    public float dashingPower = 12f;

    //Chest Bools
    public bool Card1Picked = false;
    public bool Card2Picked = false;
    public bool Card3Picked = false;
    public bool Card4Picked = false;

    private float attackTime;
    private float dashingTime = 0.3f;
    private float dashingCooldown = 2.7f;
    private bool canDash = true;

    public float attackRange;
    private string dashDirAnim = "Dash_Down";
    private string Facing = "down";
    private Vector2 movement;

    private float tpCooldown;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (tpCooldown >= 1 && collider.gameObject.tag == "Door")
        {
            transform.position = collider.gameObject.GetComponent<DoorMechanics>().targetDoorPos;
            currRoom = collider.gameObject.GetComponent<DoorMechanics>().roomIndex;
            tpCooldown = 0;
        }
    }

    private void Start()
    {
        uiScript = GameObject.FindWithTag("Components").GetComponent<UIScript>();
    }
    private void Update()
    {
        // Cooldown
        if (tpCooldown <= 1) tpCooldown += Time.deltaTime;
        if (attackTime <= attackCooldown) attackTime += Time.deltaTime;
        if (stamina < dashingCooldown) stamina += Time.deltaTime;
        if (stamina < dashingCooldown) UpdateStamina();
        if (isDashing) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Mouse position Calculations and storing for checking where to attack.
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 temp = new Vector3(mousePos.x, mousePos.y, 0);
        Vector3 direction = temp - gameObject.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        // Controls
        if (Input.GetKeyDown(KeyCode.Mouse0) && attackTime >= attackCooldown) StartCoroutine(Hit());
        if (Input.GetKeyDown(KeyCode.Mouse1) && canDash && movement.sqrMagnitude != 0) StartCoroutine(Dash());
        if (Input.GetKeyDown(KeyCode.H)) HealPotion();

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        switch ((movement.x, movement.y))
        {
            case (1, 0):
                dashDirAnim = "Dash_Right";
                break;
            case (-1, 0):
                dashDirAnim = "Dash_Left";
                break;
            case (0, 1):
                dashDirAnim = "Dash_Up";
                break;
            case (0, -1):
                dashDirAnim = "Dash_Down";
                break;
            case (1, 1):
                Facing = "up-right";
                dashDirAnim = "Dash_Up";
                break;
            case (1, -1):
                Facing = "down-right";
                dashDirAnim = "Dash_Down";
                break;
            case (-1, 1):
                Facing = "up-left";
                dashDirAnim = "Dash_Up";
                break;
            case (-1, -1):
                Facing = "down-left";
                dashDirAnim = "Dash_Down";
                break;
        }

        if (angle > -45 && angle < 45)
        {
            Facing = "up";
        }
        else if (angle > -135 && angle < -45)
        {
            Facing = "right";
        }
        else if (angle > -225 && angle < -135)
        {
            Facing = "down";
        }
        else if (angle > -270 && angle < -225 || angle > 45 && angle < 90)
        {
            Facing = "left";
        }
        else
        {
            Debug.Log("Error ANGLE");
        }

        myLight.pointLightOuterRadius = Vision * 4;
    }
    private void FixedUpdate()
    {
        if (isDashing) return;
        if (movement.magnitude > 1) rb.velocity = new Vector2(movement.x * (speed - 0.5f), movement.y * (speed - 0.5f));
        else rb.velocity = new Vector2(movement.x * speed, movement.y * speed);
        uiScript.UpdateUI();
    }

    public void UpgradeEvo()
    {
        // Make changes based on evo
        switch (WeaponEvo)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
        }
        WeaponEvo++;
        Debug.Log("You LEVELED UP to " + WeaponEvo);
    }

    private IEnumerator Hit()
    {
        attackTime = 0;
        int randNumb = UnityEngine.Random.Range(0, 1);
        switch (Facing)
        {
            case "right":
                attackPoint.position = gameObject.transform.position + new Vector3(attackRange, 0f, 0f);
                if (randNumb == 0) animator.Play("Attack_Right1"); else animator.Play("Attack_Right2");
                break;
            case "left":
                attackPoint.position = gameObject.transform.position + new Vector3(-attackRange, 0f, 0f);
                if (randNumb == 0) animator.Play("Attack_Left1"); else animator.Play("Attack_Left2");
                break;
            case "up":
                attackPoint.position = gameObject.transform.position + new Vector3(0f, attackRange, 0f);
                animator.Play("Attack_Up");
                break;
            case "down":
                attackPoint.position = gameObject.transform.position + new Vector3(0f, -attackRange, 0f);
                animator.Play("Attack_Down");
                break;
            case "up-right":
                attackPoint.position = gameObject.transform.position + new Vector3(attackRange, attackRange, 0f);
                animator.Play("Attack_Up");
                break;
            case "up-left":
                attackPoint.position = gameObject.transform.position + new Vector3(-attackRange, attackRange, 0f);
                animator.Play("Attack_Up");
                break;
            case "down-right":
                attackPoint.position = gameObject.transform.position + new Vector3(attackRange, -attackRange, 0f);
                animator.Play("Attack_Down");
                break;
            case "down-left":
                attackPoint.position = gameObject.transform.position + new Vector3(-attackRange, -attackRange, 0f);
                animator.Play("Attack_Down");
                break;
        }
        yield return new WaitForSeconds(0.2f);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, 1, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.GetComponent<EnemyBlob>() != null) enemy.gameObject.GetComponent<EnemyBlob>().TakeDamage(damage);
            if (enemy.gameObject.GetComponent<EnemyFrog>() != null) enemy.gameObject.GetComponent<EnemyFrog>().TakeDamage(damage);
            if (enemy.gameObject.GetComponent<EnemySpider>() != null) enemy.gameObject.GetComponent<EnemySpider>().TakeDamage(damage);
            if (enemy.gameObject.GetComponent<BossCat>() != null) enemy.gameObject.GetComponent<BossCat>().TakeDamage(damage);
        }
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        if (movement.magnitude > 1)
        {
            rb.velocity = new Vector2(movement.x * (dashingPower - 3), movement.y * (dashingPower - 3));
            animator.Play(dashDirAnim);
        }
        else
        {
            rb.velocity = new Vector2(movement.x * dashingPower, movement.y * dashingPower);
            animator.Play(dashDirAnim);
        }
        stamina = 0;
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        rb.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    public void HealPotion()
    {
        if (Hearts < MaxHearts)
        {
            Hearts += 1;
            heartsHUD.UpdateHearts();
        }
    }
    public void TakeDamage(int amount)
    {
        Hearts -= amount;
        heartsHUD.UpdateHearts();
        StartCoroutine(DamageFlash());

        if (Hearts <= 0) Death();
    }

    void UpdateStamina()
    {
        staminaProcent = stamina / dashingCooldown;

        staminaBar.transform.localScale = new Vector3(transform.localScale.x * 25, transform.localScale.y * 25, transform.localScale.z);

        staminaMask.transform.localScale = new Vector3(staminaProcent * 500, transform.localScale.y * 25, transform.localScale.z);

    }

    private IEnumerator DamageFlash()
    {
        myLight.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        myLight.color = Color.white;
    }

    public void Death()
    {
        Debug.Log("YOU DIED");
        Destroy(gameObject);
    }

}