using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageBase : MonoBehaviour
{
    public bool IsFixed;
    public bool isExtended;
    public List<Types.ItemType> AvailableItemTypes;
    [HideInInspector] public Item ConnectedItem;

    public Transform TargetTileMap;
    public Transform TargetItemSlots;

    public Storage Storage;

    [HideInInspector] public List<TileSlot> TileSlots;
    [HideInInspector] public List<Tile> Tiles;

    [HideInInspector] private bool _isConnectedToTarget;

    public GameObject ItemSlotPrefab;

    void Init()
    {
        SetupTiles();

        _isConnectedToTarget = TargetTileMap == null ? false : true;

        if (_isConnectedToTarget)
        {
            ConnectToInventoryPage();
            RefreshInventoryPage();
            //SetConnectItemsToTiles(); 
            //RefreshInventoryPage();
        }
    }

    private void Awake()
    {
        if (!IsFixed) return;
        Init();
    }

    private void Start()
    {
        //AddItem_Auto("akm");
        //AddItem_Auto("glock17");
        //AddItem_Auto("chest");
    }

    private void Update()
    {
        
        if (_isConnectedToTarget == false)
        {
            if (TargetTileMap != null)
            {
                ConnectToInventoryPage();
                RefreshInventoryPage();
                _isConnectedToTarget = true;
            }
        }

        Storage.ControlStorage();
    }
    
    
    #region OnAwake
    /// <summary>
    /// Setup tiles linked to inventory
    /// </summary>
    private void SetupTiles()
    {
        SetTiles();
        SetTileCoordinates();
    }

    /// <summary>
    /// Connect Storage with Inventory UI
    /// </summary>
    private void ConnectToInventoryPage()
    {
        PullTileSlots();
        SetupTileSlotCoordinates();
        SetTileSlotsToTiles();
        SetConnectTileSlotsToTiles();
    }

    #region Init Tile Slots
    private void PullTileSlots()
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

            tileSlot.ConnectToTile();
        }
    }
    private void SetupTileSlotCoordinates()
    {
        int counter = 0;
        for (int i = 0; i < TileSlots.Count / Storage.TileSize.x; i++)
        {
            for (int l = 0; l < TileSlots.Count / Storage.TileSize.y; l++)
            {
                TileSlots[counter].Coordinates = new Vector2Int(l, i);
                counter += 1;
            }
        }
    }

    #endregion

    #region Init Tiles
    private void SetTiles()
    {
        Tiles.Clear();

        for (int i = 0; i < Storage.TileSize.x * Storage.TileSize.y; i++)
        {
            Tile tile = new Tile();

            tile.SetConnectedStorage(this);
            Tiles.Add(tile);
        }
    }

    public void SetTileCoordinates()
    {
        int counter = 0;
        for (int i = 0; i < Tiles.Count / Storage.TileSize.x; i++)
        {
            for (int l = 0; l < Tiles.Count / Storage.TileSize.y; l++)
            {
                Tiles[counter].Coordinats = new Vector2Int(l, i);
                counter += 1;
            }
        }
    }

    private void SetTileSlotsToTiles()
    {
        for (int i = 0; i < Storage.TileSize.x * Storage.TileSize.y; i++)
        {
            Tiles[i].TileSlot = TileSlots[i];
        }
    }
    #endregion


    #endregion

    #region General

    /// <summary>
    /// Refresh All Inventery UI and Recreate Item Slots
    /// </summary>
    public void RefreshInventoryPage()
    {
        for (int i = 0; i < TargetItemSlots.childCount; i++)
        {
            Destroy(TargetItemSlots.GetChild(i).gameObject);
        }

        foreach (Item item in Storage.Items)
        {
           // if (this is Inventory storage)
           // {
           //     AssingItemToTile(item, item.Size, item.Coordinate);
           // }
           // 
           // if (this is EquipmentStorage equipmentStorage)
           // {
           //     AssingItemToTile(item, new Vector2Int(1, 1), item.Coordinate);
           // }
            
            
            if (isExtended)
                AssingItemToTile(item, new Vector2Int(1, 1), item.Coordinate);
            else
                AssingItemToTile(item, item.Size, item.Coordinate);

            CreateItemSlotOnUI(item);
        }
    }

    #region Deletes
    /// <summary>
    /// Delete Specified Item Slot
    /// </summary>
    public void DeleteItemSlot(ItemSlot itemSlot)
    {
        Destroy(itemSlot.gameObject);
    }

    /// <summary>
    /// Remove All Connections on Item Slot about Tiles
    /// </summary>
    /// <param name="itemSlot"></param>
    public void RemoveItemSlotFromTileSlots(ItemSlot itemSlot)
    {
        for (int i = 0; i < itemSlot.ConnectedTileSlots.Count; i++)
        {
            itemSlot.ConnectedTileSlots[i].AssignedItem = null;
        }
        itemSlot.ConnectedTileSlots.Clear();
        itemSlot.ConnectedStorage.Storage.Items.Remove(itemSlot.ConnectedStorage.Storage.Items.Find(x => x == itemSlot.AssignedItem));
    }
    #endregion

    /// <summary>
    /// Connect the Item to Tile Area in this Inventory
    /// </summary>
    public void SetItemToEmptyArea(Item item, Tile currentSlot)
    {
        Vector2Int pivotPosition = currentSlot.Coordinats;

        for (int i = 0; i < item.Size.x; i++)
        {
            for (int l = 0; l < item.Size.y; l++)
            {
                Tile tile = currentSlot.ConnectedStorage.Tiles.Find(x => x.Coordinats.x == currentSlot.Coordinats.x + i && x.Coordinats.y == currentSlot.Coordinats.y + l);
                tile.AssignedItem = item;
            }
        }

        currentSlot.ConnectedStorage.Storage.Items.Add(item);
    }

    /// <summary>
    /// Connect the Item to the Specified Tile in this Inventory
    /// </summary>
    public void SetItemToEmptyTile(Item item, Tile currentSlot)
    {
        Vector2Int pivotPosition = currentSlot.Coordinats;

        for (int i = 0; i < 1; i++)
        {
            for (int l = 0; l < 1; l++)
            {
                Tile tile = currentSlot.ConnectedStorage.Tiles.Find(x => x.Coordinats.x == currentSlot.Coordinats.x + i && x.Coordinats.y == currentSlot.Coordinats.y + l);
                tile.AssignedItem = item;
            }
        }

        currentSlot.ConnectedStorage.Storage.Items.Add(item);
    }

    /// <summary>
    /// Reassing Item Slot
    /// </summary>
    public void ReplaceItemSlot(ItemSlot itemSlot, Tile tileSlot)
    {
        itemSlot.PivotTileSlot = tileSlot;
        itemSlot.AssignedItem.Coordinate = tileSlot.Coordinats;
        itemSlot.transform.position = tileSlot.TileSlot.transform.GetChild(0).position;
        itemSlot.ConnectedStorage = tileSlot.ConnectedStorage;
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
                if (slot != null)
                {
                    if (isMovable)
                    {
                        slot.TileSlot.isHighlight = true;
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

    #region Controls 

    /// <summary>
    ///Finds first empty area with given size and if is exist return coordinates
    /// </summary>
    public Vector2Int FindEmptyTileArea(Vector2Int itemSize)
    {
        Vector2Int tempCoordinate = new Vector2Int();

        for (int a = 0; a < Tiles.Count; a++)
        {
            tempCoordinate = Tiles[a].Coordinats;

            if (IsEmptyTileArea(itemSize, tempCoordinate))
            {
                return tempCoordinate;
            }
        }

        return new Vector2Int(-1, -1);
    }

    /// <summary>
    ///Return a bool, is Exist a Tile Area in this Inventory
    /// </summary>
    public bool IsExistEmptyTileArea(Vector2Int itemSize)
    {
        Vector2Int tempCoordinate = new Vector2Int();

        for (int a = 0; a < Tiles.Count; a++)
        {
            tempCoordinate = Tiles[a].Coordinats;

            if (IsEmptyTileArea(itemSize, tempCoordinate))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///Return a bool, is Tile Area is Empty in given Coordinate
    /// </summary>
    public bool IsEmptyTileArea(Vector2Int itemSize, Vector2Int coordinate)
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

    public bool IsEmptyTile(Vector2Int coordinate)
    {
        Vector2Int pivotPosition = coordinate;

        bool isEmpty = true;

        Tile slot = Tiles.Find(x => x.Coordinats.x == coordinate.x && x.Coordinats.y == coordinate.y);

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

        return isEmpty;
    }

    #endregion

    #region Create
    /// <summary>
    ///Create a Item Slot for Item on UI
    /// </summary>
    private void CreateItemSlotOnUI(Item item)
    {
        ItemSlot itemSlot = CreateItemSlot(item.Id, item.Coordinate);

        itemSlot.UpdateImage();
        if (isExtended)
        {
            SynchTileSlotInItemSlot(itemSlot, item.Coordinate);
        }
        else
        {
            SynchTileSlotsInItemSlot(itemSlot, item.Coordinate);
        }
       
        if (itemSlot.AssignedItem.Direction) itemSlot.ChangeDirection();
    }
    #endregion

    #region Create Item Slot


    /// <summary>
    ///Create a Item Slot and Build it.
    /// </summary>
    private ItemSlot CreateItemSlot(string id, Vector2Int toCoordinate)
    {
        GameObject newItemPrefab = Instantiate(ItemSlotPrefab, TargetItemSlots);
        ItemSlot newItemSlot = newItemPrefab.GetComponent<ItemSlot>();

        Tile tile = Tiles.Find(x => x.Coordinats.x == toCoordinate.x && x.Coordinats.y == toCoordinate.y);

        newItemSlot.PivotTileSlot = tile;
        newItemSlot.AssignedItem = tile.AssignedItem;
        newItemSlot.AssignedItem.Coordinate = tile.Coordinats;
        newItemSlot.ConnectedStorage = tile.ConnectedStorage;

        newItemPrefab.transform.position = tile.TileSlot.transform.GetChild(0).position;
        newItemPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(100 * newItemSlot.AssignedItem.Size.x, 100 * newItemSlot.AssignedItem.Size.y);

        return newItemSlot;

    }


    #endregion

    #region Add Item
    public void AddItem_Auto(string id)
    {
        Item item = ItemBehaviour.CreateNewItem(id);

        Vector2Int toCoordinate = FindEmptyTileArea(item.Size);

        if(toCoordinate == new Vector2Int(-1, -1))
        {
            item.ChangeItemDirection();
            toCoordinate = FindEmptyTileArea(item.Size);
        }


        if (toCoordinate != new Vector2Int(-1, -1))
        {
            AddItem_ToCoordinate(item, toCoordinate);
            item.Coordinate = toCoordinate;

            if (_isConnectedToTarget)
            {
                ItemSlot itemSlot = CreateItemSlot(id, toCoordinate);
                itemSlot.UpdateImage();

                SynchTileSlotsInItemSlot(itemSlot, toCoordinate);
                itemSlot.ChangeDirection();
            }

        }
    }

    private void AddItem_ToCoordinate(Item item, Vector2Int coordinate)
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

        Tiles.Find(x => x.Coordinats.x == coordinate.x && x.Coordinats.y == coordinate.y).ConnectedStorage.Storage.Items.Add(item);

    }

    /// <summary>
    /// Assing Connected Tiles to Specified Item Slot in Specified Coordinates
    /// </summary>
    /// <param name="itemSlot"></param>
    /// <param name="coordinate"></param>
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

    /// <summary>
    /// Assing Only 1 Connected Tile to Specified Item Slot in Specified Coordinate
    /// </summary>
    /// <param name="itemSlot"></param>
    /// <param name="coordinate"></param>
    public void SynchTileSlotInItemSlot(ItemSlot itemSlot, Vector2Int coordinate)
    {
        Tile slot = Tiles.Find(x => x.Coordinats.x == coordinate.x && x.Coordinats.y == coordinate.y);
        itemSlot.ConnectedTileSlots.Add(slot);
    }
    private void AssingItemToTile(Item item, Vector2Int size, Vector2Int coordinate)
    {
        for (int i = 0; i < size.x; i++)
        {
            for (int l = 0; l < size.y; l++)
            {
                Tile slot = Tiles.Find(x => x.Coordinats.x == coordinate.x + i && x.Coordinats.y == coordinate.y + l); //currentSlot.ConnectedStorage.ItemSlots.Find(x => x.Coordinats.x == currentSlot.Coordinats.x + i && x.Coordinats.y == currentSlot.Coordinats.y + l);
                slot.AssignedItem = item;
            }
        }
    }
    #endregion

  

    #region UI Configuration
    private void SetItemsSlotSprite(ItemSlot itemSlot)
    {
        itemSlot.transform.GetChild(0).GetComponent<Image>().sprite = itemSlot.AssignedItem.Sprite;
    }


    #endregion

}
