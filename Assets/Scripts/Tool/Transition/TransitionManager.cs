using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public bool isLoading = false;
    private static TransitionManager instance;
    [SerializeField]
    private CanvasGroup fadeCanvasGroup;
    private bool isFade;
    [SerializeField]
    private float fadeDuration = 0.1f;

    public static TransitionManager Instance
    {
        get => instance;
    }

    public event UnityAction OnStartLoadSceneEvent;
    public event UnityAction OnAfterLoadSceneEvent;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void Transition(string from, string to)
    {
        if (!isLoading)
        {
            StartCoroutine(TransitionToScene(from, to));
        }
    }

    private IEnumerator TransitionToScene(string from, string to)
    {
        isLoading = true;
        yield return Fade(1);
        if (from != string.Empty)
        {
            OnStartLoadSceneEvent?.Invoke();
            yield return SceneManager.UnloadSceneAsync(from);
        }
        yield return SceneManager.LoadSceneAsync(to, LoadSceneMode.Additive);
        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newScene);
        OnAfterLoadSceneEvent?.Invoke();
        yield return Fade(0);
        isLoading = false;
    }

    private IEnumerator Fade(float targetAlpha)
    {
        isFade = true;
        fadeCanvasGroup.blocksRaycasts = true;
        float speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / fadeDuration;
        while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
        {
            fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
            yield return null;
        }

        fadeCanvasGroup.blocksRaycasts = false;
        isFade = false;
    }
}
