using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Storage : IStorage
{
    [SerializeField] private Vector2Int _tileSize;
    [SerializeField] private List<Item> _items = new List<Item>();
    [SerializeField] private List<Tile> _tiles = new List<Tile>();

    public Vector2Int TileSize
    {
        get => _tileSize;
        set => _tileSize = value;
    }

    public List<Item> Items
    {
        get => _items;
        set => _items = value;
    }

    public List<Tile> Tiles
    {
        get => _tiles;
        set => _tiles = value;
    }

    public void Init()
    {
        CreateTiles();
        SetTileCoordinates();
    }

    #region Base
    public void CreateTiles()
    {

        Tiles.Clear();
        
        for (int i = 0; i < TileSize.x * TileSize.y; i++)
        {
            Tile tile = new Tile();

            Tiles.Add(tile);
        }
    }

    public void SetTileCoordinates()
    {
        int counter = 0;
        for (int i = 0; i < Tiles.Count / TileSize.x; i++)
        {
            for (int l = 0; l < Tiles.Count / TileSize.y; l++)
            {
                Tiles[counter].Coordinats = new Vector2Int(l, i);
                counter += 1;
            }
        }
    }

    public void AddItem_Auto(string id)
    {
        Item item = ItemBehaviour.CreateNewItem(id);

        Vector2Int toCoordinate = FindEmptyTileArea(item.Size);

        if (toCoordinate != new Vector2Int(-1, -1))
        {
            item.Coordinat = toCoordinate;
            AddItem_ToCoordinate(item, toCoordinate);

        }
    }
    public void MoveItem_Auto(Item item)
    {
        Vector2Int toCoordinate = FindEmptyTileArea(item.Size);

        if (toCoordinate != new Vector2Int(-1, -1))
        {
            item.Coordinat = toCoordinate;
            AddItem_ToCoordinate(item, toCoordinate);

        }
    }

    #endregion

    #region Control
    public bool IsTileAreaEmpty(Vector2Int itemSize, Vector2Int coordinate)
    {
        Vector2Int pivotPosition = coordinate;

        bool isEmpty = true;

        for (int i = 0; i < itemSize.x; i++)
        {
            for (int l = 0; l < itemSize.y; l++)
            {
                Tile slot = Tiles.Find(x => x.Coordinats.x == coordinate.x + i && x.Coordinats.y == coordinate.y + l);
                if (slot == null)
                {
                    isEmpty = false;
                    return isEmpty;
                }
                else if (slot.AssignedItem != null)
                {
                    isEmpty = false;
                    return isEmpty;
                }
            }
        }

        return isEmpty;
    }
    public Vector2Int FindEmptyTileArea(Vector2Int itemSize)
    {
        Vector2Int tempCoordinate = new Vector2Int();
        Debug.Log(Tiles.Count);
        for (int a = 0; a < Tiles.Count; a++)
        {
            tempCoordinate = Tiles[a].Coordinats;

            if (IsTileAreaEmpty(itemSize, tempCoordinate))
            {
                return tempCoordinate;
            }
        }

        return new Vector2Int(-1, -1);
    }

    public void AddItem_ToCoordinate(Item item, Vector2Int coordinate)
    {
        Vector2Int pivotPosition = coordinate;

        for (int i = 0; i < item.Size.x; i++)
        {
            for (int l = 0; l < item.Size.y; l++)
            {
                Tile slot = Tiles.Find(x => x.Coordinats.x == coordinate.x + i && x.Coordinats.y == coordinate.y + l); //currentSlot.ConnectedStorage.ItemSlots.Find(x => x.Coordinats.x == currentSlot.Coordinats.x + i && x.Coordinats.y == currentSlot.Coordinats.y + l);
                slot.AssignedItem = item;
            }
        }

        Items.Add(item); 
        //Tiles.Find(x => x.Coordinats.x == coordinate.x && x.Coordinats.y == coordinate.y).ConnectedStorage.Storage.Items.Add(item);

    }
    #endregion
}
