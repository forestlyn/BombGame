using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MyTool.Music
{
    public class MusicManager : MonoBehaviour
    {
        private static MusicManager instance;

        [SerializeField]
        private GameObject _audioSourcePrefab;
        public static MusicManager Instance
        {
            get => instance;
        }

        private AudioSourcePool _audioSourcePool;
        public MusicList musicList;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                if (_audioSourcePool == null)
                    _audioSourcePool = new AudioSourcePool(_audioSourcePrefab, this.gameObject);
            }
            else
            {
                Destroy(this);
            }
        }

        private void FixedUpdate()
        {
            _audioSourcePool.Update();
        }

        public void PlayEffect(MusicEnum musicType)
        {
            AudioClip clip = musicList.GetClip(musicType);
            if (clip != null)
            {
                var audioSource = _audioSourcePool.GetAudioSource(AudioSourceType.Effect);
                audioSource.clip = clip;
                audioSource.Play();
                _audioSourcePool.AddIntoUsingList(audioSource);
            }
        }

    }
}
