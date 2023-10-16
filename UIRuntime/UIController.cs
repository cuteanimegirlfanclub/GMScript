using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GMEngine
{
    public class UIController : MonoBehaviour
    {
        [Header("Input")]
        public InputAction openInventoryAction;

        [Header("Slot Cache")]
        public GameObject inventoryPrefab;

        private void Awake()
        {
            InstantiateInventoryUI();
        }

        private void OnEnable()
        {
            openInventoryAction.Enable(); openInventoryAction.canceled += DisplayInventoryUI;
        }

        private void OnDisable()
        {
            openInventoryAction.Disable(); openInventoryAction.canceled -= DisplayInventoryUI;
        }
        
        public void InstantiateInventoryUI()
        {
            inventoryPrefab = Instantiate(inventoryPrefab, transform);
            var inventoryUI = inventoryPrefab.GetComponent<InventoryUI>();
            inventoryUI.InitiateInventoryUI(inventoryUI.inventorySO);
            inventoryPrefab.SetActive(false);
        }

        private void DisplayInventoryUI(InputAction.CallbackContext ctx)
        {
            inventoryPrefab.SetActive(!inventoryPrefab.activeSelf);
        }
    }

}

