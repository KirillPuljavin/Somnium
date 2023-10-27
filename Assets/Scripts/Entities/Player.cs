using System.Collections;
using System.Collections.Generic;
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

    // Movement
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Text heartsHUD;
    float moveHorizontal;
    float moveVertical;

    public int moveSpeed;
    public int dashCooldown;

    void Start()
    {

    }

    void Update()
    {
        // Movement
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(moveHorizontal * moveSpeed, moveVertical * moveSpeed);

        if (Input.GetKeyDown(KeyCode.H))
        {
            HealPotion();
        }
    }

    public void HealPotion()
    {
        Hearts += 0.5f;
        heartsHUD.text = "Hearts: " + Hearts;
    }
    public void TakeDamage(float amount)
    {
        Hearts -= amount;
        heartsHUD.text = "Hearts: " + Hearts;

        if (Hearts <= 0) Death();
    }
    public void Death()
    {
        Debug.Log("YOU DIED");
        Destroy(gameObject);
    }
}
