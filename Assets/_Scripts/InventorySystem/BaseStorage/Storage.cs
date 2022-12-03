using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Storage
{
    [SerializeField] private Vector2Int _tileSize;
    [SerializeField] private List<ItemBase> _items;

    public Vector2Int TileSize
    {
        get => _tileSize;
        set => _tileSize = value;
    }

    public List<ItemBase> Items
    {
        get => _items;
        set => _items = value;
    }


}
