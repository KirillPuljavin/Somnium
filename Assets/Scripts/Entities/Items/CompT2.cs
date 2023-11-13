using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompT2 : MonoBehaviour
{

    private Player Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider.gameObject.tag);
        if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "dashHitbox")
        {
            Player.ComponentsTier2++;
            Destroy(gameObject);
        }
    }
}
