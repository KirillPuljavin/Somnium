using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartUpdate : MonoBehaviour
{
    //Prefabs
    public GameObject fullHeartIcon;
    public GameObject halfHeartIcon;
    public GameObject emptyHeartIcon;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform childObj in transform)
        {
            Destroy(childObj.gameObject);
        }
        Instantiate(fullHeartIcon, gameObject.transform);
    }

    // Update is called once per frame
    public void UpdateHearts()
    {
        
    }
}
