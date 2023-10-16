using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine
{
    public abstract class AudioSO : ScriptableObject
    {
        public abstract void Play(AudioSource source);
    }

    public interface IStoppableAudio
    {
        void Stop(AudioSource source);
    }

    public interface IPausableAudio
    {
        void Pause(AudioSource source);
    }

    public interface ILoopableAudio
    {
        void Play(AudioSource source, bool isLoop);
    }

    public interface IOneShotAudio
    {
        void PlayOneShot(AudioSource source);
    }
}


