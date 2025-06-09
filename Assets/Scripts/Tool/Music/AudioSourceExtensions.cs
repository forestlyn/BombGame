using MyTools.MyEventSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MyTool.Music
{
    public static class AudioSourceExtensions
    {
        public static IEnumerator OnComplete(this AudioSource audioSource, System.Action callback)
        {
            yield return new WaitWhile(() => audioSource.isPlaying);
            callback?.Invoke();
        }
    }
}
