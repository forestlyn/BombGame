using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTool.Music
{

    public class MusicList : MonoBehaviour
    {
        public MusicSO[] musicSOs;
        public BGMSO[] bgmSOs;
        public AudioClip GetClip(MusicEnum musicType)
        {
            foreach (var item in musicSOs)
            {
                if (item.musicType == musicType)
                {
                    return item.clips[Random.Range(0, item.clips.Length)];
                }
            }
            Debug.LogError("音效" + musicType + "不存在");
            return null;
        }

        public AudioClip GetClip(SceneEnum musicType)
        {
            foreach (var item in bgmSOs)
            {
                if (item.musicType == musicType)
                {
                    if(item.clips.Length == 0)
                    {
                        return null;
                    }
                    return item.clips[Random.Range(0, item.clips.Length)];
                }
            }
            Debug.LogError("BMG" + musicType + "不存在");
            return null;
        }
    }
}