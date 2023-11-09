using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartUpdate : MonoBehaviour
{
    private GameObject PlayerObj;
    Player player;

    //Prefabs
    public GameObject fullHeartIcon;
    public GameObject halfHeartIcon;
    public GameObject emptyHeartIcon;
    void Start()
    {
        PlayerObj = GameObject.FindWithTag("Player");
        player = PlayerObj.GetComponent<Player>();
    }
    public void UpdateHearts()
    {
        foreach (Transform childObj in transform)
        {
            Destroy(childObj.gameObject);
        }
        if (player.Hearts % 2 == 0)
            Instantiate(fullHeartIcon, gameObject.transform);
    }
}
