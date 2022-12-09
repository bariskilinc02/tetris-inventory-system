using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public bool LoadOnStart;

    [SerializeField] private Inventory Inventory;
    private const string inventoryId = "inventory";

    [SerializeField] private Inventory Storage;
    private const string storageId = "storage";

    [SerializeField] private Inventory PrimaryWeapon;
    private const string primaryWeaponId = "primaryWeapon";

    private void Awake()
    {
        if(LoadOnStart) LoadGame();

    }

    private void Start()
    {
    
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            SaveGame();
        }
    }

    #region Save Load Game
    private void SaveGame()
    {
        SaveData<StorageData>(inventoryId, ExportStorage(Inventory.Storage));
        SaveData<StorageData>(storageId, ExportStorage(Storage.Storage));
        SaveData<StorageData>(primaryWeaponId, ExportStorage(PrimaryWeapon.Storage));
    }

    private void LoadGame()
    {
        if (PlayerPrefs.HasKey(inventoryId))
        {
            Inventory.Storage = ImportStorage(LoadData<StorageData>(inventoryId));
        }

        if (PlayerPrefs.HasKey(storageId))
        {
            Storage.Storage = ImportStorage(LoadData<StorageData>(storageId));
        }

        if (PlayerPrefs.HasKey(primaryWeaponId))
        {
            PrimaryWeapon.Storage = ImportStorage(LoadData<StorageData>(primaryWeaponId));
        }

    }

    #endregion

    #region Save / Load
    /// <summary>
    /// Save data with given key value
    /// </summary>
    public void SaveData<T>(string key, T saveData)
    {
        string gameDataJson = DataBehaviour.Serialize<T>(saveData);
        Debug.Log(gameDataJson);
        PlayerPrefs.SetString(key, gameDataJson);
    }

    /// <summary>
    /// Load data with save key saved earlier
    /// </summary>
    public T LoadData<T>(string key)
    {
        string gameDataJson = PlayerPrefs.GetString(key);
        Debug.Log(gameDataJson);
        return DataBehaviour.DeSerialize<T>(gameDataJson);
    }

    #endregion

    #region Exports for Save
    private StorageData ExportStorage(Storage storage)
    {
        StorageData storageData = new StorageData();

        storageData.TileSize = storage.TileSize;

        for (int i = 0; i < storage.Items.Count; i++)
        {
            ItemData itemData = new ItemData();

            itemData.ItemId = storage.Items[i].Id;
            itemData.ItemCoordinate = storage.Items[i].Coordinat;
            itemData.Direction = storage.Items[i].Direction;

            if (storage.Items[i].IsStorageItem())
            {
                StorageItem storageItem = (StorageItem)storage.Items[i];
                itemData.StorageData = ExportStorage(storageItem.Storage);
            }
            

            storageData.Items.Add(itemData);
        }


        return storageData;
    
    }
    #endregion

    #region Imports for Load
    private Storage ImportStorage(StorageData storageData)
    {
        Storage storage = new Storage();

        storage.TileSize = storageData.TileSize;

        for (int i = 0; i < storageData.Items.Count; i++)
        {
            ItemData itemData = storageData.Items[i];

            Item item = ItemBehaviour.CreateNewItem(itemData.ItemId);
            item.Coordinat = itemData.ItemCoordinate;
            item.Direction = itemData.Direction;
            item.Size = item.Direction ? new Vector2Int(item.Size.y, item.Size.x) : item.Size;

            if (item.IsStorageItem())
            {
                StorageItem storageItem = (StorageItem)item;
                storageItem.Storage = ImportStorage(storageData.Items[i].StorageData);
                storageItem.Storage.Init();
            }

            storage.Items.Add(item);
        }


        return storage;

    }
    #endregion
}
