using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{

    public GameObject EnemyObj;
    EnemyBlob enemy;
    public float cooldown = 0.2f;

    void Start()
    {
        EnemyObj = GameObject.FindWithTag("Blob");
        enemy = EnemyObj.GetComponent<EnemyBlob>();
    }
    void Update()
    {
        if (cooldown <= 0)
        {
            Destroy(gameObject);
        }
        cooldown -= Time.deltaTime;
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Blob")
        {
            enemy.TakeDamage();
            Debug.Log(collider);
        }
    }
}
