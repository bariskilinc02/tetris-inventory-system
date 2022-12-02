using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStoragable
{
    public Vector2Int TileSize { get; set; }
    public List<ItemBase> SubItems { get; set; }
}
