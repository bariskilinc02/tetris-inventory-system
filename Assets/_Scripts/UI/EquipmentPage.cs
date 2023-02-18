using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentPage : MonoBehaviour
{
    public EquipmentStorage thisInventory;

    public Inventory connectedStorage;

    public Transform TargetTileMap;
    public Transform TargetItemSlots;
    [SerializeField] private GridLayoutGroup grid;
    [SerializeField] private Transform pageParent;
    
    [SerializeField] private GameObject tileSlotPrefab;

    public void Update()
    {
        if (thisInventory.Storage.isStorageUpdated)
        {
            StartCoroutine(CreateUI());
            thisInventory.Storage.isStorageUpdated = false;
        }
        else if(thisInventory.Storage.Items.Count > 0 && thisInventory.Storage.Items[0] is StorageItem storageItem && storageItem.Storage.isStorageUpdated)
        {
            StartCoroutine(UpdateUI());
            storageItem.Storage.isStorageUpdated = false;
        }
    }

    private IEnumerator CreateUI()
    {
        DeleteTileSlots();
        DeleteItemSlots();

        
        if (thisInventory.Storage.Items.Count == 0)
        {
            connectedStorage.TargetTileMap = null;
            connectedStorage.TargetItemSlots = null;
            
        }
        else
        {
            StorageItem targetItem = thisInventory.Storage.Items[0] as StorageItem;
            
            connectedStorage.TargetTileMap = null;
            connectedStorage.TargetItemSlots = null;
            
            connectedStorage.AvailableItemTypes = targetItem.AvailableItems;

            connectedStorage.ConnectedItem = targetItem;
            connectedStorage.Storage = targetItem.Storage;
            connectedStorage.Tiles = targetItem.Storage.Tiles;
            
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = targetItem.Storage.TileSize.x;

            DrawTileSlots(targetItem.Storage.TileSize);

            yield return new WaitForEndOfFrame();
            
            connectedStorage.TargetTileMap = TargetTileMap;
            connectedStorage.TargetItemSlots = TargetItemSlots;

            connectedStorage.Init();
        }
       
    }
    private IEnumerator UpdateUI()
    {
        if (thisInventory.Storage.Items.Count == 0)
        {
            connectedStorage.TargetTileMap = null;
            connectedStorage.TargetItemSlots = null;
            
        }
        else
        {
            StorageItem targetItem = thisInventory.Storage.Items[0] as StorageItem;
            
            connectedStorage.TargetTileMap = null;
            connectedStorage.TargetItemSlots = null;

            connectedStorage.ConnectedItem = targetItem;
            connectedStorage.Storage = targetItem.Storage;
            connectedStorage.Tiles = targetItem.Storage.Tiles;
            
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = targetItem.Storage.TileSize.x;
            
            yield return new WaitForEndOfFrame();
            
            connectedStorage.TargetTileMap = TargetTileMap;
            connectedStorage.TargetItemSlots = TargetItemSlots;

            connectedStorage.Init();
        }
       
    }

    public void RefreshPage()
    {
        StartCoroutine(CreateUI());
    }
    

    private void DrawTileSlots(Vector2Int gridSize)
    {
        for (int i = 0; i < gridSize.x * gridSize.y; i++)
        {
            Instantiate(tileSlotPrefab, TargetTileMap);
        }
    }

    private void DeleteTileSlots()
    {
        for (int i = 0; i < TargetTileMap.transform.childCount; i++)
        {
            Destroy(TargetTileMap.transform.GetChild(i).gameObject);
        }
    }
    
    private void DeleteItemSlots()
    {
        for (int i = 0; i < TargetItemSlots.transform.childCount; i++)
        {
            Destroy(TargetItemSlots.transform.GetChild(i).gameObject);
        }
    }
}
