using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStorage
{
    public Vector2Int TileSize { get; set; }
    public List<Item> Items { get; set; }
    public List<Tile> Tiles { get; set; }
}
