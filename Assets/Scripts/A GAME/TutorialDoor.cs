using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TutorialDoor : MonoBehaviour
{
    public GameObject loading;
    public string scenecool;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            loading.SetActive(true);
            SceneManager.LoadScene(scenecool);
        }
    }

}
