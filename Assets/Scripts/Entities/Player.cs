using System;
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
    [SerializeField] private HorizontalLayoutGroup heartsHUD;
    float moveHorizontal;
    float moveVertical;

    public float moveSpeed;
    public float dashCooldown;

    // UI
    [SerializeField] private GameObject heartIcon;

    void Start()
    {

    }

    void Update()
    {

        // Movement
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(moveHorizontal * moveSpeed, moveVertical * moveSpeed);
        dashCooldown -= Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.H))
        {
            HealPotion();
        }
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
