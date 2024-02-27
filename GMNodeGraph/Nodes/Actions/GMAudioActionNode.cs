using GMEngine.GMAddressables;
using GMEngine.Value;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.GMNodes
{
    public class GMAudioActionNode : GMActionNode
    {
        [SerializeField] private MultipleSFXSimpleSO AudioSFXSO;
        [SerializeField] private StringReferenceRO audioSourceAddress;
        [SerializeField] private BooleanReferenceRO key;
        
        public void Execute()
        {
            if (key.Value)
            {
                PlayAudio();
            }
            else
            {
                StopAudio();
            }
        }

        void PlayAudio()
        {
            var audioSource = AddressablesManager.GetMember(audioSourceAddress.Value).GetComponent<AudioSource>();
            AudioSFXSO.Play(audioSource, true);
        }

        void StopAudio()
        {
            var audioSource = AddressablesManager.GetMember(audioSourceAddress.Value).GetComponent<AudioSource>();
            AudioSFXSO.Stop(audioSource);
        }

        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {

        }

        protected override ProcessStatus OnUpdate()
        {
            Execute();
            return ProcessStatus.Success;
        }


    }

}
