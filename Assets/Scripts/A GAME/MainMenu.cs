using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public sceneManagerScript sceneManager;
    // Start is called before the first frame update
    void Start()
    {
        sceneManager = GameObject.FindWithTag("EventSystem").GetComponent<sceneManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnMouseDown()
    {
        sceneManager.SwitchToDungeon1();
    }

    void OnMouseOver()
    {
        
    }
    void OnMouseExit()
    {
        
    }
}
