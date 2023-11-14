using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    public Animator animator;
    private bool isTriggered = false;
    private Vector3 cardLocation1;
    private Vector3 cardLocation2;
    private Vector3 cardLocation3;
    public GameObject card1;
    public GameObject card2;
    public GameObject card3;
    public GameObject card4;
    public Array Cards;


    // Start is called before the first frame update
    void Start()
    {
        cardLocation1 = new Vector3(-10, 0, 0);
        cardLocation2 = new Vector3(-5, 0, 0);
        cardLocation3 = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Cards = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject obj in Cards)
        {
            obj.gameObject.transform.localScale = new Vector3(150, 150, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "dashHitbox" && isTriggered == false)
        {
            animator.Play("ChestAnim");
            isTriggered = true;
            int r = UnityEngine.Random.Range(1, 5);
            switch (r)
            {
                case (1):
                    Instantiate(card1, cardLocation1, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    Instantiate(card2, cardLocation2, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    Instantiate(card3, cardLocation3, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    break;
                case (2):
                    Instantiate(card2, cardLocation1, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    Instantiate(card3, cardLocation2, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    Instantiate(card4, cardLocation3, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    break;
                case (3):
                    Instantiate(card1, cardLocation1, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    Instantiate(card2, cardLocation2, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    Instantiate(card4, cardLocation3, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    break;
                case (4):
                    Instantiate(card1, cardLocation1, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    Instantiate(card3, cardLocation2, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    Instantiate(card4, cardLocation3, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    break;
            }
        }
    }
}
