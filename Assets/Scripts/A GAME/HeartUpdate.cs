using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartUpdate : MonoBehaviour
{
    //Prefabs
    public GameObject fullHeartIcon;
    public GameObject halfHeartIcon;
    public GameObject emptyHeartIcon;
    public void UpdateHearts()
    {
        foreach (Transform childObj in transform)
        {
            Destroy(childObj.gameObject);
        }
        Instantiate(fullHeartIcon, gameObject.transform);
    }
}
