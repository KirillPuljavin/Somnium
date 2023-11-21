using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject LoadingScreen;
    private float loadCooldown = 0;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Dungeon 1")
        {
            
            LoadingScreen.SetActive(true);
        }
    }
    void Update()
    {
        // if (loadCooldown < 20f) loadCooldown += Time.deltaTime;
        // if (loadCooldown >= 20f) LoadingScreen.SetActive(false);

    }

    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneId);


        while (!asyncOperation.isDone)
        {
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            LoadingScreen.SetActive(true);

            yield return null;
        }
    }
}
