using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine
{
    public class FadeInOutUI : MonoBehaviour
    {
        public float fadeDuration = 1.0f;

        [Header("Refercence")]
        public CanvasGroup canvasGroup;

        public IEnumerator FadeIn()
        {
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0;
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
            }

            float startTime = Time.time;
            float elapsedTime = 0;

            while (elapsedTime < fadeDuration)
            {
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
                }

                elapsedTime = Time.time - startTime;
                yield return null;
            }
        }

        public IEnumerator FadeOut()
        {
            if (canvasGroup != null)
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
            }

            float startTime = Time.time;
            float elapsedTime = 0;

            while (elapsedTime < fadeDuration)
            {
                if (canvasGroup != null)
                {
                    canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
                }

                elapsedTime = Time.time - startTime;
                yield return null;
            }
        }

        private void OnEnable()
        {
            StartCoroutine(FadeIn());
        }
    }
}
