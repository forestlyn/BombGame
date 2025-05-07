using MyTools.MyEventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MyTool.Music
{
    public class BGMChangeEventArgs : EventArgs
    {
        public SceneEnum BGMType { get; set; }
        public BGMChangeEventArgs(SceneEnum bgmType)
        {
            BGMType = bgmType;
        }
    }

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
        [SerializeField]
        private AudioSource _BGMAudioSource;
        public MyEvent OnBGMChange = MyEvent.CreateEvent((int)EventTypeEnum.BGMChange);

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                if (_audioSourcePool == null)
                    _audioSourcePool = new AudioSourcePool(_audioSourcePrefab, this.gameObject);
                if(_BGMAudioSource == null)
                    _BGMAudioSource = gameObject.AddComponent<AudioSource>();
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            OnBGMChange.AddListener(OnBGMChangeHandler);
            TransitionManager.Instance.OnAfterLoadSceneEvent.AddListener(OnAfterLoadScene);
        }


        private void OnDisable()
        {
            OnBGMChange.RemoveListener(OnBGMChangeHandler);
            TransitionManager.Instance.OnAfterLoadSceneEvent.RemoveListener(OnAfterLoadScene);
        }
        private void OnBGMChangeHandler(object sender, EventArgs e)
        {
            if (e is BGMChangeEventArgs bgmChangeEventArgs)
            {
                PlayBGM(bgmChangeEventArgs.BGMType);
            }
        }

        private void OnAfterLoadScene(object sender, EventArgs e)
        {
            OnBGMChange.Invoke(this, new BGMChangeEventArgs(GameManager.Instance.currentSceneEnum));
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
                //Debug.Log("播放音效" + musicType);
                var audioSource = _audioSourcePool.GetAudioSource(AudioSourceType.Effect);
                audioSource.clip = clip;
                audioSource.Play();
                _audioSourcePool.AddIntoUsingList(audioSource);
            }
        }

        private AudioClip lastAudioClip;
        public void PlayBGM(SceneEnum BGMIdx)
        {
            Debug.Log("播放BGM" + BGMIdx);
            AudioClip audioClip = musicList.GetClip(BGMIdx);
            if (audioClip != null)
            {
                if(lastAudioClip != null && lastAudioClip == audioClip)
                {
                    return;
                }
                //Debug.Log("播放BGM" + BGMIdx);
                _BGMAudioSource.clip = audioClip;
                _BGMAudioSource.loop = true;
                _BGMAudioSource.Play();
                lastAudioClip = audioClip;
            }
            else
            {
                _BGMAudioSource.Stop();
            }
        }
    }
}
