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
    private float speed = 2f;

    private bool canDash = true;
    public bool isDashing = false;
    private float dashingPower = 10f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 3f;
    public Animator animator;
    Vector2 movement;
    public float damage = 1;
    public GameObject EnemyObj;
    EnemyBlob enemyBlob;
    private string Facing = "down";
    public GameObject HitArea;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private HorizontalLayoutGroup heartsHUD;
    // UI
    [SerializeField] private GameObject heartIcon;

    void Start()
    {
        enemyBlob = EnemyObj.GetComponent<EnemyBlob>();
    }
    private void Update()
    {
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
        Debug.Log(movement.x);
        Debug.Log(movement.y);
        if (movement.x == 1 && movement.y == 0)
        {
            Facing = "right";
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
        }
        if (movement.x == 0 && movement.y == 1)
        {
            Facing = "up";
        }
        if (movement.x == 0 && movement.y == -1)
        {
            Facing = "down";
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Hit();
        }
    }
    private void Hit()
    {
        if (Facing == "right")
        {
            Vector3 newPosition = gameObject.transform.position + new Vector3(2f, 0f, 0f);
            Instantiate(HitArea, newPosition, Quaternion.identity);
        }
        if (Facing == "left")
        {
            Vector3 newPosition = gameObject.transform.position + new Vector3(-2f, 0f, 0f);
            Instantiate(HitArea, newPosition, Quaternion.identity);
        }
        if (Facing == "up")
        {
            Vector3 newPosition = gameObject.transform.position + new Vector3(0f, 2f, 0f);
            Instantiate(HitArea, newPosition, Quaternion.identity);
        }
        if (Facing == "down")
        {
            Vector3 newPosition = gameObject.transform.position + new Vector3(0f, -2f, 0f);
            Instantiate(HitArea, newPosition, Quaternion.identity);
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
        }
        else
        {
            rb.velocity = new Vector2(movement.x * dashingPower, movement.y * dashingPower);
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