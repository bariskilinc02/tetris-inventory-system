using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{

    public StorageBase ConnectedStorage;
    public Item AssignedItem;
    public Tile PivotTileSlot;

    public List<Tile> ConnectedTileSlots;
}
