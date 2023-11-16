using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderWeb : MonoBehaviour
{
    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("AddHitbox", 0.5f);
        Invoke("RemoveObj", 5f);
    }

    void AddHitbox()
    {
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
    }
    void RemoveObj()
    {
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player") player.speed /= 2; player.dashingPower /= 1.4f; gameObject.GetComponentInParent<EnemySpider>().speed *= 2;
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player") player.speed *= 2; player.dashingPower *= 1.4f; gameObject.GetComponentInParent<EnemySpider>().speed /= 2;
    }
}
