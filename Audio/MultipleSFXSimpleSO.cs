using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine
{
    [CreateAssetMenu(fileName = "Simple Multiple SFX", menuName = "Scriptable Object/Audio/Simple Multiple SFX")]
    public class MultipleSFXSimpleSO : AudioSO, ILoopableAudio, IStoppableAudio
    {
        public AudioClip[] audioClips;

        [Range(0f, 1f)]
        public float m_SFXVolume;
        public override void Play(AudioSource source)
        {
            if (audioClips.Length == 0) { return; }
            source.clip = audioClips[Random.Range(0, audioClips.Length)];
            source.volume = m_SFXVolume;
            source.Play();
        }
        public void Play(AudioSource source, bool isLoop)
        {
            if (audioClips.Length == 0) { return; }
            source.clip = audioClips[Random.Range(0, audioClips.Length)];
            source.volume = m_SFXVolume;
            source.loop = isLoop;
            source.Play();
        }

        public void Stop(AudioSource source)
        {
            source.Stop();
        }
    }
}