using GMEngine.Value;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.GMAddressables
{
    public class AddressableMono : MonoBehaviour
    {
        [SerializeField] public StringReferenceRO address;

        public void Awake()
        {
            AddressablesManager.RegisterMember(this);
        }

        public T GetComponent<T>(string componentName)
        {
            return GetComponent<T>(componentName);
        }

        public T GetComponent<T>(Type type)
        {
            return GetComponent<T>(type);
        }

        public void OnDestroy()
        {
            AddressablesManager.UnRegisterMember(this);
        }
    }
}

