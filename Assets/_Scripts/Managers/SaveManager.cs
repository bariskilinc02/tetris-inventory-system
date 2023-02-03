using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public bool LoadOnStart;

    [SerializeField] private Inventory Inventory;
    private const string inventoryId = "inventory";

    [SerializeField] private Inventory Storage;
    private const string storageId = "storage";

    [SerializeField] private EquipmentStorage PrimaryWeapon;
    private const string primaryWeaponId = "primaryWeapon";

    [SerializeField] private EquipmentStorage SecondaryWeapon;
    private const string secondaryWeaponId = "secondaryWeapon";
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
        SaveData<StorageData>(secondaryWeaponId, ExportStorage(SecondaryWeapon.Storage));
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

        if (PlayerPrefs.HasKey(secondaryWeaponId))
        {
            SecondaryWeapon.Storage = ImportStorage(LoadData<StorageData>(secondaryWeaponId));
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
       // Debug.Log(gameDataJson);
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
            itemData.ItemCoordinate = storage.Items[i].Coordinate;
            itemData.Direction = storage.Items[i].Direction;

            if (storage.Items[i] is StorageItem storageItem)
            {
                itemData.StorageData = ExportStorage(storageItem.Storage);
            }

            if (storage.Items[i] is WeaponItem weaponItem)
            {
                itemData.SubData = ExportSubData(weaponItem.SubModItems);
            }
            
            if (storage.Items[i] is ModItem modItem)
            {
                itemData.SubData = ExportSubData(modItem.SubModItems);
            }

            if (storage.Items[i] is MagazineItem magazineItem)
            {
                itemData.MagazineData = ExportMagazineData(magazineItem);
            }

            storageData.Items.Add(itemData);
        }


        return storageData;
    
    }

    private SubData ExportSubData(List<SubModItem> subModItems)
    {
        SubData subData = new SubData();
        for (int i = 0; i < subModItems.Count; i++)
        {
            ItemData itemData = new ItemData();

            if (subModItems[i].ModItem == null)
            {
                itemData.ItemId = "null";
            }
            else
            {
                itemData.ItemId = subModItems[i].ModItem.Id;
                itemData.ItemCoordinate = subModItems[i].ModItem.Coordinate;
                itemData.Direction = subModItems[i].ModItem.Direction;

                if (subModItems[i].ModItem is ModItem modItem)
                {
                    itemData.SubData = ExportSubData(modItem.SubModItems);
                }
            }


            subData.SubItems.Add(itemData);
        }

        return subData;
    }

    private MagazineData ExportMagazineData(MagazineItem magazineItem)
    {
        MagazineData magazineData = new MagazineData();

        for (int i = 0; i < magazineItem.Bullets.Count; i++)
        {
            ItemData itemData = new ItemData();

            itemData.ItemId = magazineItem.Bullets[i].Id;
            magazineData.Bullets.Add(itemData);
        }

        return magazineData;
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
            item.Coordinate = itemData.ItemCoordinate;
            item.Direction = itemData.Direction;
            item.Size = item.Direction ? new Vector2Int(item.Size.y, item.Size.x) : item.Size;

            if (item is StorageItem storageItem)
            {
                storageItem.Storage = ImportStorage(storageData.Items[i].StorageData);
                storageItem.Storage.Init();
            }

            if (item is WeaponItem weaponItem)
            {
                weaponItem.SubModItems = ImportSubData(itemData.SubData, weaponItem.SubModItems);
            }
            
            if (item is ModItem modItem)
            {
                modItem.SubModItems = ImportSubData(itemData.SubData, modItem.SubModItems);
            }
            if (item is MagazineItem magazineItem)
            {
                magazineItem.Bullets = ImportMagazine(itemData.MagazineData);
            }

            storage.Items.Add(item);
        }


        return storage;

    }

    private List<SubModItem> ImportSubData(SubData subData, List<SubModItem> subModItems)
    {
        for (int i = 0; i < subModItems.Count; i++)
        {
            if (subData.SubItems[i].ItemId == "null")
            {
                continue;
            }
            else
            {
                Item item = ItemBehaviour.CreateNewItem(subData.SubItems[i].ItemId);

                if (item is ModItem modItem)
                {
                    modItem.SubModItems = ImportSubData(subData.SubItems[i].SubData, modItem.SubModItems);
                }
                subModItems[i].ModItem = item;
            }
        }
        return subModItems;
    }

    private List<Item> ImportMagazine(MagazineData magazineData)
    {
        List<Item> Bullets = new List<Item>();
        for (int i = 0; i < magazineData.Bullets.Count; i++)
        {
            Item item = ItemBehaviour.CreateNewItem(magazineData.Bullets[i].ItemId);
            
            Bullets.Add(item);
        }

        return Bullets;
    }
    #endregion
}
