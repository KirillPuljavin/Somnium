using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class HeartUpdate : MonoBehaviour
{
    private GameObject PlayerObj;
    Player player;

    //Prefabs
    public GameObject fullHeartIcon;
    public GameObject halfHeartIcon;
    public GameObject emptyHeartIcon;
    private int playerHearts;
    private int tempAmt = 0;
    void Start()
    {
        PlayerObj = GameObject.FindWithTag("Player");
        player = PlayerObj.GetComponent<Player>();
        

        UpdateHearts();
    }
    public void UpdateHearts()
    {
        playerHearts = player.Hearts;
        tempAmt = 0;
        foreach (Transform childObj in transform)
        {
            Destroy(childObj.gameObject);
        }
        for (int i = 0; i < player.MaxHearts; i += 2)
        {
            tempAmt += 2;
            if (tempAmt <= playerHearts)
            {
                Instantiate(fullHeartIcon, gameObject.transform.position, Quaternion.identity, gameObject.transform);
                Debug.Log("Full Heart Added");
            }
            else if (tempAmt == playerHearts + 1)
            {
                Instantiate(halfHeartIcon, gameObject.transform.position, Quaternion.identity, gameObject.transform);
                Debug.Log("Half Heart Added");
            }
            else
            {
                Instantiate(emptyHeartIcon, gameObject.transform.position, Quaternion.identity, gameObject.transform);
                Debug.Log("Empty Heart Added");
            }
        }
    }
}
