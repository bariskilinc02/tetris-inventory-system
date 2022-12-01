using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{

    public StorageBase ConnectedStorage;
    public ItemBase AssignedItem;
    public TileSlot PivotTileSlot;

    public List<TileSlot> ConnectedTileSlots;
}
