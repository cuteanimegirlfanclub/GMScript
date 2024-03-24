using GMEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GMEngine.Game
{
    public class GlobalMenuItem : GMUgui
    {
        [SerializeField] private Button btn;
        private static readonly string _default = "Action";

        public void Awake()
        {
            Initiate();
        }

        public void OnDisable()
        {
            GetComponentInChildren<TextMeshProUGUI>().text = _default;
            btn.onClick.RemoveAllListeners();
        }

        public override void Initiate()
        {
            if (btn == null)
            {
                btn = GetComponent<Button>();
            }
        }
    }
}

