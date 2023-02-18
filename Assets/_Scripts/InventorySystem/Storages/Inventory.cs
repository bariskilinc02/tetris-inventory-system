using System.Collections;
using System.Collections.Generic;
using _Scripts.Interfaces.Item;
using UnityEngine;

public class Inventory: StorageBase, IAddable
{
    public void AddItem(ItemSlot itemSlot, TileSlot tileSlot, out bool isAdded)
    {
        if (AvailableItemTypes.Exists(x => x == itemSlot.AssignedItem.ItemType))
        {
            if (itemSlot.AssignedItem is StorageItem storageItem && storageItem.IsSubItem(tileSlot.ConnectedStorage.ConnectedItem))//if (itemSlot.AssignedItem == ConnectedItem)
            {
                isAdded = false;
            }
            else if (tileSlot.ConnectedStorage.IsEmptyTileArea(itemSlot.AssignedItem.Size, tileSlot.Coordinates))
            {
                SetItemToEmptyArea(itemSlot.AssignedItem, tileSlot.ConnectedTile);
        
                ReplaceItemSlot(itemSlot, tileSlot.ConnectedTile);
                SynchTileSlotsInItemSlot(itemSlot, tileSlot.Coordinates);
                isAdded = true;
            }
            else
            {
                isAdded = false;
            }
        }
        else
        {
            isAdded = false;
        }
    }
}
