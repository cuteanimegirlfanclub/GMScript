using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace GMEngine.UI
{
    public enum BoxType
    {
        message,
        confirmation
    }

    public class SuspendedConfirmationBox : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI messageBox;

        public async UniTask<bool> WaitForUserResponse(string message, BoxType boxType)
        {
            messageBox.text = message; 

            //CancellationTokenSource cts = new CancellationTokenSource();

            //if (gameObject.activeSelf == true)
            //{
            //    cts.Cancel();
            //    Debug.Log($"{name} Cancelling Asynchronous Method");
            //}

            gameObject.SetActive(true);
            bool haveResponsed = false;

            switch (boxType)
            {
                case BoxType.message:
                    {
                        bool userResponse = await InternalCallMessageBox(haveResponsed);
                        return userResponse;
                    }
                case BoxType.confirmation:
                    {
                        Debug.Log($"{name} Getting Confirmation Box");
                        bool userResponse = await InternalCallConfirmationBox(haveResponsed);
                        return userResponse;
                    }
                default:
                    {
                        throw new InvalidOperationException($"Unsupported boxType:{boxType}");
                    }
            }
        }

        private async UniTask<bool> InternalCallConfirmationBox(bool haveResponsed)
        {
            BinaryButtonGroup btn = GetComponentInChildren<BinaryButtonGroup>();
            btn.GetButton(BoxType.confirmation);
            btn.gameObject.SetActive(true);

            bool userResponse = false;

            UnityAction yesAction = () =>
            {
                userResponse = true;
                haveResponsed = true;
            };
            UnityAction noAction = () =>
            {
                userResponse = false;
                haveResponsed = true;
            };

            btn.RegisterYesBtnListener(yesAction);
            btn.RegisterNoBtnListener(noAction);

            while (!haveResponsed)
            {
                await UniTask.Yield();
            }

            btn.UnregisterAllListener();

            gameObject.SetActive(false);

            return userResponse;
        }

        private async UniTask<bool> InternalCallMessageBox(bool haveResponsed)
        {
            BinaryButtonGroup btn = GetComponentInChildren<BinaryButtonGroup>();
            btn.GetButton(BoxType.message);
            btn.gameObject.SetActive(true);

            bool userResponse = false;

            UnityAction yesAction = () =>
            {
                userResponse = true;
                haveResponsed = true;
            };

            btn.RegisterYesBtnListener(yesAction);

            while (!haveResponsed)
            {
                await UniTask.Yield();
            }

            btn.UnregisterAllListener();

            //btn.gameObject.SetActive(false);
            gameObject.SetActive(false);

            return userResponse;
        }
    }
}

