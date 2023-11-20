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
    public GameObject AugmentBG;

    private Player Player;
    private Vector3 cardLocation1;
    private Vector3 cardLocation2;
    private Vector3 cardLocation3;
    private Vector3 BGLocation;
    private bool clickable = false;
    private bool Card1Picked;
    private GameObject Hud;

    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Hud = GameObject.FindWithTag("HUD");
    }

    void Update()
    {
        cardLocation1 = new UnityEngine.Vector3(Player.transform.position.x - 2.5f, Player.transform.position.y + 1, 0);
        cardLocation2 = new UnityEngine.Vector3(Player.transform.position.x, Player.transform.position.y + 1, 0);
        cardLocation3 = new UnityEngine.Vector3(Player.transform.position.x + 2.5f, Player.transform.position.y + 1, 0);
        BGLocation = new UnityEngine.Vector3(Player.transform.position.x, Player.transform.position.y + 0.65f, 0);
        

        Card1Picked = Player.Card1Picked;
        Card2Picked = Player.Card2Picked;
        Card3Picked = Player.Card3Picked;
        Card4Picked = Player.Card4Picked;

        Cards = GameObject.FindGameObjectsWithTag("Card");
        foreach (GameObject obj in Cards)
        {
            obj.gameObject.transform.localScale = new UnityEngine.Vector3(150, 150, 0);
        }
        
        if (Input.GetKeyDown(KeyCode.E) && clickable)
        {
            CardPick();
            Instantiate(AugmentBG, BGLocation, UnityEngine.Quaternion.identity, Hud.transform);
            Debug.Log("BG SUMMONED!");
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
        else switch (Card1Picked, Card2Picked, Card3Picked, Card4Picked)
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
                    StartCoroutine(Player.Alert("You already have 2 augments!"));
                    break;
            }
        Destroy(this);
    }
}
