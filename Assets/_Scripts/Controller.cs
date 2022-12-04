using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour
{
    public static Controller Instance;

    private TileSlot _currentSlot;
    public ItemSlot CurrentMovingItem;

    private GraphicRaycaster m_Raycaster;
    private PointerEventData m_PointerEventData;
    private EventSystem m_EventSystem;

    [HideInInspector] public bool isAreaEmpty;
    [HideInInspector] public bool isItemMoving;
    public bool isItemPropertiesShowing;


    #region Unity Functions
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        m_EventSystem = EventSystem.current;
        m_Raycaster = FindObjectOfType<GraphicRaycaster>();
        m_PointerEventData = new PointerEventData(m_EventSystem);
    }

    void Update()
    {


        if (isItemMoving)
        {
            UpdateMovingItem();

            OnMouseTileSlot();

            IsMouseFinishMovingItem();
        }
        else
        {
            IsMouseStartMoveItem();

            IsMouseRightClick();
        }

     
    }

    #endregion

    #region Mouse Commands
    private void IsMouseRightClick()
    {
        if (!Input.GetMouseButtonDown(1)) return;

        List <RaycastResult> results = SendRay();

        if (results.Count < 1) return;
        if (results[0].gameObject.CompareTag("ItemSlot"))
        {
            ItemSlot clickedItemSlot = results[0].gameObject.GetComponent<ItemSlot>();
            if (clickedItemSlot.AssignedItem.GetType() == typeof(StorageItem))
            {

                StorageItem currentStorageItem = (StorageItem)clickedItemSlot.AssignedItem;
                Storage currentStorage = currentStorageItem.Storage;

                StoragePageCreator.Instace.Create(clickedItemSlot.AssignedItem);
            }
        }

    }


    private void OnMouseTileSlot()
    {
        List<RaycastResult> results  = SendRay();

        if (results.Count < 1) return;

        if (results[0].gameObject.CompareTag("TileSlot"))
        {
            _currentSlot = results[0].gameObject.GetComponent<TileSlot>();

            //if (CurrentMovingItem.AssignedItem.GetType() == typeof(StorageItem))
            //{
            //    StorageItem Item = (StorageItem)CurrentMovingItem.AssignedItem;
            //    Item.Storage.Items.Exists(x => x == StoragePageCreator.Instace.CurrentItem);
            //}

            if (CurrentMovingItem.AssignedItem == StoragePageCreator.Instace.CurrentItem)
            {
                isAreaEmpty = false;
            }
            else
            {
                isAreaEmpty = _currentSlot.ConnectedStorage.IsTileAreaEmpty(CurrentMovingItem.AssignedItem.Size, _currentSlot.Coordinats);
            }



            if (isAreaEmpty) _currentSlot.ConnectedStorage.UpdateTileSlotHighlight(CurrentMovingItem.AssignedItem.Size, _currentSlot.Coordinats, true);
            else _currentSlot.ConnectedStorage.UpdateTileSlotHighlight(CurrentMovingItem.AssignedItem.Size, _currentSlot.Coordinats, false);

        }
        else
        {
            _currentSlot = null;
        }

    }

    private void IsMouseStartMoveItem()
    {
        if (!Input.GetMouseButtonDown(0) && !isItemMoving) return;

        List<RaycastResult> results = SendRay();

        if (results.Count < 1) return;
        if (results[0].gameObject.CompareTag("ItemSlot"))
        {
            CurrentMovingItem = results[0].gameObject.GetComponent<ItemSlot>();
            CurrentMovingItem.GetComponent<Image>().raycastTarget = false;
            CurrentMovingItem.ConnectedStorage.RemoveItemSlotFromTileSlots(CurrentMovingItem);

            isItemMoving = true;
        }

    }
   
    private void UpdateMovingItem()
    {
        if (!Input.GetMouseButton(0) && !isItemMoving) return;

        CurrentMovingItem.gameObject.transform.position = Input.mousePosition;

    }

    private void IsMouseFinishMovingItem()
    {
        if (!Input.GetMouseButtonUp(0) || !isItemMoving) return;

        List<RaycastResult> results = SendRay();

        if (results.Count < 1) return;

        if (results[0].gameObject.CompareTag("TileSlot"))
        {
            _currentSlot = results[0].gameObject.GetComponent<TileSlot>();

            if (CurrentMovingItem.AssignedItem == StoragePageCreator.Instace.CurrentItem)
            {
                RestoreItemSlot(CurrentMovingItem);
            }
            else if (_currentSlot.ConnectedStorage.IsTileAreaEmpty(CurrentMovingItem.AssignedItem.Size, _currentSlot.Coordinats))//if (IsTileAreaEmptyForItem(CurrentMovingItem.AssignedItem, _currentSlot))
            {
                _currentSlot.ConnectedStorage.SetItemToEmptyArea(CurrentMovingItem.AssignedItem, _currentSlot.ConnectedTile);

                _currentSlot.ConnectedStorage.ReplaceItemSlot(CurrentMovingItem, _currentSlot.ConnectedTile);
                _currentSlot.ConnectedStorage.SynchTileSlotsInItemSlot(CurrentMovingItem, _currentSlot.Coordinats);

                CurrentMovingItem.transform.parent = _currentSlot.ConnectedStorage.TargetItemSlots;
                CurrentMovingItem.GetComponent<Image>().raycastTarget = true;
                isItemMoving = false;
            }
            else
            {
                RestoreItemSlot(CurrentMovingItem);
            }
        }
        else if (results[0].gameObject.CompareTag("ItemSlot"))
        {
            ItemSlot currentItemSlot = results[0].gameObject.GetComponent<ItemSlot>();
            Item currentItem = currentItemSlot.AssignedItem;

            if (currentItem.IsStorageItem())
            {
                StorageItem currentStorageItem = (StorageItem)currentItem;
                Storage currentStorage = currentStorageItem.Storage;

                if (currentStorage.FindEmptyTileArea(CurrentMovingItem.AssignedItem.Size) != new Vector2Int(-1, -1))
                {
                    CurrentMovingItem.ConnectedStorage.RemoveItemSlotFromTileSlots(CurrentMovingItem);
                    CurrentMovingItem.ConnectedStorage.DeleteItemSlot(CurrentMovingItem);
                    currentStorage.MoveItem_Auto(CurrentMovingItem.AssignedItem);

                    isItemMoving = false;
                }
                else
                {
                    RestoreItemSlot(CurrentMovingItem);
                }

                if (currentItem == StoragePageCreator.Instace.CurrentItem)
                {
                    StoragePageCreator.Instace.CurrentInventory.RefreshInventoryPage();
                }
            }
            else
            {
                RestoreItemSlot(CurrentMovingItem);
            }
           
        }
        else
        {
            RestoreItemSlot(CurrentMovingItem);
        }

    }
    #endregion

    private List<RaycastResult> SendRay()
    {
        List<RaycastResult> results = new List<RaycastResult>();

        m_PointerEventData.position = Input.mousePosition;
        m_Raycaster.Raycast(m_PointerEventData, results);

        return results;
    }


    private void RestoreItemSlot(ItemSlot itemSlot)
    {
        itemSlot.ConnectedStorage.SetItemToEmptyArea(itemSlot.AssignedItem, itemSlot.PivotTileSlot);

        itemSlot.ConnectedStorage.ReplaceItemSlot(itemSlot, itemSlot.PivotTileSlot);
        itemSlot.ConnectedStorage.SynchTileSlotsInItemSlot(itemSlot, itemSlot.PivotTileSlot.TileSlot.Coordinats);

        itemSlot.GetComponent<Image>().raycastTarget = true;
        isItemMoving = false;
    }
}
