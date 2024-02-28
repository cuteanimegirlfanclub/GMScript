using GMEngine.GMAddressables;
using GMEngine.Value;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GMEngine.GMNodes
{
    public class GMAudioActionNode : GMActionNode
    {
        [SerializeField] private MultipleSFXSimpleSO AudioSFXSO;
        [SerializeField] private StringReferenceRO audioSourceAddress;
        [SerializeField] private BooleanReferenceRO key;
        private AudioSource source;

        protected override void OnStart()
        {
            source = AddressablesManager.GetMember(audioSourceAddress.Value).GetComponent<AudioSource>();
        }

        protected override void OnStop()
        {

        }

        protected override ProcessStatus OnUpdate()
        {
            Execute();
            return ProcessStatus.Success;
        }

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
            AudioSFXSO.PlayPersist(source);
        }

        void StopAudio()
        {
            AudioSFXSO.Stop(source);
        }


    }

}
