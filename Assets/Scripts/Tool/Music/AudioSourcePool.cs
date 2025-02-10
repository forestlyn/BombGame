using MyTool.ObjectPool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MyTool.Music
{
    public enum AudioSourceType
    {
        Effect,
    }
    public class AudioSourcePool : MyObjectPool<AudioSourceType>
    {

        public AudioSourcePool(GameObject prefab, GameObject parent = null) : base(new Dictionary<AudioSourceType, GameObject> { { AudioSourceType.Effect, prefab } }, parent)
        {

        }

        private List<AudioSource> audioSourcesIsUsing = new List<AudioSource>();


        public void Update()
        {
            for (int i = 0; i < audioSourcesIsUsing.Count; i++)
            {
                if (!audioSourcesIsUsing[i].isPlaying)
                {
                    ReturnAudioSource(AudioSourceType.Effect, audioSourcesIsUsing[i]);
                    audioSourcesIsUsing.RemoveAt(i);
                    i--;
                }
            }
        }

        public AudioSource GetAudioSource(AudioSourceType key)
        {
            return GetObject(key).GetComponent<AudioSource>();
        }

        public void AddIntoUsingList(AudioSource audioSource)
        {
            audioSourcesIsUsing.Add(audioSource);
        }

        public void ReturnAudioSource(AudioSourceType key, AudioSource obj)
        {
            ReturnObject(key, obj.gameObject);
        }
    }
}
