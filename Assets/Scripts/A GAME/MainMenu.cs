using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public sceneManagerScript sceneManager;

    void Start()
    {
        sceneManager = GameObject.FindWithTag("EventSystem").GetComponent<sceneManagerScript>();
    }

    public void PlayButton()
    {
        Debug.Log("Play");
        sceneManager.SwitchToDungeon1();
    }
    public void ExitButton()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
