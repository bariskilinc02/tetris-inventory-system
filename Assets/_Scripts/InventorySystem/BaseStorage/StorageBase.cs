using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageBase : MonoBehaviour
{
    public bool IsFixed;
    public Item ConnectedItem;

    public Transform TargetTileMap;
    public Transform TargetItemSlots;

    public Storage Storage;

    public List<TileSlot> TileSlots;
    public List<Tile> Tiles;

    [SerializeField] private bool _isConnectedToTarget;
    [SerializeField] private bool _isStorageUIUpdated;

    public GameObject ItemSlotPrefab;

    void Init()
    {
        ConnectSelf();

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
        if (Input.GetKeyDown(KeyCode.T))
        {
            AddItem_Auto("chest");
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
        for (int i = 0; i < TileSlots.Count / Storage.TileSize.x; i++)
        {
            for (int l = 0; l < TileSlots.Count / Storage.TileSize.y; l++)
            {
                TileSlots[counter].Coordinats = new Vector2Int(l, i);
                counter += 1;
            }
        }
    }
    
    public void SetConnectItemsToTiles()
    {
        for (int i = 0; i < Storage.Items.Count; i++)
        {
            Tile tile = Tiles.Find(x => x.Coordinats == Storage.Items[i].Coordinat);
            SetItemToEmptyArea(Storage.Items[i], tile);
        }
    }
    #endregion

    #region Set Init Tile
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

    public void ConnectTileSlotsToTiles()
    {
        for (int i = 0; i < Storage.TileSize.x * Storage.TileSize.y; i++)
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

            if (IsEmptyTileArea(itemSize, tempCoordinate))
            {
                return tempCoordinate;
            }
        }

        return new Vector2Int(-1, -1);
    }

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
    /// Determine if given coordinates are empty and return boolean
    /// </summary>
    /// <returns></returns>
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


    #endregion

    #region Create
    public Item CreateNewItem(string id)
    {
        return Instantiate(ItemDataBase.Instance.Items.Find(x => x.Id == id));
    }
    
    public void CreateItemSlot(Item item)
    {
        ItemSlot itemSlot = CreateItemSlot(item.Id, item.Coordinat);

        SetItemsSlotSprite(itemSlot);
        SynchTileSlotsInItemSlot(itemSlot, item.Coordinat);
        if (itemSlot.AssignedItem.Direction) itemSlot.ChangeDirection();
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
        Item item = CreateNewItem(id);

        Vector2Int toCoordinate = FindEmptyTileArea(item.Size);

        if(toCoordinate == new Vector2Int(-1, -1))
        {
            item.ChangeItemDirection();
            toCoordinate = FindEmptyTileArea(item.Size);
        }


        if (toCoordinate != new Vector2Int(-1, -1))
        {
            AddItem_ToCoordinate(item, toCoordinate);
            item.Coordinat = toCoordinate;

            if (_isConnectedToTarget)
            {
                ItemSlot itemSlot = CreateItemSlot(id, toCoordinate);
                SetItemsSlotSprite(itemSlot);

                SynchTileSlotsInItemSlot(itemSlot, toCoordinate);
                itemSlot.ChangeDirection();
            }

        }
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

        Tiles.Find(x => x.Coordinats.x == coordinate.x && x.Coordinats.y == coordinate.y).ConnectedStorage.Storage.Items.Add(item);

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
    public void SynchTileSlotsWithItemInStorage(Item item, Vector2Int coordinate)
    {
        for (int i = 0; i < item.Size.x; i++)
        {
            for (int l = 0; l < item.Size.y; l++)
            {
                Tile slot = Tiles.Find(x => x.Coordinats.x == coordinate.x + i && x.Coordinats.y == coordinate.y + l); //currentSlot.ConnectedStorage.ItemSlots.Find(x => x.Coordinats.x == currentSlot.Coordinats.x + i && x.Coordinats.y == currentSlot.Coordinats.y + l);
                slot.AssignedItem = item;
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
        itemSlot.ConnectedStorage.Storage.Items.Remove(itemSlot.ConnectedStorage.Storage.Items.Find(x => x == itemSlot.AssignedItem));
    }
    

    #endregion

    #region UI Configuration
    public void SetItemsSlotSprite(ItemSlot itemSlot)
    {
        itemSlot.transform.GetChild(0).GetComponent<Image>().sprite = itemSlot.AssignedItem.Sprite;
    }

    public void DeleteItemSlot(ItemSlot itemSlot)
    {
        Destroy(itemSlot.gameObject);
    }

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

        foreach (Item item in Storage.Items)
        {
            SynchTileSlotsWithItemInStorage(item, item.Coordinat);
            CreateItemSlot(item);
        }
    }



    #endregion

    #region Item Direction
    
    #endregion
}
