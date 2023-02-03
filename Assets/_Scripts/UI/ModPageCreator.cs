using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModPageCreator : MonoBehaviour
{
    public static ModPageCreator Instance;

    public Item CurrentItem;
    
    public GameObject UIParent;
    
    [Header("Prefabs")]
    public GameObject WeaponPage;
    public GameObject SingleTileSlot;
    public GameObject ItemSlotPrefab;
    
    [Header("Page")]
    public GameObject Page;
    public Button CloseButton; 
    
    public GameObject ScrollView;
    public GameObject ItemSlots;
    public GameObject Tiles;

    public List<SingleTileSlot> TileSlots;

    
    private void Awake()
    {
        Instance = this;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            Create(CurrentItem);
        }
    }

    public void Create(Item item)
    {
        Destroy(Page);
        
        GetItem(item);
        CreateFrame();
        SetStorageComponents();
     
        CreateTileSlots();
        CreateItemSlots();
    }
    
    public void GetItem(Item item)
    {
        CurrentItem = item;
    }
    
    public void CreateFrame()
    {
        Page = Instantiate(WeaponPage, UIParent.transform);
    }
    
    public void SetStorageComponents()
    {
        ScrollView = Page.transform.GetChild(1).gameObject;
        ItemSlots = Page.transform.GetChild(2).gameObject;
        
        CloseButton = Page.transform.GetChild(3).GetChild(0).GetChild(0).GetComponent<Button>();
        CloseButton.onClick.AddListener(ClosePage);
        
        Tiles = ScrollView.transform.GetChild(0).GetChild(0).gameObject;
    }
    
    public void CreateTileSlots()
    {
        TileSlots.Clear();
        if (CurrentItem is ModItem weaponItem)
        {
            for (int i = 0; i < weaponItem.SubModItems.Count; i++)
            {
                GameObject tileSlot = Instantiate(SingleTileSlot, Tiles.transform);
                SingleTileSlot singleTileSlot = tileSlot.GetComponent<SingleTileSlot>();
                singleTileSlot.ConnectedParentItem = CurrentItem;
                singleTileSlot.ConnectedSubItem = weaponItem.SubModItems[i];
                
                TileSlots.Add(singleTileSlot);
            }
        }
    }
    
    public void CreateItemSlots()
    {
    
        if (CurrentItem is ModItem weaponItem)
        {
            for (int i = 0; i < weaponItem.SubModItems.Count; i++)
            {
               
                if (weaponItem.SubModItems[i].ModItem != null)
                {
                    GameObject itemSlotGameObject = Instantiate(ItemSlotPrefab, Tiles.transform);
                    ItemSlot itemSlot = itemSlotGameObject.GetComponent<ItemSlot>();
                    

                    itemSlot.AssignedItem = weaponItem.SubModItems[i].ModItem;
                    itemSlot.ConnectToSingleTileSlot(TileSlots[i]);
                    itemSlot.FitVisual(itemSlot.ConnectedSingleTileSlot);
                }
            }
        }
    }
    
    public void ClosePage()
    {
        CurrentItem = null;
        Destroy(Page);
    }
}
