using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GMEngine
{
    public class LightController : MonoBehaviour
    {
        public void OpenAndClose()
        {
            Light[] lights = GetComponentsInChildren<Light>();
            foreach (Light light in lights)
            {
                light.enabled = !light.enabled;
            }
        }

        public void OpenAndClose(bool key)
        {
            Light[] lights = GetComponentsInChildren<Light>();
            if (key)
            {
                foreach (Light light in lights) { light.enabled = true;}
            }
            else
            {
                foreach (Light light in lights) { light.enabled = false;}
            }
        }
    }

}

