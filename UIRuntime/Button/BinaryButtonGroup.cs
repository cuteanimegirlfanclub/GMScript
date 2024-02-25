using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEditor.Build.Pipeline.Tasks;

namespace GMEngine.UI
{
    public class BinaryButtonGroup : MonoBehaviour
    {
        [SerializeField] private Button yesBtn;
        [SerializeField] private Button noBtn;

        private void OnEnable()
        {
            yesBtn.onClick.RemoveAllListeners();
            noBtn.onClick.RemoveAllListeners();
        }

        public void UnregisterAllListener()
        {
            yesBtn.onClick.RemoveAllListeners();
            noBtn.onClick.RemoveAllListeners();
        }

        public void RegisterYesBtnListener(UnityAction action)
        {
            Debug.Log($"Registering {action}");
            yesBtn.onClick.AddListener(action);
        }

        public void RegisterNoBtnListener(UnityAction action)
        {
            Debug.Log($"Registering {action}");
            noBtn.onClick.AddListener(action);
        }

        public void GetButton(BoxType box)
        {
            UnregisterAllListener();
            switch (box)
            {
                case BoxType.message:
                {
                        yesBtn.transform.position.Set(0, transform.position.y, transform.position.z);
                        yesBtn.gameObject.SetActive(true);
                        noBtn.gameObject.SetActive(false);
                        break;

                }
                case BoxType.confirmation:
                {
                        Transform transform = noBtn.transform;
                        yesBtn.transform.position.Set(-transform.position.x,transform.position.y, transform.position.z);
                        yesBtn.gameObject.SetActive(true);
                        noBtn.gameObject.SetActive(true);
                        break;
                }
            }
        }

        private void OnValidate()
        {
            if(transform.localPosition.x != 0 || transform.localPosition.y != 0)
            {
                Debug.LogError($"{gameObject.name} Button Group Require Zero Argument Transform!");
                return;
            }
        }
    }


}
