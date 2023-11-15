using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card1 : MonoBehaviour{
    public Array Cards;
    private Player Player;
    public Animator animator;
    public Animation anim;
    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    public void OnMouseDown()
    {
        Cards = GameObject.FindGameObjectsWithTag("Card");
        Player.Vision += 1;
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