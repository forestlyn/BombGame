using MyTool.Music;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicSO", menuName = "MusicSO")]
public class MusicSO : ScriptableObject
{
    //��Ƶ����
    public MusicEnum musicType;

    //��ƵƬ��
    public AudioClip[] clips;

    //���ż��
    public float playDelta = 0.05f;
}
