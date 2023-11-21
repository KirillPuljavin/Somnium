using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

public class AugmentsVis : MonoBehaviour
{
    public GameObject Augment1;
    public GameObject Augment2;
    public GameObject Augment3;
    public GameObject Augment4;

    private Player player;
    private UnityEngine.Vector3 firstPos;
    private UnityEngine.Vector3 secondPos;
    private bool firstAugmentPicked = false;
    private bool card1sel = false;
    private bool card2sel = false;
    private bool card3sel = false;
    private bool card4sel = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        firstPos = new UnityEngine.Vector3(0, 0, 0);
        secondPos = new UnityEngine.Vector3(0, 0, 0);
    }

    public void UpdateAugments()
    {
        if (!firstAugmentPicked)
        {
    
            if (player.Card1Picked)
            {
                Instantiate(Augment1, firstPos, UnityEngine.Quaternion.identity, gameObject.transform);
                Debug.Log("Augment 1 is picked");
                firstAugmentPicked = true;
                card1sel = true;
            }
            if (player.Card2Picked)
            {
                Instantiate(Augment2, firstPos, UnityEngine.Quaternion.identity, gameObject.transform);
                Debug.Log("Augment 2 is picked");
                firstAugmentPicked = true;
                card2sel = true;
            }
            if (player.Card3Picked)
            {
                Instantiate(Augment3, firstPos, UnityEngine.Quaternion.identity, gameObject.transform);
                Debug.Log("Augment 3 is picked");
                firstAugmentPicked = true;
                card3sel = true;
            }
            if (player.Card4Picked)
            {
                Instantiate(Augment4, firstPos, UnityEngine.Quaternion.identity, gameObject.transform);
                Debug.Log("Augment 4 is picked");
                firstAugmentPicked = true;
                card4sel = true;
            }
        }
        else
        {
            if (player.Card1Picked && !card1sel)
            {
                Instantiate(Augment1, secondPos, UnityEngine.Quaternion.identity, gameObject.transform);
                Debug.Log("Augment 1 is picked");
                firstAugmentPicked = true;
                card1sel = true;
            }
            else if (player.Card2Picked && !card2sel)
            {
                Instantiate(Augment2, secondPos, UnityEngine.Quaternion.identity, gameObject.transform);
                Debug.Log("Augment 1 is picked");
                firstAugmentPicked = true;
                card1sel = true;
            }
            else if (player.Card3Picked && !card3sel)
            {
                Instantiate(Augment3, secondPos, UnityEngine.Quaternion.identity, gameObject.transform);
                Debug.Log("Augment 1 is picked");
                firstAugmentPicked = true;
                card1sel = true;
            }
            else if (player.Card4Picked && !card4sel)
            {
                Instantiate(Augment4, secondPos, UnityEngine.Quaternion.identity, gameObject.transform);
                Debug.Log("Augment 1 is picked");
                firstAugmentPicked = true;
                card1sel = true;
            }
            else Debug.Log("Error!");
        }
    }
}
