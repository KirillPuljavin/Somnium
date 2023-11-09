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
        {
            switch ((player.Hearts))
            {
                case (2):
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(emptyHeartIcon, gameObject.transform);
                    Instantiate(emptyHeartIcon, gameObject.transform);
                    Instantiate(emptyHeartIcon, gameObject.transform);
                    Instantiate(emptyHeartIcon, gameObject.transform);
                    break;
                case (4):
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(emptyHeartIcon, gameObject.transform);
                    Instantiate(emptyHeartIcon, gameObject.transform);
                    Instantiate(emptyHeartIcon, gameObject.transform);
                    break;
                case (6):
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(emptyHeartIcon, gameObject.transform);
                    Instantiate(emptyHeartIcon, gameObject.transform);
                    break;
                case (8):
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(emptyHeartIcon, gameObject.transform);
                    break;
                case (10):
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(fullHeartIcon, gameObject.transform);
                    break;

            }
        }
        else
        {
            switch ((player.Hearts))
            {
                case (1):
                    Instantiate(halfHeartIcon, gameObject.transform);
                    Instantiate(emptyHeartIcon, gameObject.transform);
                    Instantiate(emptyHeartIcon, gameObject.transform);
                    Instantiate(emptyHeartIcon, gameObject.transform);
                    Instantiate(emptyHeartIcon, gameObject.transform);
                    break;
                case (3):
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(halfHeartIcon, gameObject.transform);
                    Instantiate(emptyHeartIcon, gameObject.transform);
                    Instantiate(emptyHeartIcon, gameObject.transform);
                    Instantiate(emptyHeartIcon, gameObject.transform);
                    break;
                case (5):
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(halfHeartIcon, gameObject.transform);
                    Instantiate(emptyHeartIcon, gameObject.transform);
                    Instantiate(emptyHeartIcon, gameObject.transform);
                    break;
                case (7):
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(halfHeartIcon, gameObject.transform);
                    Instantiate(emptyHeartIcon, gameObject.transform);
                    break;
                case (9):
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(fullHeartIcon, gameObject.transform);
                    Instantiate(halfHeartIcon, gameObject.transform);
                    break;
            }
        }
    }
}
