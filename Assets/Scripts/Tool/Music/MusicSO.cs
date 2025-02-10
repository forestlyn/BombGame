using MyTool.Music;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicSO", menuName = "MusicSO")]
public class MusicSO : ScriptableObject
{
    //“Ù∆µ¿‡–Õ
    public MusicEnum musicType;

    //“Ù∆µ∆¨∂Œ
    public AudioClip[] clips;

    //≤•∑≈º‰∏Ù
    public float playDelta = 0.05f;
}
