using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;
using UnityEngine;

public class NavMeshUpdate : MonoBehaviour
{

    public GameObject LoadingScreen;
    public NavMeshSurface[] surfaces;
    // Start is called before the first frame update
    void Start()
    {
        LoadingScreen.SetActive(true);
        Invoke("updateNav", 0.1f);
        Load();
        LoadingScreen.SetActive(false);
    }

    // Update is called once per frame
    
    void updateNav()
    {
        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }
    }
    IEnumerator Load()
    {
        yield return new WaitForSeconds(1f);
    }
}
