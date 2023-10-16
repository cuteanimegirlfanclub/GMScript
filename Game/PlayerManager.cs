using UnityEngine;
using GMEngine.Value;
using GMEngine;

public class PlayerManager : MonoBehaviour, ISaveDataSender, ISaveDataRecevier
{
    public GameObject playerPrefab;

    public InventorySO playerInventory;
    public BaseItemSO FallbackItem;

    public FloatReferenceRO playerMaxHealth;
    public FloatReferenceRO playerMaxMental;

    public FloatReferenceRW playerCurrentMental;
    public FloatReferenceRW playerCurrentHealth;

    private void Awake()
    {
        InitializePlayer();
    }

    public void InitializePlayer()
    {
        Debug.Log("Init Player");

#if UNITY_EDITOR
        if (GameObject.FindWithTag("MainChara"))
        {
            SetupStatus();
            SetupBasicInventory();
            RegisterStroage();
            return;
        }
#endif
        Instantiate(playerPrefab);
        SetupStatus();
        SetupBasicInventory();
        RegisterStroage();
    }

    private void SetupBasicInventory()
    {
        playerInventory.items.Clear();
        playerInventory.items.Add(FallbackItem);
        playerInventory.handItem = playerInventory.items[0];
    }

    private void SetupStatus()
    {
        playerCurrentHealth.SetValue(playerMaxHealth.Value);
        playerCurrentMental.SetValue(playerMaxMental.Value);
    }

    private void RegisterStroage()
    {
        SimpleStroage stroage = GameManager.Instance.GetComponent<SimpleStroage>();
        stroage.OnPullDataRequest += SendData;
        stroage.OnReceiveDataRequest += ReceiveData;
    }

    public void SendData(SaveData data)
    {
        var playerReference = GameObject.FindGameObjectWithTag("MainChara");

        data.playerPosition = playerReference.transform.position;
        data.playerRotation = playerReference.transform.rotation;
        
        data.playerHealth = playerCurrentHealth.Value;
        data.playerMental = playerCurrentMental.Value;

        //the first(number0)item is hand, which is a fall back item, so we should skip it
        for (int i = 1; i < playerInventory.items.Count; i++)
        {
            data.inventoryItemDatas.Add(data.PackToSaveData(playerInventory.items[i].gameObjectReference));
        }

        if (playerInventory.items.Contains(playerInventory.handItem))
        {
            data.handItemNum = playerInventory.items.IndexOf(playerInventory.handItem);
            Debug.Log($"Current handitem index  {data.handItemNum}");
        }
        else
        {
            data.handItemNum = -1;
        }
    }

    public void ReceiveData(SaveData data)
    {
        Debug.Log("Receiving Player Data...");
        //disable some component for receiving data...
        var playerReference = GameObject.FindGameObjectWithTag("MainChara");

        playerReference.GetComponent<Animator>().enabled = false;
        playerReference.GetComponent<CharacterController>().enabled = false;

        playerReference.transform.position = data.playerPosition;
        playerReference.transform.rotation = data.playerRotation;

        SetupStatus();
        SetupBasicInventory();

        foreach (var item in data.inventoryItemDatas)
        {
            Vector3 inventoryPosition = new Vector3(0, -5, 0);

            BaseItemSO itemSO = data.UnPackToGameData(item);
            itemSO.name = item.name;
            playerInventory.AddItem(itemSO);

            if(itemSO.gameObjectReference == null)
            {
                ItemManager.Instance.CreateItem(item.name, inventoryPosition, Quaternion.identity);
            }
            
        }

        //if(data.handItemNum != -1)
        //{
        //    BaseItemSO handItemSO = playerInventory.items[data.handItemNum];
        //    playerInventory.SetHandItem(handItemSO);
        //}


        //enable
        playerReference.GetComponent<Animator>().enabled = true;
        playerReference.GetComponent<CharacterController>().enabled = true;
    }


}
