using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    // Variables
    public bool inDungeon = false;
    public int currRoom = 2;

    public float Hearts = 5;
    public int Stamina;
    public int ComponentsTier2;
    public int ComponentsTier3;
    public int WeaponEvo;
    private float horizontal;
    private float vertical;
    private float speed = 3f;

    private bool canDash = true;
    public bool isDashing = false;
    private float dashingPower = 14f;
    private float dashingTime = 0.3f;
    private float dashingCooldown = 3f;
    public Animator animator;
    Vector2 movement;
    public float damage = 1;
    public GameObject blob;

    private string Facing = "down";
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private HorizontalLayoutGroup heartsHUD;
    // UI
    [SerializeField] private GameObject heartIcon;
    float attackCooldown = 0;
    string dashDirAnim = "Dash_Down";
    public Transform attackPoint;
    public float attackRange = 1f;
    public LayerMask enemyLayers;


    void Start()
    {

    }
    private void Update()
    {
        if (attackCooldown > 0) attackCooldown -= Time.deltaTime;
        if (isDashing)
        {
            return;
        }

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


        if (movement.x == 1 && movement.y == 0)
        {
            Facing = "right";
            dashDirAnim = "Dash_Right";
        }
        if (movement.x == 1 && movement.y == 1)
        {
            Facing = "up-right";
        }
        if (movement.x == 1 && movement.y == -1)
        {
            Facing = "down-right";
        }
        if (movement.x == -1 && movement.y == 1)
        {
            Facing = "up-left";
        }
        if (movement.x == -1 && movement.y == -1)
        {
            Facing = "down-left";
        }
        if (movement.x == -1 && movement.y == 0)
        {
            Facing = "left";
            dashDirAnim = "Dash_Left";
        }
        if (movement.x == 0 && movement.y == 1)
        {
            Facing = "up";
            dashDirAnim = "Dash_Up";
        }
        if (movement.x == 0 && movement.y == -1)
        {
            Facing = "down";
            dashDirAnim = "Dash_Down";
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && attackCooldown <= 0)
        {
            Hit();
        }
    }
    private void Hit()
    {
        attackCooldown = 0.5f;
        if (Facing == "right")
        {
            Vector3 newPosition = gameObject.transform.position + new Vector3(1f, 0.5f, 0f);
            attackPoint.position = newPosition;
            float randNumb;
            randNumb = Random.Range(0, 4);
            if (randNumb < 2) animator.Play("Attack_Right1");
            if (randNumb >= 2) animator.Play("Attack_Right2");
        }
        else if (Facing == "left")
        {
            Vector3 newPosition = gameObject.transform.position + new Vector3(-1f, 0.5f, 0f);
            attackPoint.position = newPosition;
            float randNumb;
            randNumb = Random.Range(0, 4);
            if (randNumb < 2) animator.Play("Attack_Left1");
            if (randNumb >= 2) animator.Play("Attack_Left2");
        }
        else if (Facing == "up")
        {
            Vector3 newPosition = gameObject.transform.position + new Vector3(0f, 2f, 0f);
            attackPoint.position = newPosition;
            animator.Play("Attack_Up");
        }
        else if (Facing == "down")
        {
            Vector3 newPosition = gameObject.transform.position + new Vector3(0f, -0.5f, 0f);
            attackPoint.position = newPosition;
            animator.Play("Attack_Down");
        }
        else if (Facing == "up-right")
        {
            Vector3 newPosition = gameObject.transform.position + new Vector3(1f, 2f, 0f);
            attackPoint.position = newPosition;
            animator.Play("Attack_Up");
        }
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("hit" + enemy.name);
            enemy.
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
        Hearts += 0.5f;
        UpdateHearts();
    }
    public void TakeDamage(float amount)
    {
        Hearts -= amount;
        UpdateHearts();

        if (Hearts <= 0) Death();
    }

    private void UpdateHearts()
    {
        // foreach (Transform childObj in heartsHUD.transform)
        // {
        //     Destroy(childObj.gameObject);
        // }
        // Instantiate(heartIcon, heartsHUD.gameObject.transform);
    }


    public void Death()
    {
        Debug.Log("YOU DIED");
        Destroy(gameObject);
    }
}