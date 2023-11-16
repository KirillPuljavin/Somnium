using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card2 : MonoBehaviour
{
    public Array Cards;
    private Player Player;
    public Animator animator;
    public Animation anim;
    public ChestScript chest;
    public AugmentsVis aug;
    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
        chest = GameObject.FindWithTag("Chest").GetComponent<ChestScript>();
        aug = GameObject.FindWithTag("AugmentsUI").GetComponent<AugmentsVis>();

    }
    void OnMouseDown()
    {
        Cards = GameObject.FindGameObjectsWithTag("Card");
        Player.speed += 1;
        Player.dashingPower += 4;
        Player.Card2Picked = true;
        foreach (GameObject obj in Cards)
        {
            Destroy(obj);
        }
        aug.UpdateAugments();
    }
    void OnMouseOver()
    {
        animator.Play("Card2");
    }
    void OnMouseExit()
    {
        animator.Play("Card2 1");
    }
}