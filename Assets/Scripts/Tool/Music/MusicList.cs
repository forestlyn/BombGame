using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MyTool.Music
{

    public class MusicList : MonoBehaviour
    {
        public MusicSO[] musicSOs;
        public BGMSO[] bgmSOs;

        public IEnumerator AsyncPreloadBGMProgressive()
        {
            float count = 0;
            foreach (var item in bgmSOs)
            {
                if (item.beginClip != null && !item.beginClipLoaded)
                {
                    count++;
                }
                if (item.loopClip != null && !item.loopClipLoaded)
                {
                    count++;
                }
            }
            if (count == 0)
            {
                Debug.LogWarning("没有需要预加载的BGM");
                LoadResourcesManager.Instance.LoadProgress = 1f;
                yield break;
            }
            float percentPerClip = 1f / count;
            foreach (var item in bgmSOs)
            {
                //Debug.Log($"开始预加载BGM {item.musicType}");
                //Debug.Log($"{item.beginClip != null} {item.beginClipLoaded}");
                if (item.beginClip != null && !item.beginClipLoaded)
                {
                    //Debug.Log($"开始预加载BGM {item.musicType} 的 beginClip");
                    // 分帧加载beginClip
                    var beginRequest = item.beginClip.LoadAudioData();
                    while (!beginRequest)
                    {
                        yield return null; // 每帧检查一次
                    }
                    item.beginClipLoaded = true;
                    LoadResourcesManager.Instance.LoadProgress += percentPerClip;
                }

                // 下一帧再加载loopClip
                yield return null;

                if (item.loopClip != null && !item.loopClipLoaded)
                {
                    //Debug.Log($"开始预加载BGM {item.musicType} 的 loopClip");
                    var loopRequest = item.loopClip.LoadAudioData();
                    while (!loopRequest)
                    {
                        yield return null;
                    }
                    item.loopClipLoaded = true;
                    LoadResourcesManager.Instance.LoadProgress += percentPerClip;
                    yield return null;
                }
            }
            if (LoadResourcesManager.Instance.LoadProgress <= 1f)
                LoadResourcesManager.Instance.LoadProgress += 1f;
        }
        public void PreloadBGM()
        {
            foreach (var item in bgmSOs)
            {
                item.beginClipLoaded = false;
                item.loopClipLoaded = false;
            }
            StartCoroutine(AsyncPreloadBGMProgressive());
        }

        public AudioClip GetClip(MusicEnum musicType)
        {
            foreach (var item in musicSOs)
            {
                if (item.musicType == musicType)
                {
                    return item.clips[UnityEngine.Random.Range(0, item.clips.Length)];
                }
            }
            Debug.LogError("音效" + musicType + "不存在");
            return null;
        }

        public AudioClip GetClip(SceneEnum musicType, bool is_first = false)
        {
            foreach (var item in bgmSOs)
            {
                if (item.musicType == musicType)
                {
                    if (is_first && item.beginClipLoaded)
                    {
                        return item.beginClip;
                    }
                    else if (item.loopClipLoaded)
                    {
                        return item.loopClip;
                    }
                    else
                    {
                        Debug.LogWarning("BGM " + musicType + " 的音频片段未加载或为空");
                        return null;
                    }
                }
            }
            Debug.LogError("BMG" + musicType + "不存在");
            return null;
        }
    }
}