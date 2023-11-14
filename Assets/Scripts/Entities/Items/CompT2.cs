using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompT2 : MonoBehaviour
{

    private Player player;
    [SerializeField] private UIScript uiScript;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.ComponentsTier2++;
            uiScript.UpdateUI();
            Destroy(gameObject);
        }
    }
}
