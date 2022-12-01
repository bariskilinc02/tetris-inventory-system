using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Level", order = 1)]
public class ItemBase: ScriptableObject
{
    public string Name;
    public string Id;

    public Vector2Int Size;
    public Vector2 Coordinat;

    public Sprite Sprite;

    public int Quantity;
    public int MaxQuantity;

    public float ItemWeight;
    public float TotalWeight;

    public ItemBase()
    {

    }

    public ItemBase(ItemBase item)
    {
        Name = item.Name;
        Id = item.Id;
        Size = item.Size;
        Coordinat = item.Coordinat;
        Sprite = item.Sprite;
        Quantity = item.Quantity;
        MaxQuantity = item.MaxQuantity;
        ItemWeight = item.ItemWeight;
        TotalWeight = item.TotalWeight;
    }

    public ItemBase(string id)
    {
        ItemBase item = ItemDataBase.Instance.Items.Find(x => x.Id == id);
        Name = item.Name;
        Id = item.Id;
        Size = item.Size;
        Coordinat = item.Coordinat;
        Sprite = item.Sprite;
        Quantity = item.Quantity;
        MaxQuantity = item.MaxQuantity;
        ItemWeight = item.ItemWeight;
        TotalWeight = item.TotalWeight;
    }
}
