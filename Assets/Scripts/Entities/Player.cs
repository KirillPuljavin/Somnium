using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Info
    public int Hearts;
    public int Stamina;
    public int ComponentsTier2;
    public int ComponentsTier3;

    // Movement
    [SerializeField] private Rigidbody2D rb;
    float moveHorizontal;
    float moveVertical;
    public int moveSpeed;
    public int dashCooldown;

    void Start()
    {

    }

    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveHorizontal * moveSpeed, rb.velocity.y);
    }
}
