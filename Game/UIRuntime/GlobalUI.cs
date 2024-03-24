using UnityEngine;

namespace GMEngine.UI
{
    public class GlobalUI : MonoBehaviour
    {
        [SerializeField] private GameObject systemMessageBox;

        private void Awake()
        {
            InstantiateSystemUIBox();
        }

        public SuspendedConfirmationBox GetSystemBox()
        {
            return systemMessageBox.GetComponent<SuspendedConfirmationBox>();
        }

        public void InstantiateSystemUIBox()
        {
            systemMessageBox = Instantiate(Resources.Load("SuspendMessageBox", typeof(GameObject)), transform) as GameObject;
        }
    }


}
