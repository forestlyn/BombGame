using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BGMSO", menuName = "BGMSO", order = 1)]
public class BGMSO : ScriptableObject
{
    public AudioClip[] clips;
    public SceneEnum musicType;
}
