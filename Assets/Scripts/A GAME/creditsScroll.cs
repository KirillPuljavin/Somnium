using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class creditsScroll : MonoBehaviour
{
    float x = 0;
    void Update()
    {
        transform.parent.parent.GetChild(1).gameObject.SetActive(false);
        if (x < 20)
        {
            transform.Translate(Vector3.up * Time.deltaTime * 1.5f);
            x += Time.deltaTime;
        }
        else
        {
            SceneManager.LoadScene("Main Meny");
        }
    }
}
