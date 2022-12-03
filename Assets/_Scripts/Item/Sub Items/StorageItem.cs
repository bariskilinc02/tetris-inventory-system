using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Storage Item", order = 1)]
public class StorageItem : ItemBase, IStoragable
{

    [SerializeField] private Vector2Int _tileSize;
    [SerializeField] private List<ItemBase> _items;

    public Vector2Int TileSize {
        get => _tileSize;
        set => _tileSize = value; 
    }

    public List<ItemBase> Items { 
        get => _items;
        set => _items = value;
    }
}
