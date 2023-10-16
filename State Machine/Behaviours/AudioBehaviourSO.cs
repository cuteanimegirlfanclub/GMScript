using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GMEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Behaviours/Audio Behaviour")]
public class AudioBehaviourSO : BehaviourSO
{
    public MultipleSFXSimpleSO AudioSFXSO;
    public AudioSourceSO audioSource;

    public Action action = Action.Play;
    public enum Action
    {
        Play,
        Stop
    }

    public override void Execute(StateMachineController controller)
    {
        if(action == Action.Play)
        {
            PlayAudio();
        }
        else if(action == Action.Stop)
        {
            StopAudio();
        }
    }

    void PlayAudio()
    {
        AudioManager.Instance.PlaySimpleSFX(AudioSFXSO, audioSource);
    }

    void StopAudio()
    {
        AudioManager.Instance.StopSimpleSFX(AudioSFXSO, audioSource);
    }
}
