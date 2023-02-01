using System.Collections;
using System.Collections.Generic;
using _Scripts.Interfaces.Item;
using UnityEngine;

public class EquipmentStorage : StorageBase, IAddable
{
    
    public void AddItem(ItemSlot itemSlot, TileSlot tileSlot, out bool isAdded)
    {
        if (tileSlot.ConnectedStorage.AvailableItemTypes.Exists(x => x == itemSlot.AssignedItem.ItemType))
        {
            if (tileSlot.ConnectedStorage.IsEmptyTileArea(new Vector2Int(1, 1), new Vector2Int(0, 0)))//if (IsTileAreaEmptyForItem(CurrentMovingItem.AssignedItem, _currentSlot))
            {
                tileSlot.ConnectedStorage.SetItemToEmptyTile(itemSlot.AssignedItem, tileSlot.ConnectedTile);

                tileSlot.ConnectedStorage.ReplaceItemSlot(itemSlot, tileSlot.ConnectedTile);
                tileSlot.ConnectedStorage.SynchTileSlotInItemSlot(itemSlot, tileSlot.Coordinates);

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
