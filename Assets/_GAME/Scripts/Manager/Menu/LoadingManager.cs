using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private Slider loadingSlider;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Tutorial"))
            StartCoroutine(loadingScene("Tutorial"));
        else
            StartCoroutine(loadingScene("Menu"));
    }
    IEnumerator loadingScene(string arenaNumber)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(arenaNumber);


        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 10f);
            loadingSlider.value = progress;
            yield return null;

        }

    }
}
