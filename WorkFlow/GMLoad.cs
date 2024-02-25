using GMEngine.Prototype;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;

namespace GMEngine.WorkFlow
{
    public static class GMLoad
    {
        public static async UniTask<T> LoadPrototypeAsync<T>(string address) where T : IPrototype<T>
        {
            T product =  await Addressables.LoadAssetAsync<T>(address);
            return product.DeepCopy();
        }

        
    }


}
