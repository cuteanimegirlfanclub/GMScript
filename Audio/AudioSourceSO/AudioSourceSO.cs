using UnityEngine;

namespace GMEngine
{
    //may be it will be acting like a setting holder/configurator SO, should it incharge of creating AudioSource?
    [CreateAssetMenu(menuName = "Scriptable Object/Audio/Aduio Source")]
    public class AudioSourceSO : ComponentSO
    {
        private AudioSource audioSource;

        public override void AddComponent(GameObject gameObject)
        {
            gameObject.AddComponent<AudioSource>();
        }

        public AudioSource GetAudioSource()
        {
            if (audioSource == null) { audioSource = AudioManager.Instance.CreateAudioSource(this.name); }
            return audioSource;
        }

        //enum or inheritage?
        //public class ReusableAudioSource
        //public class SingleuseAudioSource
    }

}

