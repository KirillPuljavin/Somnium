using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card4 : MonoBehaviour{
    public Array Cards;
    private Player Player;
    private HeartUpdate heartsHUD;
    void Start()
    {
        Player = GameObject.FindWithTag("Player").GetComponent<Player>();
        heartsHUD = GameObject.FindWithTag("HeartsUI").GetComponent<HeartUpdate>();
    }
    void OnMouseDown()
    {
        Cards = GameObject.FindGameObjectsWithTag("Card");
        Player.MaxHearts += 2;
        Player.Hearts += 2;
        heartsHUD.UpdateHearts();
        foreach (GameObject obj in Cards)
        {
            Destroy(obj);
        }
    }
}