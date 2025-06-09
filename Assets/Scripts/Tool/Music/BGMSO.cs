using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BGMSO", menuName = "BGMSO", order = 1)]
public class BGMSO : ScriptableObject
{
    public AudioClip beginClip;
    public AudioClip loopClip;
    [HideInInspector] public bool beginClipLoaded = false;
    [HideInInspector] public bool loopClipLoaded = false;
    public SceneEnum musicType;
}
