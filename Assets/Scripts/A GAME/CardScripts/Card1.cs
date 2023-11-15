using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card1 : MonoBehaviour
{
    public Array Cards;
    private Player Player;
    public Animator animator;
    public Animation anim;
    public ChestScript chest;
    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
        chest = GameObject.FindWithTag("Chest").GetComponent<ChestScript>();
    }
    public void OnMouseDown()
    {
        Cards = GameObject.FindGameObjectsWithTag("Card");
        Player.Vision += 1;
        Player.Card1Picked = true;
        foreach (GameObject obj in Cards)
        {
            Destroy(obj);
        }
    }

    void OnMouseOver()
    {
        animator.Play("Card1");
    }
    void OnMouseExit()
    {
        animator.Play("Card1 1");
    }
}