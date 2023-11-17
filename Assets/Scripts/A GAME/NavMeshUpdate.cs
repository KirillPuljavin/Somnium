using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;
using UnityEngine;

public class NavMeshUpdate : MonoBehaviour
{
    public NavMeshSurface[] surfaces;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("updateNav", 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void updateNav()
    {
        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }
    }
}
