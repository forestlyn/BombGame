using System.Collections;
using UnityEngine;

namespace MyTool.Music
{
    public static class AudioSourceExtensions
    {
        private const float ClipDelta = 0.01f; // A small value to check if the audio is almost finished
        public static IEnumerator OnComplete(this AudioSource audioSource, System.Action callback)
        {
            float clipLength = audioSource.clip.length;

            while (audioSource.IsPlaying())
            {
                yield return null;
            }

            if (audioSource.time >= clipLength - ClipDelta)
            {
                callback?.Invoke();
            }
        }

        public static bool IsPlaying(this AudioSource audioSource)
        {
            float clipLength = audioSource.clip.length;
            return audioSource != null && (audioSource.isPlaying && audioSource.time < clipLength - ClipDelta);
        }
    }
}
