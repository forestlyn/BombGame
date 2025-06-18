using MyTools.MyEventSystem;
using System;
using UnityEngine;

namespace MyTool.Music
{
    public class BGMChangeEventArgs : EventArgs
    {
        public SceneEnum BGMType { get; set; }
        public bool IsFirst { get; set; }
        public BGMChangeEventArgs(SceneEnum bgmType, bool is_first = false)
        {
            BGMType = bgmType;
            IsFirst = is_first;
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

                if (musicList != null)
                {
                    musicList.PreloadBGM();
                }
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
            _BGMAudioSource.ignoreListenerPause = true; // 忽略暂停事件
        }


        private void OnDisable()
        {
            OnBGMChange.RemoveListener(OnBGMChangeHandler);
            TransitionManager.Instance.OnAfterLoadSceneEvent.RemoveListener(OnAfterLoadScene);
        }
        private void OnBGMChangeHandler(object sender, EventArgs e)
        {
            Debug.Log("BGMChangeHandler");
            if (e is BGMChangeEventArgs bgmChangeEventArgs)
            {
                PlayBGM(bgmChangeEventArgs.BGMType, bgmChangeEventArgs.IsFirst);
            }
        }

        private void OnAfterLoadScene(object sender, EventArgs e)
        {
            OnBGMChange.Invoke(this, new BGMChangeEventArgs(GameManager.Instance.currentSceneEnum, true));
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
        public void PlayBGM(SceneEnum BGMIdx,bool is_first = false)
        {
            Debug.Log("播放BGM" + BGMIdx);
            AudioClip audioClip = musicList.GetClip(BGMIdx, is_first);
            if (audioClip != null)
            {
                if(lastAudioClip != null && (lastAudioClip == audioClip && _BGMAudioSource.IsPlaying()))
                {
                    return;
                }
                //Debug.Log("播放BGM" + BGMIdx);
                _BGMAudioSource.clip = audioClip;
                _BGMAudioSource.loop = false;
                _BGMAudioSource.Play();
                StartCoroutine(_BGMAudioSource.OnComplete(() =>
                {
                    OnBGMChange.Invoke(this, new BGMChangeEventArgs(GameManager.Instance.currentSceneEnum, false));
                }));
                lastAudioClip = audioClip;
            }
            else
            {
                lastAudioClip = null;
                _BGMAudioSource.Stop();
            }
        }
    }
}
