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
    public List<TileSlot> ItemSlots;

    private bool _isConnectedToTarget;

    public GameObject ItemSlotPrefab;

    void Init()
    {
        _isConnectedToTarget = TargetTileMap == null ? false : true;

        if (_isConnectedToTarget)
        {
            SetTileSlots();
            SetTileSlotCoordinates();
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
       
    }

    #region Set Init
    private void SetTileSlots()
    {
        ItemSlots.Clear();

        for (int i = 0; i < TargetTileMap.childCount; i++)
        {
            TileSlot tileSlot = TargetTileMap.GetChild(i).GetComponent<TileSlot>();

            tileSlot.SetConnectedStorage(this);
            ItemSlots.Add(tileSlot);
        }
    }

    public void SetTileSlotCoordinates()
    {
        int counter = 0;
        for (int i = 0; i < ItemSlots.Count / TileSize.x; i++)
        {
            for (int l = 0; l < ItemSlots.Count / TileSize.y; l++)
            {
                ItemSlots[counter].Coordinats = new Vector2Int(l, i);
                counter += 1;
            }
        }
    }
    #endregion

    #region Controls 
    public Vector2Int FindEmptyTileArea(Vector2Int itemSize)
    {
        Vector2Int tempCoordinate = new Vector2Int();

        for (int a = 0; a < ItemSlots.Count; a++)
        {
            tempCoordinate = ItemSlots[a].Coordinats;

            if (IsTileAreaEmpty(itemSize, tempCoordinate))
            {
                return tempCoordinate;
            }
        }

        return new Vector2Int(-1, -1);
    }
    public bool IsTileAreaEmpty(Vector2Int itemSize, Vector2Int coordinate)
    {
        Vector2Int pivotPosition = coordinate;

        bool isEmpty = true;

        for (int i = 0; i < itemSize.x; i++)
        {
            for (int l = 0; l < itemSize.y; l++)
            {
                TileSlot slot = ItemSlots.Find(x => x.Coordinats.x == coordinate.x + i && x.Coordinats.y == coordinate.y + l);
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

    #region Create Item
    public ItemBase CreateNewItem(string id)
    {
        return new ItemBase(id);
    }
    #endregion

    #region Create Item Slot
    public ItemSlot CreateItemSlot(string id, Vector2Int toCoordinate)
    {
        GameObject newItemPrefab = Instantiate(ItemSlotPrefab, TargetItemSlots);
        ItemSlot newItemSlot = newItemPrefab.GetComponent<ItemSlot>();

        TileSlot tileSlot = ItemSlots.Find(x => x.Coordinats.x == toCoordinate.x && x.Coordinats.y == toCoordinate.y);

        newItemSlot.PivotTileSlot = tileSlot;
        newItemSlot.AssignedItem = tileSlot.AssignedItem;
        newItemSlot.ConnectedStorage = tileSlot.ConnectedStorage;

        newItemPrefab.transform.position = tileSlot.transform.GetChild(0).position;
        newItemPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(100 * newItemSlot.AssignedItem.Size.x, 100 * newItemSlot.AssignedItem.Size.y);

        return newItemSlot;

    }

    public void ReplaceItemSlot(ItemSlot itemSlot, TileSlot tileSlot)
    {

        itemSlot.PivotTileSlot = tileSlot;
        itemSlot.transform.position = tileSlot.transform.GetChild(0).position;
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

            ItemSlot itemSlot = CreateItemSlot(id ,toCoordinate);
            SetItemsSlotSprite(itemSlot);

            SynchTileSlotsInItemSlot(itemSlot, toCoordinate);
        }
    }

    public void AddItem_ToCoordinate(ItemBase item, Vector2Int coordinate)
    {
        Vector2Int pivotPosition = coordinate;

        for (int i = 0; i < item.Size.x; i++)
        {
            for (int l = 0; l < item.Size.y; l++)
            {
                TileSlot slot = ItemSlots.Find(x => x.Coordinats.x == coordinate.x + i && x.Coordinats.y == coordinate.y + l); //currentSlot.ConnectedStorage.ItemSlots.Find(x => x.Coordinats.x == currentSlot.Coordinats.x + i && x.Coordinats.y == currentSlot.Coordinats.y + l);
                slot.AssignedItem = item;
            }
        }

        ItemSlots.Find(x => x.Coordinats.x == coordinate.x && x.Coordinats.y == coordinate.y).ConnectedStorage.Items.Add(item);
        //currentSlot.ConnectedStorage.Items.Add(item);
    }

    public void SynchTileSlotsInItemSlot(ItemSlot itemSlot, Vector2Int coordinate)
    {
        for (int i = 0; i < itemSlot.AssignedItem.Size.x; i++)
        {
            for (int l = 0; l < itemSlot.AssignedItem.Size.y; l++)
            {
                TileSlot slot = ItemSlots.Find(x => x.Coordinats.x == coordinate.x + i && x.Coordinats.y == coordinate.y + l); //currentSlot.ConnectedStorage.ItemSlots.Find(x => x.Coordinats.x == currentSlot.Coordinats.x + i && x.Coordinats.y == currentSlot.Coordinats.y + l);
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
                TileSlot slot = ItemSlots.Find(x => x.Coordinats.x == coordinate.x + i && x.Coordinats.y == coordinate.y + l); //currentSlot.ConnectedStorage.ItemSlots.Find(x => x.Coordinats.x == currentSlot.Coordinats.x + i && x.Coordinats.y == currentSlot.Coordinats.y + l);
                if(slot != null)
                {
                    if (isMovable)
                    {
                        slot.isHighLight = true;
                    }
                    else
                    {
                        slot.isRedLight = true;
                    }
              
                }
               
            }
        }
    }
    #endregion


}
