using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenController : MonoBehaviour
{

    private float progressForBar;
    private Slider loadingBar;

    // Start is called before the first frame update
    void Start()
    {
        loadingBar = GetComponent<Slider>();
        progressForBar = 0;
        StartCoroutine(load());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator load()
    {
        AsyncOperation loadingScene = SceneManager.LoadSceneAsync(2);
        while (!loadingScene.isDone)
        {
            progressForBar = Mathf.Clamp01(loadingScene.progress/0.9f);
            loadingBar.value = progressForBar;
            yield return null;
        }
    }
}
