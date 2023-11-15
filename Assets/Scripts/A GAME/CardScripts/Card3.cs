using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card3 : MonoBehaviour
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
    void OnMouseDown()
    {
        Cards = GameObject.FindGameObjectsWithTag("Card");
        Player.damage += 1;
        Player.dashDamage += 2;
        Player.Card3Picked = true;
        foreach (GameObject obj in Cards)
        {
            Destroy(obj);
        }
    }
    void OnMouseOver()
    {
        animator.Play("Card3");
    }
    void OnMouseExit()
    {
        animator.Play("Card3 1");
    }
}