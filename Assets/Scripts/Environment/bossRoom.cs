using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class bossRoom : MonoBehaviour
{
    public GameObject door;
    public GameObject block;
    private Tilemap doorRenderer;
    private Tilemap blockRenderer;
    private float fadeSpeed = 0.5f;

    private void Start()
    {
        doorRenderer = door.GetComponent<Tilemap>();
        blockRenderer = block.GetComponent<Tilemap>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            StartCoroutine(FadeOutDoor());
        }
    }

    private IEnumerator FadeOutDoor()
    {
        Color doorColor = doorRenderer.color;
        while (doorColor.a > 0)
        {
            doorColor.a -= fadeSpeed * Time.deltaTime;

            doorRenderer.color = doorColor;
            yield return null;
        }
        door.SetActive(false);
        blockRenderer.GetComponent<TilemapCollider2D>().enabled = true;
    }
}


