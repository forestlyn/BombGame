using DG.Tweening;
using MyTools.MyEventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadResourcesManager : MonoBehaviour
{
    public Animator animator;
    public GameObject animGO;
    public float smoothTime = 0.5f; // 平滑过渡时间
    public static LoadResourcesManager Instance { get; private set; }

    public MyEvent LoadedResourcesEvent = MyEvent.CreateEvent((int)EventTypeEnum.LoadResources);
    private float loadProgress = 0f;
    public float LoadProgress
    {
        get => loadProgress;
        set
        {
            DOTween.Kill(this); // 取消之前的动画
            float newValue = Mathf.Clamp01(value);
            DOTween.To(() => loadProgress, x => loadProgress = x, newValue, smoothTime)
                   .SetId(this)
                   .OnUpdate(() => {
                       SetLoadPercent(loadProgress);
                       if (loadProgress >= 0.999f)
                       {
                           LoadedResourcesEvent.Invoke(this, EventArgs.Empty);
                           animGO.SetActive(false);
                       }
                       else
                       {
                           animGO.SetActive(true);
                       }
                   });
        }
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }



    public void SetLoadPercent(float percent)
    {
        if (animator != null)
        {
            animator.Play("Load", 0, percent);
        }
    }

}
