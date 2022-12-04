using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoragePageCreator : MonoBehaviour
{
    public static StoragePageCreator Instace;

    #region Components
    [Header("Parents")]
    public GameObject UIParent;

    [Header("Prefabs")]
    public GameObject StoragePage;
    public GameObject TilePrefab;
    public GameObject ItemSlotPrefab;

    public GameObject EmptyRect;

    [Header("Storage Page")]
    public GameObject Page;

    public GameObject Background;
    public GameObject ScrollView;
    public GameObject ItemSlots;
    public GameObject Tiles;
    public Button CloseButton;


    [Header("Inputs")]
    public Item CurrentItem;
    public StorageBase CurrentInventory;
    public Storage CurrentStorage;

    [Header("Info")]

    public Vector2Int TileSize;
    #endregion

    private void Awake()
    {
        Instace = this;
    }

    public void GetStorageFromItem(Item item)
    {
        StorageItem currentStorageItem = (StorageItem)item;
        Storage currentStorage = currentStorageItem.Storage;

        CurrentStorage = currentStorage;
        CurrentItem = item;
    }

    public void GetStorageInfos()
    {
        TileSize = CurrentStorage.TileSize;
    }

    public void SetStorageComponents()
    {
        CurrentInventory = Page.GetComponent<Inventory>();
        CurrentInventory.Storage = CurrentStorage;
        CurrentInventory.Tiles = CurrentStorage.Tiles;
        CurrentInventory.ConnectedItem = CurrentItem;


        Background = Page.transform.GetChild(0).gameObject;
        ScrollView = Page.transform.GetChild(1).gameObject;
        ItemSlots = Page.transform.GetChild(2).gameObject;
        CloseButton = Page.transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<Button>();
        CloseButton.onClick.AddListener(ClosePage);

        Tiles = ScrollView.transform.GetChild(0).GetChild(0).gameObject;

        Page.GetComponent<RectTransform>().sizeDelta = new Vector2(TileSize.x * 100, TileSize.y*  100);
    }

    public void SetTileSlots()
    {
        CurrentInventory.TileSlots.Clear();

        for (int i = 0; i < TileSize.x * TileSize.y; i++)
        {
            TileSlot tileSlot = Tiles.transform.GetChild(i).GetComponent<TileSlot>();

            tileSlot.SetConnectedStorage(CurrentInventory);
            CurrentStorage.Tiles[i].ConnectedStorage = CurrentInventory;
            //TileSlots.Add(tileSlot);
        }
    }

    #region Create
    public void Create(Item item)
    {
        Destroy(Page);

        GetStorageFromItem(item);
        GetStorageInfos();

        CreateFrame();
        SetStorageComponents();

        CreateTileSlots();
        SetTileSlots();

    }
    public void CreateFrame()
    {
        Page = Instantiate(StoragePage, UIParent.transform);
    }
    
    public void CreateTileSlots()
    {
        for (int i = 0; i < TileSize.x; i++)
        {
            for (int a = 0; a < TileSize.y; a++)
            {
                GameObject TileSlot = Instantiate(TilePrefab, Tiles.transform);
                CurrentInventory.TileSlots.Add(TileSlot.GetComponent<TileSlot>());
            }
        }
    }


    #endregion


    public void ClosePage()
    {
        CurrentItem = null;
        CurrentInventory = null;
        Destroy(Page);
    }


}
