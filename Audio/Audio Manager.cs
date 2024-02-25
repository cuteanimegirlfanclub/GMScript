using UnityEditor;
using UnityEngine;

namespace GMEngine
{
    [CreateAssetMenu(menuName = "Scriptable Object/Audio/Audio Manager")]
    public class AudioManager : ScriptableObject
    {
        private static string AssetName => nameof(AudioManager);

        private static AudioManager s_instance;

        public static AudioManager Instance
        {
            get
            {
                if (s_instance) return s_instance;
                s_instance = Resources.Load<AudioManager>(AssetName);

                if(s_instance) return s_instance;
                s_instance = CreateInstance<AudioManager>();

#if UNITY_EDITOR
                AssetDatabase.CreateAsset(s_instance, $"Assets/GMEngine/GMResources/Audio/Resources/{AssetName}.asset");
#endif
                return s_instance;
            }
        }

        private static string GetKey(string variableName) => $"{nameof(AudioManager)} - {variableName}";

        [Range(0,1)]
        public float globalVolume;

        public static void Save()
        {
            PlayerPrefs.SetFloat(GetKey(nameof(globalVolume)), Instance.globalVolume);
        }

        public static void Load()
        {
            Instance.globalVolume = PlayerPrefs.GetFloat(GetKey(nameof(globalVolume)));
        }

        public void PlaySimpleSFX(AudioSO audio, AudioSourceSO audioSource)
        {
            audio.Play(audioSource.GetAudioSource()) ;
        }

        public void StopSimpleSFX(IStoppableAudio audio, AudioSourceSO audioSource)
        {
            audio.Stop(audioSource.GetAudioSource());
        }

        //should audio manager incharge of audiosource management/creation?
        public AudioSource CreateAudioSource(string name)
        {
            GameObject AudioSourceGO = new GameObject(name);
            AudioSourceGO.transform.parent = Camera.main.transform;
            AudioSource audioSource = AudioSourceGO.AddComponent<AudioSource>();
            return audioSource;
        }

        public void DestroyAudioSource(AudioSource audioSource)
        {
            if (audioSource != null)
            {

            }
        }
    }

}

