using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTool.Music
{

    public class MusicList : MonoBehaviour
    {
        public MusicSO[] musicSOs;

        public AudioClip GetClip(MusicEnum musicType)
        {
            foreach (var item in musicSOs)
            {
                if (item.musicType == musicType)
                {
                    return item.clips[Random.Range(0, item.clips.Length)];
                }
            }
            return null;
        }
    }
}