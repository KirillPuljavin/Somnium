using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    // Variables
    public float Hearts = 5;
    public int Stamina;
    public int ComponentsTier2;
    public int ComponentsTier3;
    public int WeaponEvo;
    private float horizontal;
    private float vertical;
    private float speed = 2f;
    private bool isFacingRight = true;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 10f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 3f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private HorizontalLayoutGroup heartsHUD;
    // UI
    [SerializeField] private GameObject heartIcon;

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Mouse1) && canDash)
        {
            StartCoroutine(Dash());
        }

        
        if (Input.GetKeyDown(KeyCode.H))
        {
            HealPotion();
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        
        rb.velocity = new Vector2(horizontal * speed, vertical * speed);
        
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(horizontal * dashingPower, vertical * dashingPower);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
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
        foreach (Transform childObj in heartsHUD.transform)
        {
            Destroy(childObj.gameObject);
        }

        Instantiate(heartIcon, heartsHUD.gameObject.transform);
    }
    
    public void Death()
    {
        Debug.Log("YOU DIED");
        Destroy(gameObject);
    }
}