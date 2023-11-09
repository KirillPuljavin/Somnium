using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // References
    public Animator animator;
    Vector2 movement;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private HeartUpdate heartsHUD;
    public LayerMask enemyLayers;
    public Transform attackPoint;

    // Variables
    public bool inDungeon = false;
    public int currRoom = 2;
    public int Hearts = 10;
    public int Stamina;
    public int ComponentsTier2;
    public int ComponentsTier3;
    public int WeaponEvo;

    public float speed = 3f;
    private bool canDash = true;
    public bool isDashing = false;
    private float dashingPower = 14f;
    private float dashingTime = 0.3f;
    private float dashingCooldown = 3f;
    string dashDirAnim = "Dash_Down";

    private string Facing = "down";
    public int damage = 2;
    float attackCooldown = 0;
    public float attackRange = 1f;

    void Start()
    {
        heartsHUD.UpdateHearts();
    }

    private float tpCooldown;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (tpCooldown >= 1 && collider.gameObject.tag == "Door")
        {
            transform.position = collider.gameObject.GetComponent<DoorMechanics>().targetDoorPos;
            tpCooldown = 0;
        }
    }

    private void Update()
    {
        // Cooldown
        if (tpCooldown <= 1) tpCooldown += Time.deltaTime;
        if (attackCooldown > 0) attackCooldown -= Time.deltaTime;

        if (isDashing) return;

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Mouse1) && canDash)
        {
            StartCoroutine(Dash());
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            HealPotion();
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        switch ((movement.x, movement.y))
        {
            case (1, 0):
                Facing = "right";
                dashDirAnim = "Dash_Right";
                break;
            case (-1, 0):
                Facing = "left";
                dashDirAnim = "Dash_Left";
                break;
            case (0, 1):
                Facing = "up";
                dashDirAnim = "Dash_Up";
                break;
            case (0, -1):
                Facing = "down";
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
            default:
                Facing = "up";
                dashDirAnim = "Dash_Up";
                break;
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && attackCooldown <= 0) Hit();
    }
    private void Hit()
    {
        attackCooldown = 0.5f;

        int randNumb = Random.Range(0, 1);
        switch (Facing)
        {
            case "right":
                attackPoint.position = gameObject.transform.position + new Vector3(1f, 0.5f, 0f);
                if (randNumb == 0) animator.Play("Attack_Right1"); else animator.Play("Attack_Right2");
                break;
            case "left":
                attackPoint.position = gameObject.transform.position + new Vector3(-1f, 0.5f, 0f);
                if (randNumb == 0) animator.Play("Attack_Left1"); else animator.Play("Attack_Left2");
                break;
            case "up":
                attackPoint.position = gameObject.transform.position + new Vector3(0f, 2f, 0f);
                animator.Play("Attack_Up");
                break;
            case "down":
                attackPoint.position = gameObject.transform.position + new Vector3(0f, -0.5f, 0f);
                animator.Play("Attack_Down");
                break;

            case "up-right":
                attackPoint.position = gameObject.transform.position + new Vector3(1f, 2f, 0f);
                animator.Play("Attack_Up");
                break;
            case "up-left":
                attackPoint.position = gameObject.transform.position + new Vector3(-1f, 2f, 0f);
                animator.Play("Attack_Up");
                break;
            case "down-right":
                attackPoint.position = gameObject.transform.position + new Vector3(1f, -0.5f, 0f);
                animator.Play("Attack_Down");
                break;
            case "down-left":
                attackPoint.position = gameObject.transform.position + new Vector3(-1f, -0.5f, 0f);
                animator.Play("Attack_Down");
                break;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyBlob enemyb = enemy.gameObject.GetComponent<EnemyBlob>();
            enemyb.enemyHP -= damage;
        }
    }
    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        if (movement.magnitude > 1)
        {
            rb.velocity = new Vector2(movement.x * (speed - 0.5f), movement.y * (speed - 0.5f));
        }
        else
        {
            rb.velocity = new Vector2(movement.x * speed, movement.y * speed);
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
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        rb.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    public void HealPotion()
    {
        if (Hearts < 10)
        {
            Hearts += 1;
            heartsHUD.UpdateHearts();
        }
    }
    public void TakeDamage(int amount)
    {
        Hearts -= amount;
        heartsHUD.UpdateHearts();

        if (Hearts <= 0) Death();
    }

    public void Death()
    {
        Debug.Log("YOU DIED");
        Destroy(gameObject);
    }
}