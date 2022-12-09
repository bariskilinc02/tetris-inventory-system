using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageUtils : MonoBehaviour
{
    #region TileSlot Inits
    public void AccessToTileSlots(List<TileSlot> TileSlots, Transform TargetTileMap)
    {
        TileSlots.Clear();

        for (int i = 0; i < TargetTileMap.childCount; i++)
        {
            TileSlot tileSlot = TargetTileMap.GetChild(i).GetComponent<TileSlot>();

            TileSlots.Add(tileSlot);
        }
    }

    public void AssingConnectedStorageToTileSlot(List<TileSlot> TileSlots, StorageBase Storage)
    {
        TileSlots.Clear();

        for (int i = 0; i < TileSlots.Count; i++)
        {
            TileSlots[i].SetConnectedStorage(Storage);
        }
    }

    public void SetTileSlotCoordinates(List<TileSlot> TileSlots, Vector2Int TileSize)
    {
        int counter = 0;
        for (int i = 0; i < TileSlots.Count / TileSize.x; i++)
        {
            for (int l = 0; l < TileSlots.Count / TileSize.y; l++)
            {
                TileSlots[counter].Coordinats = new Vector2Int(l, i);
                counter += 1;
            }
        }
    }
    #endregion

}
