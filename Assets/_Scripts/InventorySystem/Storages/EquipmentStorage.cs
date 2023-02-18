using System.Collections;
using System.Collections.Generic;
using _Scripts.Interfaces.Item;
using UnityEngine;

public class EquipmentStorage : StorageBase, IAddable
{
    public TileSlot connectedTileSlot;
    public void AddItem(ItemSlot itemSlot, TileSlot tileSlot, out bool isAdded)
    {
        if (tileSlot.ConnectedStorage.AvailableItemTypes.Exists(x => x == itemSlot.AssignedItem.ItemType))
        {
            if (itemSlot.AssignedItem is StorageItem storageItem && storageItem.IsSubItem(tileSlot.ConnectedStorage.ConnectedItem))
            {
                isAdded = false;
            }
            else if (tileSlot.ConnectedStorage.IsEmptyTileArea(new Vector2Int(1, 1), new Vector2Int(0, 0)))//if (IsTileAreaEmptyForItem(CurrentMovingItem.AssignedItem, _currentSlot))
            {
                tileSlot.ConnectedStorage.SetItemToEmptyTile(itemSlot.AssignedItem, tileSlot.ConnectedTile);

                tileSlot.ConnectedStorage.ReplaceItemSlot(itemSlot, tileSlot.ConnectedTile);
                tileSlot.ConnectedStorage.SynchTileSlotInItemSlot(itemSlot, tileSlot.Coordinates);
                itemSlot.ConnectedSingleTileSlot = null;

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
