using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogFly : MonoBehaviour
{
    Player Player;
    public int speed;
    public int damageHearts;

    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
        StartCoroutine(Kill());
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector2(Player.gameObject.transform.position.x, Player.gameObject.transform.position.y), speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player.TakeDamage(damageHearts);
            Destroy(gameObject);
        }
    }

    public void Hit() => Destroy(gameObject);
    private IEnumerator Kill()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
