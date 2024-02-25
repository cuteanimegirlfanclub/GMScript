using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine.Editor
{ 
    public static class GMGUIResources
    {
        public static Texture2D SliderBar { get => _sliderBar != null ? _sliderBar : _sliderBar = Resources.Load<Texture2D>("Assets/GMEngine/GMResources/Resources/BasicUI Metal/Sprites/SliderHandleSprite.png"); }
        private static Texture2D _sliderBar;

        public static Texture2D SliderHandle { get => _sliderHandle != null ? _sliderHandle : _sliderHandle = Resources.Load<Texture2D>("Assets/GMEngine/GMResources/Resources/BasicUI Metal/Toggle/Toggle.png"); }
        private static Texture2D _sliderHandle;
    }
}

