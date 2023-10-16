using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GMEngine
{
    public class LoadingUI : MonoBehaviour
    {
        private Slider slider;

        public void Awake()
        {
            slider = GetComponent<Slider>();
        }

        public void SetupSliderValue(float value)
        {
            slider.value = value;
        }

    }
}


