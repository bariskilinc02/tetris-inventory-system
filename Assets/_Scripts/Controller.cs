using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour
{
    public static Controller Instance;

    public GameObject Canvas;
    
    private TileSlot _currentTileSlot;
    private SingleTileSlot currentSingleTileSlot;
    public ItemSlot CurrentMovingItem;

    [SerializeField] private GraphicRaycaster m_Raycaster;
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
        m_Raycaster = GameObject.FindWithTag("MainCanvas").GetComponent<GraphicRaycaster>();
        m_PointerEventData = new PointerEventData(m_EventSystem);
    }

    void Update()
    {
        if (isItemMoving)
        {
            UpdateMovingItem();

            IfMouseOnSlot();
            OnChangeItemDirection(CurrentMovingItem);

            IsMouseEndUp();
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
        else
        {
            RightClickMenu.Instance.CloseMenu();
        }

    }

    private void IsMouseStartMoveItem()
    {
        if (!Input.GetMouseButtonDown(0) && !isItemMoving) return;

        isItemRotated = false;

        List<RaycastResult> results = SendRay();

        GameObject onMouseObjectGameObject = results[0].gameObject;

        if (results.Count < 1) return;
        if (onMouseObjectGameObject.GetComponent<ItemSlot>() != null)
        {
            CurrentMovingItem = onMouseObjectGameObject.GetComponent<ItemSlot>();

            if (CurrentMovingItem is TraderItemSlot)
            {
                TradeManager.Instance.Buy(CurrentMovingItem);
                RightClickMenu.Instance.CloseMenu();
            }
            else
            {
                CurrentMovingItem.GetComponent<Image>().raycastTarget = false;

            
                if (CurrentMovingItem.ConnectedStorage == null)
                {
                    CurrentMovingItem.ConnectedSingleTileSlot.ConnectedSubItem.ModItem = null;
                }
                else
                {
                    CurrentMovingItem.ConnectedStorage.RemoveItemSlotFromTileSlots(CurrentMovingItem);
                }
     
                CurrentMovingItem.FixVisual();
            
                isItemMoving = true;
                CurrentMovingItem.transform.parent = Canvas.transform;
                RestoreItemSlotVisual(CurrentMovingItem, false);
            
                RightClickMenu.Instance.CloseMenu();
            }
        }
        else
        {
            RightClickMenu.Instance.CloseMenu();
        }
    }

    //Update on mouse slots if item moving 
    private void IfMouseOnSlot()
    {
        List<RaycastResult> results  = SendRay();

        if (results.Count < 1) return;

        GameObject onMouseObjectGameObject = results[0].gameObject;
        
        //Tile Slot
        if (results[0].gameObject.CompareTag("TileSlot"))
        {
            _currentTileSlot = results[0].gameObject.GetComponent<TileSlot>();

            if (_currentTileSlot.ConnectedStorage.IsAvailableItem(CurrentMovingItem.AssignedItem))
            {
                if (CurrentMovingItem.AssignedItem is StorageItem storageItem && storageItem.IsSubItem(_currentTileSlot.ConnectedStorage.ConnectedItem))
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
            }
            else
            {
                isAreaEmpty = false;
            }
            

            if (isAreaEmpty) _currentTileSlot.ConnectedStorage.UpdateTileSlotHighlight(CurrentMovingItem.AssignedItem.Size, _currentTileSlot.Coordinates, true);
            else _currentTileSlot.ConnectedStorage.UpdateTileSlotHighlight(CurrentMovingItem.AssignedItem.Size, _currentTileSlot.Coordinates, false);

        }
        //Single Tile Slot
        else if (results[0].gameObject.CompareTag("SingleTileSlot"))
        {
            currentSingleTileSlot = results[0].gameObject.GetComponent<SingleTileSlot>();

            if (currentSingleTileSlot.ConnectedSubItem.IsAvailable(CurrentMovingItem.AssignedItem, currentSingleTileSlot.ConnectedParentItem))
            {
                currentSingleTileSlot.isHighlight = true;
            }
            else
            {
                currentSingleTileSlot.isRedLight = true;
            }
          
          
        }
        //Item Slot
        else if (onMouseObjectGameObject.GetComponent<ItemSlot>() != null)
        {
            
            ItemSlot currentItemSlot = results[0].gameObject.GetComponent<ItemSlot>();

            if (currentItemSlot is TraderItemSlot)
            {
                
            }
            else
            {
                if (currentItemSlot.AssignedItem is StorageItem storageItem)
                {
                    if (!storageItem.isAvailable(CurrentMovingItem.AssignedItem))
                    {
                        currentItemSlot.RedLightItem();
                    }
                    else
                    {
                        if (CurrentMovingItem.AssignedItem is StorageItem _storageItem && _storageItem.IsSubItem(currentItemSlot.AssignedItem) )
                        {
                            currentItemSlot.RedLightItem();
                        }
                        else if ( storageItem.Storage.IsExistEmptyTileArea(CurrentMovingItem.AssignedItem.Size) ||
                                  storageItem.Storage.IsExistEmptyTileArea(new Vector2Int(CurrentMovingItem.AssignedItem.Size.y, CurrentMovingItem.AssignedItem.Size.x )) )
                        {
                            currentItemSlot.HighlightItem();
                        }
                    }
                }
                else if (currentItemSlot.AssignedItem is WeaponItem weaponItem)
                {
                    if (weaponItem.CheckItemIsCompatible(CurrentMovingItem.AssignedItem,out SubModItem subModItem))
                    {
                        currentItemSlot.HighlightItem();
                    }
             
                }
                else if (currentItemSlot.AssignedItem is ModItem modItem)
                {
                    if (modItem.CheckItemIsCompatible(CurrentMovingItem.AssignedItem,out SubModItem subModItem))
                    {
                        currentItemSlot.HighlightItem();
                    }
             
                }
            }
            
           
        }
        else
        {
            _currentTileSlot = null;
        }

    }
    private void UpdateMovingItem()
    {
        if (!Input.GetMouseButton(0) && !isItemMoving) return;

        CurrentMovingItem.gameObject.transform.position = Input.mousePosition;

    }

    //Trigger when item stop moving
    private void IsMouseEndUp()
    {
        if (!Input.GetMouseButtonUp(0) || !isItemMoving) return;
        
        List<RaycastResult> results = SendRay();
        
        if (results.Count < 1) return;
            
        GameObject onMouseObjectGameObject = results[0].gameObject;
        
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
        else if (results[0].gameObject.CompareTag("SingleTileSlot"))
        {
            currentSingleTileSlot = results[0].gameObject.GetComponent<SingleTileSlot>();
            
            if (currentSingleTileSlot.ConnectedSubItem.IsAvailable(CurrentMovingItem.AssignedItem, currentSingleTileSlot.ConnectedParentItem))
            {
                currentSingleTileSlot.ConnectedSubItem.ModItem = CurrentMovingItem.AssignedItem;

                CurrentMovingItem.ConnectToSingleTileSlot(currentSingleTileSlot);
                CurrentMovingItem.FitVisualSingleTileSlot(currentSingleTileSlot);

                isItemMoving = false;
                CurrentMovingItem.GetComponent<Image>().raycastTarget = true;
            }
            else
            {
                RestoreItemSlot(CurrentMovingItem);
            }
        }
        else if (onMouseObjectGameObject.GetComponent<ItemSlot>() != null)//taşınan item mod parçası ise hata alınır. SingleTileSlot için koşul sağla
        {
            ItemSlot currentItemSlot = results[0].gameObject.GetComponent<ItemSlot>();
            Item currentItem = currentItemSlot.AssignedItem;
                
            if (currentItem is StorageItem currentStorageItem)
            {
                Storage currentStorage = currentStorageItem.Storage;

                if (currentStorageItem.isAvailable(CurrentMovingItem.AssignedItem))
                {
                    if (CurrentMovingItem.AssignedItem is StorageItem storageItem  && storageItem.IsSubItem(currentItemSlot.AssignedItem))
                    {
                        RestoreItemSlot(CurrentMovingItem);
                    }
                    else
                    {
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
                    }
                }
                else
                {
                    RestoreItemSlot(CurrentMovingItem);
                }
               
                

                if (StoragePageCreator.Instance.storageScreens.Exists(x => x.CurrentItem == currentItem))
                {
                    StoragePageCreator.Instance.storageScreens.Find(x => x.CurrentItem == currentItem).CurrentInventory.RefreshInventoryPage();
                }
            }
            else if (currentItem is WeaponItem currentWeaponItem)
            {
                if (currentWeaponItem.CheckItemIsCompatible(CurrentMovingItem.AssignedItem, out SubModItem subModItem))
                {
                    
                    subModItem.ModItem = CurrentMovingItem.AssignedItem;
                    CurrentMovingItem.ConnectedSingleTileSlot = currentSingleTileSlot;
                    CurrentMovingItem.ConnectedStorage = null;
                    Destroy(CurrentMovingItem.gameObject); 
                    isItemMoving = false;
                }
                else
                {
                    RestoreItemSlot(CurrentMovingItem);
                }
            
            }
            else if (currentItem is ModItem modItem and not MagazineItem and not BulletItem)
            {
                if (modItem.CheckItemIsCompatible(CurrentMovingItem.AssignedItem, out SubModItem subModItem))
                {
                    subModItem.ModItem = CurrentMovingItem.AssignedItem;
                    CurrentMovingItem.ConnectedSingleTileSlot = currentSingleTileSlot;
                    CurrentMovingItem.ConnectedStorage = null;
                    Destroy(CurrentMovingItem.gameObject); 
                    isItemMoving = false;
                }
                else
                {
                    RestoreItemSlot(CurrentMovingItem);
                }
            }
            else if (currentItem is MagazineItem magazineItem && CurrentMovingItem.AssignedItem is BulletItem bulletItem)
            {
                magazineItem.LoadAllBullet(bulletItem);
                RestoreItemSlot(CurrentMovingItem);
            }
            else if (currentItem is BulletItem targetBulletItem && CurrentMovingItem.AssignedItem is BulletItem currentBulletItem)
            {
                targetBulletItem.CombineBullet(currentBulletItem);
                RestoreItemSlot(CurrentMovingItem);
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

        if (itemSlot.ConnectedStorage == null)
        {
            itemSlot.transform.position = itemSlot.ConnectedSingleTileSlot.transform.GetChild(0).transform.position;
        }
        else
        {
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
        RightClickMenu.Instance.OpenMenu(item);
        //if (item is StorageItem or BagItem)
        //{
        //    StoragePageCreator.Instance.Create(item);
        //}
//
        //if (item is WeaponItem)
        //{
        //    PropertyPageManager.Instance.Create(item);
        //}
        //
        //if (item is ModItem modItem)
        //{
        //   // if(modItem.SubModItems.Count == 0) return;
        //    
        //    PropertyPageManager.Instance.Create(item);
        //}
    }

    private void RestoreItemSlotVisual(ItemSlot itemSlot, bool? isOverride = null)
    {
        if (isOverride != null)
        {
            if (isOverride == true)
            {
                itemSlot.FitVisualTileSlot(itemSlot.PivotTileSlot.TileSlot);
            }
            else 
            {
                itemSlot.FixVisual();
            }
        }
        else
        {
            if (itemSlot.ConnectedStorage == null && itemSlot.ConnectedSingleTileSlot != null)
            {
                itemSlot.FitVisualSingleTileSlot(itemSlot.ConnectedSingleTileSlot);
            }
            else if (itemSlot.ConnectedStorage is Inventory currentInventory)
            {
                itemSlot.FixVisual();
            }
            else if (itemSlot.ConnectedStorage is EquipmentStorage currentEquipment)
            {
                itemSlot.FitVisualTileSlot(itemSlot.PivotTileSlot.TileSlot);
            }
            else
            {
                itemSlot.FixVisual();
            }
        }
        
      
    }
}
