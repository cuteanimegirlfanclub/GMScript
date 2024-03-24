using UnityEngine;
using GMEngine.Value;
using UnityEngine.UI;
using System;

namespace GMEngine
{
    public class ValueSlider : MonoBehaviour
    {
        public FloatReferenceRO sliderValue;
        public GameActionDelegate sliderUpdateEvent;

        private Slider slider;

        [SerializeField]
        private GameObject sliderUIPrefab;
        public GameObject sliderUI;

        private void Awake()
        {
            InitiateSliderUI();
        }

        public void InitiateSliderUI()
        {
            sliderUI = Instantiate(sliderUIPrefab, transform);
            slider = sliderUI.GetComponent<Slider>();
            sliderUI.SetActive(false);
            sliderUpdateEvent.Subscribe(onUpdateSlider);
        }

        public Action onUpdateSlider;

        /// <summary>
        /// the currentValue should between 0 and 1;
        /// </summary>
        /// <param name="currentTime"></param>
        public void UpdateSlider(float currentTime)
        {
            slider.value = currentTime;
            //slider.value = Mathf.MoveTowards(slider.value, currentTime / 10f, Time.deltaTime);
        }

        public void ShowSlider()
        {
            sliderUI.SetActive(true);
            SetSliderUITransform();
        }

        public void HideSlider()
        {
            sliderUI.SetActive(false);
            slider.value = 0f;
        }

        public void SetSliderUITransform()
        {
            Vector3 offset = new Vector3(-0.5f, 0.5f, 0);
            sliderUI.transform.position = sliderUI.transform.position + offset;
            sliderUI.transform.LookAt(Camera.main.transform);
        }
    }


}
