using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.GMAddressables
{
    public class AddressablesManager : Singleton<AddressablesManager>
    {
        [SerializeField] private Dictionary<string, AddressableMono> addressables = new Dictionary<string, AddressableMono>();
        protected override void OnAwake()
        {

        }

        internal static void RegisterMember(AddressableMono addressable)
        {
            if(Instance.addressables.ContainsKey(addressable.address.Value))
            {
                Debug.LogWarning($"{addressable.gameObject.name} had been addressed!");
            }
            else
            {
                Instance.addressables.Add(addressable.address.Value, addressable);
            }
        }

        internal static void UnRegisterMember(AddressableMono addressable)
        {
            if (Instance.addressables == null) return;
            Instance.addressables.Remove(addressable.address.Value);
        }

        internal static AddressableMono GetMember(string address)
        {
            if(Instance.addressables.TryGetValue(address, out AddressableMono addressable))
            {
                return addressable;
            }
            else
            {
                Debug.LogWarning($"Can't address with {address}");
                return null;
            }
        }

    }

}
