using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChestScript : MonoBehaviour
{

    public Animator animator;
    private Vector3 cardLocation1;
    private Vector3 cardLocation2;
    private Vector3 cardLocation3;
    public GameObject card1;
    public GameObject card2;
    public GameObject card3;
    public GameObject card4;
    public Array Cards;
    private bool clickable = false;
    private Player Player;
    private bool Card1Picked;
    public bool Card2Picked;
    public bool Card3Picked;
    public bool Card4Picked;


    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        cardLocation1 = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x - 5, GameObject.FindGameObjectWithTag("Player").transform.position.y + 1, 0);
        cardLocation2 = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x, GameObject.FindGameObjectWithTag("Player").transform.position.y + 1, 0);
        cardLocation3 = new Vector3(GameObject.FindGameObjectWithTag("Player").transform.position.x + 5, GameObject.FindGameObjectWithTag("Player").transform.position.y + 1, 0);

        Card1Picked = Player.Card1Picked;
        Card2Picked = Player.Card2Picked;
        Card3Picked = Player.Card3Picked;
        Card4Picked = Player.Card4Picked;

        Cards = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject obj in Cards)
        {
            obj.gameObject.transform.localScale = new Vector3(150, 150, 0);
        }
        if (clickable && Input.GetKeyDown(KeyCode.E))
        {
            clickable = false;
            animator.Play("ChestAnim");
            if (!Card1Picked && !Card2Picked && !Card3Picked && !Card4Picked)
            {
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
            switch (Card1Picked, Card2Picked, Card3Picked, Card4Picked)
            {
                case (true, false, false, false):
                    Instantiate(card2, cardLocation1, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    Instantiate(card3, cardLocation2, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    Instantiate(card4, cardLocation3, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    break;
                case (false, true, false, false):
                    Instantiate(card1, cardLocation1, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    Instantiate(card3, cardLocation2, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    Instantiate(card4, cardLocation3, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    break;
                case (false, false, true, false):
                    Instantiate(card1, cardLocation1, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    Instantiate(card2, cardLocation2, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    Instantiate(card4, cardLocation3, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    break;
                case (false, false, false, true):
                    Instantiate(card1, cardLocation1, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    Instantiate(card2, cardLocation2, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    Instantiate(card3, cardLocation3, Quaternion.identity, GameObject.FindGameObjectWithTag("HUD").transform);
                    break;
                default:
                    Debug.Log("You already have 2 augments! Can't procced!");
                    break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "dashHitbox")
        {
            clickable = true;
        }
    }
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "dashHitbox")
        {
            clickable = false;
        }
    }
}
