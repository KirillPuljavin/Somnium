using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartUpdate : MonoBehaviour
{
    //gameObjects
    public GameObject heartsHUD;
    
    //Prefabs
    public Prefab

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform childObj in heartsHUD.transform)
        {
            Destroy(childObj.gameObject);
        }
        Instantiate(heartIcon, heartsHUD.gameObject.transform);
    }

    // Update is called once per frame
    public void UpdateHearts()
    {
        
    }
}
