using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card4 : MonoBehaviour
{
    public Array Cards;
    private Player Player;
    private HeartUpdate heartsHUD;
    public Animator animator;
    public Animation anim;
    public ChestScript chest;
    public AugmentsVis aug;
    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
        heartsHUD = GameObject.FindWithTag("HeartsUI").GetComponent<HeartUpdate>();
        chest = GameObject.FindWithTag("Chest").GetComponent<ChestScript>();
        aug = GameObject.FindWithTag("AugmentsUI").GetComponent<AugmentsVis>();
    }
    void OnMouseDown()
    {
        Cards = GameObject.FindGameObjectsWithTag("Card");
        Player.MaxHearts += 2;
        Player.Hearts += 2;
        Player.Card4Picked = true;
        heartsHUD.UpdateHearts();
        foreach (GameObject obj in Cards)
        {
            Destroy(obj);
        }
        aug.UpdateAugments();
    }
    void OnMouseOver()
    {
        animator.Play("Card4");
    }
    void OnMouseExit()
    {
        animator.Play("Card4 1");
    }
}