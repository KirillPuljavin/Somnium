using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card1 : MonoBehaviour{
    public Array Cards;
    private Player Player;
    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    void OnMouseDown()
    {
        Cards = GameObject.FindGameObjectsWithTag("Card");
        Player.Vision += 1;
        foreach (GameObject obj in Cards)
        {
            Destroy(obj);
        }
    }
}