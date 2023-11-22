using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweepEdge : MonoBehaviour
{
    public int damage;
    public int speed;

    void Start()
    {
        Destroy(gameObject, 0.5f);
    }

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D enemy) // Hit enemies
    {
        if (enemy.gameObject.GetComponent<EnemyBlob>() != null) enemy.gameObject.GetComponent<EnemyBlob>().TakeDamage(damage);
        else if (enemy.gameObject.GetComponent<EnemyFrog>() != null) enemy.gameObject.GetComponent<EnemyFrog>().TakeDamage(damage);
        else if (enemy.gameObject.GetComponent<EnemySpider>() != null) enemy.gameObject.GetComponent<EnemySpider>().TakeDamage(damage);
        else if (enemy.gameObject.GetComponent<BossCat>() != null) enemy.gameObject.GetComponent<BossCat>().TakeDamage(damage);
    }
}
