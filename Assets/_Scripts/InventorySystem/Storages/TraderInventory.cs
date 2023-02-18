using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TraderInventory : Inventory
{
    public void LoadTraderInventory(ItemList itemList)
    {
        Storage.Items.Clear();
        ClearTiles();
        
        for (int i = 0; i < itemList.Items.Count; i++)
        {
            AddItemToInventory(itemList.Items[i].Id);
        }

        
        RefreshInventoryPage();
    }
    
    private void AddItemToInventory(string itemId)
    {
        if (itemId == null) return;

        AddItem_Auto(itemId);
    }
}
