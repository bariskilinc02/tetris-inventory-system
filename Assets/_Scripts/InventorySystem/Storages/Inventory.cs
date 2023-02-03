using System.Collections;
using System.Collections.Generic;
using _Scripts.Interfaces.Item;
using UnityEngine;

public class Inventory: StorageBase, IAddable
{

    public void AddItem(ItemSlot itemSlot, TileSlot tileSlot, out bool isAdded)
    {
        
        if (itemSlot.AssignedItem == StoragePageCreator.Instance.CurrentItem)
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
}
