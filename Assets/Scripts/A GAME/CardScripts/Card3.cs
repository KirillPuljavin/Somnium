using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card3 : MonoBehaviour
{
    public Array Cards;
    public GameObject CardBG;
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
        CardBG = GameObject.FindGameObjectWithTag("CardsBG");
        Player.Card3Picked = true;

        Player.damage += 1;
        Player.dashDamage += 2;

        Player.speed = Resources.Load<Player>("Prefabs/Entities/Characters/Player").speed;
        foreach (GameObject obj in Cards)
        {
            Destroy(obj);
        }
        Destroy(CardBG);
        aug.UpdateAugments();
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