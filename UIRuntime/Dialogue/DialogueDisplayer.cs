using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GMEngine.UI
{
    [ExecuteInEditMode]
    public class DialogueDisplayer : MonoBehaviour
    {
        [Header("Display Setting")]
        [SerializeField] private int maxLength;
        [SerializeField] private float displayDelayTime;
        [SerializeField] private float charactersPerSecond;
        [Header("Cache")]
        [SerializeField] private TextMeshProUGUI textUI;
        private bool isDisplaying;

        public async UniTaskVoid DisplayDialogue(string message)
        {
            if (isDisplaying)
            {
                return;
            }

            isDisplaying = true;
            gameObject.SetActive(true);

            await PerCharacterDisplay(message);

            isDisplaying = false;
            gameObject.SetActive(false);
        }

        private async UniTask<bool> PerCharacterDisplay(string message)
        {
            textUI.text = "";
            int currentCharacterIndex = 0;
            while (currentCharacterIndex < message.Length)
            {
                textUI.text += message[currentCharacterIndex++];
                await UniTask.WaitForSeconds(1 / charactersPerSecond);
            }
            await UniTask.WaitForSeconds(displayDelayTime);
            return true;
        }



#if UNITY_EDITOR
        [Header("Editor")]
        [TextArea]
        public string editorMessage; 
        public async UniTaskVoid DisplayDialogueEditor(string message)
        {
            if (isDisplaying)
            {
                return;
            }

            isDisplaying = true;
            gameObject.SetActive(true);

            await PerCharacterDisplayEditor(message);

            isDisplaying = false;
            gameObject.SetActive(false);
        }
        private async UniTask<bool> PerCharacterDisplayEditor(string message)
        {
            textUI.text = "";
            int currentCharacterIndex = 0;
            while (currentCharacterIndex < message.Length)
            {
                textUI.text += message[currentCharacterIndex++];
                await UniTask.WaitForSeconds(1 / charactersPerSecond);
            }
            await UniTask.WaitForSeconds(displayDelayTime, true, PlayerLoopTiming.FixedUpdate);
            return true;
        }
#endif
    }
}

