using UnityEngine;
using UnityEngine.UI;

namespace GMEngine.UI
{
    public class TimerSlider : MonoBehaviour
    {
        [SerializeField]
        private SimpleTimerSO pickUpTimer;

        private Slider slider;

        [SerializeField]
        private GameObject prefabSliderUI;
        public GameObject sliderUI;

        private void Awake()
        {
            sliderUI = Instantiate(prefabSliderUI, transform);
            slider = sliderUI.GetComponent<Slider>();
            slider.gameObject.layer = 8;
            sliderUI.SetActive(false);
        }

        private void OnEnable()
        {
            pickUpTimer.OnTimerBegin += ShowSlider;
            pickUpTimer.OnTimerEnd += HideSlider;
            pickUpTimer.OnTimerUpdate += UpdateSlider;
        }

        private void OnDisable()
        {
            pickUpTimer.OnTimerBegin -= ShowSlider;
            pickUpTimer.OnTimerEnd -= HideSlider;
            pickUpTimer.OnTimerUpdate -= UpdateSlider;
        }

        private void UpdateSlider(float currentTime)
        {
            slider.value = currentTime / 10f;
            //slider.value = Mathf.MoveTowards(slider.value, currentTime / 10f, Time.deltaTime);
        }

        private void ShowSlider()
        {
            Debug.Log($"showing timer {pickUpTimer}");
            sliderUI.SetActive(true);
            //SetSliderUITransform();
        }

        private void HideSlider()
        {
            sliderUI.SetActive(false);
            slider.value = 0f;
        }
    }

}
