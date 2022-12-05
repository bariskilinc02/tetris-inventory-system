using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/BaseItem", order = 1)]
public class Item: ScriptableObject
{
    public string Name;
    public string Id;

    public Vector2Int Size;
    public Vector2Int Coordinat;

    public Sprite Sprite;

    public bool Direction;

    public int Quantity;
    public int MaxQuantity;

    public float ItemWeight;
    public float TotalWeight;

    public bool IsStorageItem()
    {
        if (GetType() == typeof(StorageItem))
        {
            return true;
        }
        else
        {
            return false;
        }
      
    }

    public void ChangeItemDirection()
    {
        Vector2Int currentSize = Size;
        Direction = !Direction;

        Size = new Vector2Int(currentSize.y, currentSize.x);
    }
}
