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
    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    void OnMouseDown()
    {
        Cards = GameObject.FindGameObjectsWithTag("Card");
        Player.speed += 1;
        Player.dashingPower += 4;
        foreach (GameObject obj in Cards)
        {
            Destroy(obj);
        }
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