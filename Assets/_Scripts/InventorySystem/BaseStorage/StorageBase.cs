using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageBase : MonoBehaviour
{


    public Transform TargetTileMap;
    public Transform TargetItemSlots;

    public Vector2Int TileSize;
    public List<ItemBase> Items;
    public List<TileSlot> TileSlots;
    public List<Tile> Tiles;

    [SerializeField]private bool _isConnectedToTarget;
    [SerializeField] private bool _isStorageUIUpdated;

    public GameObject ItemSlotPrefab;

    void Init()
    {
        ConnectSelf();

        _isConnectedToTarget = TargetTileMap == null ? false : true;

        if (_isConnectedToTarget)
        {
            ConnectToInventoryPage();
        }
    }

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        AddItem_Auto("akm");
        AddItem_Auto("glock17");
        AddItem_Auto("chest");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            AddItem_Auto("glock17");
        }

        if (_isConnectedToTarget == false)
        {
            if (TargetTileMap != null)
            {
                ConnectToInventoryPage();
                RefreshInventoryPage();
                _isConnectedToTarget = true;
            }
        }
    }

    #region Set Init
    private void SetTileSlots()
    {
        TileSlots.Clear();

        for (int i = 0; i < TargetTileMap.childCount; i++)
        {
            TileSlot tileSlot = TargetTileMap.GetChild(i).GetComponent<TileSlot>();

            tileSlot.SetConnectedStorage(this);
            TileSlots.Add(tileSlot);
        }
    }

    private void SetConnectTileSlotsToTiles()
    {
        for (int i = 0; i < TileSlots.Count; i++)
        {
            TileSlot tileSlot = TileSlots[i];

            tileSlot.SetConnectWithTiles();
        }
    }
    public void SetTileSlotCoordinates()
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

    #region Set Init Tile
    private void SetTiles()
    {
        Tiles.Clear();

        for (int i = 0; i < TileSize.x * TileSize.y; i++)
        {
            Tile tile = new Tile();

            tile.SetConnectedStorage(this);
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

    public void ConnectTileSlotsToTiles()
    {
        for (int i = 0; i < TileSize.x * TileSize.y; i++)
        {
            Tiles[i].TileSlot = TileSlots[i];
        }
    }
    #endregion

    #region Controls 

    /// <summary>
    /// Finds first empty area with given size and if is exist return coordinates
    /// </summary>
    public Vector2Int FindEmptyTileArea(Vector2Int itemSize)
    {
        Vector2Int tempCoordinate = new Vector2Int();

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

    /// <summary>
    /// Determine if given coordinates are empty and return boolean
    /// </summary>
    /// <returns></returns>
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
    #endregion

    #region Create
    public ItemBase CreateNewItem(string id)
    {
        return Instantiate(ItemDataBase.Instance.Items.Find(x => x.Id == id));
    }
    
    public void CreateItemSlot(ItemBase item)
    {
        ItemSlot itemSlot = CreateItemSlot(item.Id, item.Coordinat);

        SetItemsSlotSprite(itemSlot);
        SynchTileSlotsInItemSlot(itemSlot, item.Coordinat);
    }
    #endregion

    #region Create Item Slot
    public ItemSlot CreateItemSlot(string id, Vector2Int toCoordinate)
    {
        GameObject newItemPrefab = Instantiate(ItemSlotPrefab, TargetItemSlots);
        ItemSlot newItemSlot = newItemPrefab.GetComponent<ItemSlot>();

        Tile tile = Tiles.Find(x => x.Coordinats.x == toCoordinate.x && x.Coordinats.y == toCoordinate.y);

        newItemSlot.PivotTileSlot = tile;
        newItemSlot.AssignedItem = tile.AssignedItem;
        newItemSlot.AssignedItem.Coordinat = tile.Coordinats;
        newItemSlot.ConnectedStorage = tile.ConnectedStorage;

        newItemPrefab.transform.position = tile.TileSlot.transform.GetChild(0).position;
        newItemPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(100 * newItemSlot.AssignedItem.Size.x, 100 * newItemSlot.AssignedItem.Size.y);

        return newItemSlot;

    }

    public void ReplaceItemSlot(ItemSlot itemSlot, Tile tileSlot)
    {

        itemSlot.PivotTileSlot = tileSlot;
        itemSlot.AssignedItem.Coordinat = tileSlot.Coordinats;
        itemSlot.transform.position = tileSlot.TileSlot.transform.GetChild(0).position;
        itemSlot.ConnectedStorage = tileSlot.ConnectedStorage;
    }
    #endregion

    #region Add Item
    public void AddItem_Auto(string id)
    {
        ItemBase item = CreateNewItem(id);

        Vector2Int toCoordinate = FindEmptyTileArea(item.Size);

        if (toCoordinate != new Vector2Int(-1, -1))
        {
            AddItem_ToCoordinate(item, toCoordinate);
            item.Coordinat = toCoordinate;

            if (_isConnectedToTarget)
            {
                ItemSlot itemSlot = CreateItemSlot(id, toCoordinate);
                SetItemsSlotSprite(itemSlot);

                SynchTileSlotsInItemSlot(itemSlot, toCoordinate);
            }

        }
    }

    public void AddItem_ToCoordinate(ItemBase item, Vector2Int coordinate)
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

        Tiles.Find(x => x.Coordinats.x == coordinate.x && x.Coordinats.y == coordinate.y).ConnectedStorage.Items.Add(item);

    }

    public void SynchTileSlotsInItemSlot(ItemSlot itemSlot, Vector2Int coordinate)
    {
        for (int i = 0; i < itemSlot.AssignedItem.Size.x; i++)
        {
            for (int l = 0; l < itemSlot.AssignedItem.Size.y; l++)
            {
                Tile slot = Tiles.Find(x => x.Coordinats.x == coordinate.x + i && x.Coordinats.y == coordinate.y + l); //currentSlot.ConnectedStorage.ItemSlots.Find(x => x.Coordinats.x == currentSlot.Coordinats.x + i && x.Coordinats.y == currentSlot.Coordinats.y + l);
                itemSlot.ConnectedTileSlots.Add(slot);
            }
        }
    }

  
    #endregion

    #region Remove Item
    public void RemoveItemSlotFromTileSlots(ItemSlot itemSlot)
    {
        for (int i = 0; i < itemSlot.ConnectedTileSlots.Count; i++)
        {
            itemSlot.ConnectedTileSlots[i].AssignedItem = null;
        }
        itemSlot.ConnectedTileSlots.Clear();
        itemSlot.ConnectedStorage.Items.Remove(itemSlot.ConnectedStorage.Items.Find(x => x == itemSlot.AssignedItem));
    }
    #endregion

    #region UI Configuration
    public void SetItemsSlotSprite(ItemSlot itemSlot)
    {
        itemSlot.transform.GetChild(0).GetComponent<Image>().sprite = itemSlot.AssignedItem.Sprite;
    }

    /// <summary>
    /// Finds item slot at coordinate with given size and checks if the fill is empty
    /// </summary>
    /// <param name="itemSize"></param>
    /// <param name="coordinate"></param>
    /// <param name="isMovable"></param>
    public void UpdateTileSlotHighlight(Vector2Int itemSize, Vector2Int coordinate, bool isMovable)
    {
        Vector2Int pivotPosition = coordinate;

        for (int i = 0; i < itemSize.x; i++)
        {
            for (int l = 0; l < itemSize.y; l++)
            {
                Tile slot = Tiles.Find(x => x.Coordinats.x == coordinate.x + i && x.Coordinats.y == coordinate.y + l); //currentSlot.ConnectedStorage.ItemSlots.Find(x => x.Coordinats.x == currentSlot.Coordinats.x + i && x.Coordinats.y == currentSlot.Coordinats.y + l);
                if(slot != null)
                {
                    if (isMovable)
                    {
                        slot.TileSlot.isHighLight = true;
                    }
                    else
                    {
                        slot.TileSlot.isRedLight = true;
                    }
              
                }
               
            }
        }
    }
    #endregion

    #region General
    public void ConnectSelf()
    {
        SetTiles();
        SetTileCoordinates();
    }
    public void ConnectToInventoryPage()
    {
        SetTileSlots();
        SetTileSlotCoordinates();
        ConnectTileSlotsToTiles();
        SetConnectTileSlotsToTiles();
    }

    public void RefreshInventoryPage()
    {
        for (int i = 0; i < TargetItemSlots.childCount; i++)
        {
            Destroy(TargetItemSlots.GetChild(i).gameObject);
        }

        foreach (ItemBase item in Items)
        {
            CreateItemSlot(item);
        }
    }



    #endregion

}
