using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Storage Item", order = 1)]
public class StorageItem : ItemBase, IStoragable, IConstructor<StorageItem>
{

    [SerializeField] private Vector2Int _tileSize;
    [SerializeField] private List<ItemBase> _subItems;
    public Vector2Int TileSize {
        get => _tileSize;
        set => _tileSize = value; 
    }

    public List<ItemBase> SubItems { 
        get => _subItems;
        set => _subItems = value;
    }

    public new StorageItem Construct()
    {
        return new StorageItem();
    }

    public new StorageItem Construct(string id)
    {
        return new StorageItem(id);
    }

    public StorageItem() 
    {

    }
    public StorageItem(string id)
    {
        StorageItem item = (StorageItem)ItemDataBase.Instance.Items.Find(x => x.Id == id);
        Name = item.Name;
        Id = item.Id;
        Size = item.Size;
        Coordinat = item.Coordinat;
        Sprite = item.Sprite;
        Quantity = item.Quantity;
        MaxQuantity = item.MaxQuantity;
        ItemWeight = item.ItemWeight;
        TotalWeight = item.TotalWeight;
        TileSize = item.TileSize;
        SubItems = new List<ItemBase>();

    }
    public StorageItem(StorageItem item /*, new fields */) : base(item)
    {

    }
}
