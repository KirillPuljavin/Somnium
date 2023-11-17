using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public sceneManagerScript sceneManager;
    public Sprite normalSprite;
    public Sprite hoverSprite;

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
        gameObject.GetComponent<SpriteRenderer>().sprite = hoverSprite;
    }
    void OnMouseExit()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = normalSprite;
    }
}
