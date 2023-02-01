using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour
{
    public static Controller Instance;

    private TileSlot _currentTileSlot;
    public ItemSlot CurrentMovingItem;

    private GraphicRaycaster m_Raycaster;
    private PointerEventData m_PointerEventData;
    private EventSystem m_EventSystem;

    [HideInInspector] public bool isAreaEmpty;
     public bool isItemMoving;
    public bool isItemPropertiesShowing;

    [SerializeField] private bool isItemRotated;


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

            IfMouseOnTileSlot();
            OnChangeItemDirection(CurrentMovingItem);

            IsMouseUp();
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
        GameObject InteractedObject = results[0].gameObject;

        if (InteractedObject.CompareTag("ItemSlot"))
        {
            ItemSlot clickedItemSlot = InteractedObject.GetComponent<ItemSlot>();

            OpenStoragePage(clickedItemSlot.AssignedItem);
        }

    }


    private void IfMouseOnTileSlot()
    {
        List<RaycastResult> results  = SendRay();

        if (results.Count < 1) return;

        if (results[0].gameObject.CompareTag("TileSlot"))
        {
            _currentTileSlot = results[0].gameObject.GetComponent<TileSlot>();

            if (CurrentMovingItem.AssignedItem == StoragePageCreator.Instace.CurrentItem)
            {
                isAreaEmpty = false;
            }
            else
            {
                if (_currentTileSlot.ConnectedStorage.isExtended == false)
                {
                    isAreaEmpty = _currentTileSlot.ConnectedStorage.IsEmptyTileArea(CurrentMovingItem.AssignedItem.Size, _currentTileSlot.Coordinates);
                }
                else if (_currentTileSlot.ConnectedStorage.isExtended == true)
                {
                    if (_currentTileSlot.ConnectedStorage.AvailableItemTypes.Exists(x => x == CurrentMovingItem.AssignedItem.ItemType))
                    {
                        isAreaEmpty = _currentTileSlot.ConnectedStorage.IsEmptyTile(_currentTileSlot.Coordinates);
                    }
                    else
                    {
                        isAreaEmpty = false;
                    }
      
                }
                
            }

            if (isAreaEmpty) _currentTileSlot.ConnectedStorage.UpdateTileSlotHighlight(CurrentMovingItem.AssignedItem.Size, _currentTileSlot.Coordinates, true);
            else _currentTileSlot.ConnectedStorage.UpdateTileSlotHighlight(CurrentMovingItem.AssignedItem.Size, _currentTileSlot.Coordinates, false);

        }
        else
        {
            _currentTileSlot = null;
        }

    }

    private void IsMouseStartMoveItem()
    {
        if (!Input.GetMouseButtonDown(0) && !isItemMoving) return;

        isItemRotated = false;

        List<RaycastResult> results = SendRay();

        if (results.Count < 1) return;
        if (results[0].gameObject.CompareTag("ItemSlot"))
        {
            CurrentMovingItem = results[0].gameObject.GetComponent<ItemSlot>();
            CurrentMovingItem.GetComponent<Image>().raycastTarget = false;
            CurrentMovingItem.ConnectedStorage.RemoveItemSlotFromTileSlots(CurrentMovingItem);
            CurrentMovingItem.FixVisual();
            
            isItemMoving = true;
            RestoreItemSlotVisual(CurrentMovingItem, false);
        }
        
     

    }
   
    private void UpdateMovingItem()
    {
        if (!Input.GetMouseButton(0) && !isItemMoving) return;

        CurrentMovingItem.gameObject.transform.position = Input.mousePosition;

    }

    private void IsMouseUp()
    {
        if (!Input.GetMouseButtonUp(0) || !isItemMoving) return;
        
        List<RaycastResult> results = SendRay();
        
        if (results.Count < 1) return;

        if (results[0].gameObject.CompareTag("TileSlot"))
        {
            _currentTileSlot = results[0].gameObject.GetComponent<TileSlot>();

            if(_currentTileSlot.ConnectedTile.AssignedItem != null)
            {
                RestoreItemSlot(CurrentMovingItem);
                return;
            }

            bool isAdded = false;
            
            if (_currentTileSlot.ConnectedStorage is Inventory currentInventory)
            {
                currentInventory.AddItem(CurrentMovingItem, _currentTileSlot, out isAdded);
            }
            
            if (_currentTileSlot.ConnectedStorage is EquipmentStorage currentEquipment)
            {
                currentEquipment.AddItem(CurrentMovingItem, _currentTileSlot, out isAdded);
                if (isAdded)
                {
                    ChangeItemRotation(CurrentMovingItem, false); 
                }
              
            }
            
            if (isAdded)
            {
                CurrentMovingItem.transform.parent = _currentTileSlot.ConnectedStorage.TargetItemSlots;
                CurrentMovingItem.GetComponent<Image>().raycastTarget = true;
                
                isItemRotated = false;
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

            if (currentItem is StorageItem currentStorageItem)
            {
                Storage currentStorage = currentStorageItem.Storage;

                if (currentStorage.IsExistEmptyTileArea(CurrentMovingItem.AssignedItem.Size))
                {
                    CurrentMovingItem.ConnectedStorage.RemoveItemSlotFromTileSlots(CurrentMovingItem);
                    CurrentMovingItem.ConnectedStorage.DeleteItemSlot(CurrentMovingItem);
                    currentStorage.MoveItem_Auto(CurrentMovingItem.AssignedItem);

                    isItemRotated = false;
                    isItemMoving = false;
                }
                else if (currentStorage.IsExistEmptyTileArea(new Vector2Int(CurrentMovingItem.AssignedItem.Size.y, CurrentMovingItem.AssignedItem.Size.x)))
                {
                    CurrentMovingItem.AssignedItem.ChangeItemDirection();
                    CurrentMovingItem.ConnectedStorage.RemoveItemSlotFromTileSlots(CurrentMovingItem);
                    CurrentMovingItem.ConnectedStorage.DeleteItemSlot(CurrentMovingItem);
                    currentStorage.MoveItem_Auto(CurrentMovingItem.AssignedItem);
                    
                    isItemRotated = false;
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

        RestoreItemSlotVisual(CurrentMovingItem);
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
        if (isItemRotated) ChangeItemDirection(itemSlot);
        
        RestoreItemSlotVisual(itemSlot);

        if (itemSlot.ConnectedStorage.isExtended)
        {
            itemSlot.ConnectedStorage.SetItemToEmptyTile(itemSlot.AssignedItem, itemSlot.PivotTileSlot);
            itemSlot.ConnectedStorage.ReplaceItemSlot(itemSlot, itemSlot.PivotTileSlot);
            itemSlot.ConnectedStorage.SynchTileSlotInItemSlot(itemSlot, itemSlot.PivotTileSlot.TileSlot.Coordinates);
        }
        else
        {
            itemSlot.ConnectedStorage.SetItemToEmptyArea(itemSlot.AssignedItem, itemSlot.PivotTileSlot);
            itemSlot.ConnectedStorage.ReplaceItemSlot(itemSlot, itemSlot.PivotTileSlot);
            itemSlot.ConnectedStorage.SynchTileSlotsInItemSlot(itemSlot, itemSlot.PivotTileSlot.TileSlot.Coordinates);
        }

        itemSlot.GetComponent<Image>().raycastTarget = true;
        isItemMoving = false;
    }

    private void ChangeItemDirection(ItemSlot itemSlot)
    {
        itemSlot.AssignedItem.ChangeItemDirection();
        itemSlot.ChangeDirection();
    }

    private void OnChangeItemDirection(ItemSlot itemSlot)
    {
        if (!Input.GetKeyDown(KeyCode.R)) return;
        ChangeItemRotation(itemSlot);

    }

    private void ChangeItemRotation(ItemSlot itemSlot, bool? isOverride = null)
    {
        if (isOverride == null)
        {
            isItemRotated = !isItemRotated;

            itemSlot.AssignedItem.ChangeItemDirection();
            itemSlot.ChangeDirection();
        }
        else
        {
            if (isOverride == true)
            {
                if (!itemSlot.AssignedItem.Direction)
                {
                    itemSlot.AssignedItem.ChangeItemDirection();
                    itemSlot.ChangeDirection();
                }
          
            }
            else
            {
                if (itemSlot.AssignedItem.Direction)
                {
                    itemSlot.AssignedItem.ChangeItemDirection();
                    itemSlot.ChangeDirection();
                }
            }
        }
        

    }

    /// <summary>
    /// Check if Item is Storage Item and Create a Storage Page
    /// </summary>
    private void OpenStoragePage(Item item)
    {
        if (item is StorageItem)
        {
            StoragePageCreator.Instace.Create(item);
        }
    }

    private void RestoreItemSlotVisual(ItemSlot itemSlot, bool? isOverride = null)
    {
        if (isOverride != null)
        {
            if (isOverride == true)
            {
                itemSlot.FitVisual(itemSlot.PivotTileSlot.TileSlot);
            }
            else 
            {
                itemSlot.FixVisual();
            }
        }
        else
        {
            if (itemSlot.ConnectedStorage is Inventory currentInventory)
            {
                itemSlot.FixVisual();
            }
            else if (itemSlot.ConnectedStorage is EquipmentStorage currentEquipment)
            {
                itemSlot.FitVisual(itemSlot.PivotTileSlot.TileSlot);
            }
            else
            {
                itemSlot.FixVisual();
            }
        }
        
      
    }
}
