using GMEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GMEngine.Game
{
    public class GlobalContextualMenu : GMUgui, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObjectFactory menuItemFactory;
        [SerializeField] GameObject menuItemPrefab;
        public bool isSetup = false;
        public int currentMenuCount = -1;
        [SerializeField] private bool isFocused;

        public override void Initiate()
        {
            menuItemFactory = ScriptableObject.CreateInstance<GameObjectFactory>();
            menuItemFactory.Setup(menuItemPrefab, 5);
        }

        public void OnDisable()
        {
            PoolAll();
            isSetup = false;
            currentMenuCount = -1;
        }

        private void PoolAll()
        {
            foreach(Transform child in transform)
            {
                menuItemFactory.PoolProduct(child.gameObject);
            }
        }

        public void AppendAction(string actionName, UnityAction action)
        {
            GameObject menuItem = menuItemFactory.GetProduct(transform);
            currentMenuCount++;
            var btn = menuItem.GetComponentInChildren<Button>();
            var text = menuItem.GetComponentInChildren<TextMeshProUGUI>();
            text.text = actionName;
            isSetup = true;
            SetButtonPosition(menuItem);
            if(action != null)
            {
                btn.onClick.AddListener(action);
                btn.onClick.AddListener(DisableSelf);
            }
            else
            {
                Debug.Log($"the action trying be added to {name} is null");
            }
        }

        private void SetButtonPosition(GameObject menuItem)
        {
            var rect = menuItem.GetComponent<RectTransform>().rect;
            Vector3 newPos = new Vector3(0, - currentMenuCount * rect.height, 0);
            menuItem.GetComponent<RectTransform>().localPosition = newPos;
            int current = currentMenuCount + 1;
            GetComponent<RectTransform>().sizeDelta = new Vector2(rect.width, rect.height * current);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //Debug.Log($"{name} entered");
            isFocused = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            //Debug.Log($"{name} exited");
            if (isFocused)
            {
                OnDisable();
                gameObject.SetActive(false);
                isFocused = false;
            }
        }

        private void DisableSelf()
        {
            gameObject.SetActive(false);
        }
    }
}
