using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ChestScript : MonoBehaviour
{
    public Animator animator;
    public GameObject card1;
    public GameObject card2;
    public GameObject card3;
    public GameObject card4;
    public Array Cards;
    public bool Card4Picked;
    public bool Card2Picked;
    public bool Card3Picked;

    private Player Player;
    private Vector3 cardLocation1;
    private Vector3 cardLocation2;
    private Vector3 cardLocation3;
    private bool clickable = false;
    private bool Card1Picked;

    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        cardLocation1 = new Vector3(Player.transform.position.x - 5, Player.transform.position.y + 1, 0);
        cardLocation2 = new Vector3(Player.transform.position.x, Player.transform.position.y + 1, 0);
        cardLocation3 = new Vector3(Player.transform.position.x + 5, Player.transform.position.y + 1, 0);

        Card1Picked = Player.Card1Picked;
        Card2Picked = Player.Card2Picked;
        Card3Picked = Player.Card3Picked;
        Card4Picked = Player.Card4Picked;

        Cards = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject obj in Cards)
        {
            obj.gameObject.transform.localScale = new Vector3(150, 150, 0);
        }
        if (Input.GetKeyDown(KeyCode.E) && clickable && RoomManager.roomCleared)
        {
            CardPick();
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

    void CardPick()
    {
        Transform HUD = GameObject.FindGameObjectWithTag("HUD").transform;
        animator.Play("ChestAnim");
        Player.speed = 0;

        if (!Card1Picked && !Card2Picked && !Card3Picked && !Card4Picked)
        {
            int r = UnityEngine.Random.Range(1, 5);
            switch (r)
            {
                case (1):
                    Instantiate(card1, cardLocation1, Quaternion.identity, HUD);
                    Instantiate(card2, cardLocation2, Quaternion.identity, HUD);
                    Instantiate(card3, cardLocation3, Quaternion.identity, HUD);
                    break;
                case (2):
                    Instantiate(card2, cardLocation1, Quaternion.identity, HUD);
                    Instantiate(card3, cardLocation2, Quaternion.identity, HUD);
                    Instantiate(card4, cardLocation3, Quaternion.identity, HUD);
                    break;
                case (3):
                    Instantiate(card1, cardLocation1, Quaternion.identity, HUD);
                    Instantiate(card2, cardLocation2, Quaternion.identity, HUD);
                    Instantiate(card4, cardLocation3, Quaternion.identity, HUD);
                    break;
                case (4):
                    Instantiate(card1, cardLocation1, Quaternion.identity, HUD);
                    Instantiate(card3, cardLocation2, Quaternion.identity, HUD);
                    Instantiate(card4, cardLocation3, Quaternion.identity, HUD);
                    break;
            }
        }
        switch (Card1Picked, Card2Picked, Card3Picked, Card4Picked)
        {
            case (true, false, false, false):
                Instantiate(card2, cardLocation1, Quaternion.identity, HUD);
                Instantiate(card3, cardLocation2, Quaternion.identity, HUD);
                Instantiate(card4, cardLocation3, Quaternion.identity, HUD);
                break;
            case (false, true, false, false):
                Instantiate(card1, cardLocation1, Quaternion.identity, HUD);
                Instantiate(card3, cardLocation2, Quaternion.identity, HUD);
                Instantiate(card4, cardLocation3, Quaternion.identity, HUD);
                break;
            case (false, false, true, false):
                Instantiate(card1, cardLocation1, Quaternion.identity, HUD);
                Instantiate(card2, cardLocation2, Quaternion.identity, HUD);
                Instantiate(card4, cardLocation3, Quaternion.identity, HUD);
                break;
            case (false, false, false, true):
                Instantiate(card1, cardLocation1, Quaternion.identity, HUD);
                Instantiate(card2, cardLocation2, Quaternion.identity, HUD);
                Instantiate(card3, cardLocation3, Quaternion.identity, HUD);
                break;
            default:
                Debug.Log("You already have 2 augments! Can't procced!");
                break;
        }
        Destroy(this);
    }
}
