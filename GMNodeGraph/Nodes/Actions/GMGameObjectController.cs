using GMEngine.GMAddressables;
using GMEngine.Value;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.GMNodes
{
    public class GMGameObjectController : GMActionNode
    {
        [SerializeField] private BooleanReferenceRO key;
        [SerializeField] private StringReferenceRO address;
        protected override void OnStart()
        {

        }

        protected override void OnStop()
        {
            
        }

        protected override ProcessStatus OnUpdate()
        {
            AddressablesManager.GetMember(address.Value).gameObject.SetActive(key.Value);
            return ProcessStatus.Success;
        }
    }
}

