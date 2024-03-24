using GMEngine.Value;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace GMEngine.UI
{
    public class TimerSliderController : MonoBehaviour
    {
        [SerializeField, Obsolete]
        private SimpleTimerSO pickUpTimer;

        private Slider slider;

        [Header("Prefab")]
        [SerializeField] private GameObject prefabSliderUI;
        public GameObject sliderUI;

        /// <summary>
        /// in seconds that the timer exist after the timer count down completed 
        /// </summary>
        public FloatReferenceRO durationdDelayed;

        private void Awake()
        {
            sliderUI = Instantiate(prefabSliderUI, transform);
            slider = sliderUI.GetComponent<Slider>();
            slider.gameObject.layer = 8;
            sliderUI.SetActive(false);
        }

        private void Update()
        {
            //use coroutine later
            if (!sliderUI.activeSelf) return;
            slider.value = Mathf.MoveTowards(slider.value, slider.maxValue + durationdDelayed.Value, Time.deltaTime);
        }

        public void UseSliderOnce(float duration)
        {
            if(sliderUI.activeSelf) return;
            slider.maxValue = duration;
            sliderUI.SetActive(true);
            Debug.Log($"{gameObject.name} is showing with duration : {duration} secs");
        }

        public void HideSlider()
        {
            sliderUI.SetActive(false);
            ResetTimer();
        }

        public void ResetTimer()
        {
            slider.value = 0;
            slider.maxValue = 1;
        }
    }

}
