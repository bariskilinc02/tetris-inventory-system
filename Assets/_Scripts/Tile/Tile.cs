using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile 
{
    public StorageBase ConnectedStorage;
    public ItemBase AssignedItem;
    public Vector2Int Coordinats;
    public TileSlot TileSlot;

    public void SetConnectedStorage(StorageBase storageBase)
    {
        ConnectedStorage = storageBase;
    }
}
