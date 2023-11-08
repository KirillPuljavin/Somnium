using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public float cooldown = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown <= 0)
        {
            Destroy(gameObject);
        }
        cooldown -= Time.deltaTime;
    }
}
