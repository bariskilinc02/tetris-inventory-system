using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour
{
    public static Controller Instance;

    public ItemBase newItem;

    private TileSlot _currentSlot;
    public ItemSlot CurrentMovingItem;

    private GraphicRaycaster m_Raycaster;
    private PointerEventData m_PointerEventData;
    private EventSystem m_EventSystem;

    public bool isAreaEmpty;
    public bool isItemMoving;

    #region Unity Functions
    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        newItem = new ItemBase("akm");

        m_EventSystem = EventSystem.current;
        m_Raycaster = FindObjectOfType<GraphicRaycaster>();
        m_PointerEventData = new PointerEventData(m_EventSystem);
    }

    void Update()
    {


        if (isItemMoving)
        {
            UpdateMovingItem();

            IsMouseOnSlot();

            IsMouseFinishMovingItem();
        }
        else
        {
            IsMouseStartMoveItem();
        }
    }

    #endregion

    #region Mouse Commands
    private void IsMouseOnSlot()
    {
        List<RaycastResult> results = new List<RaycastResult>();

        m_PointerEventData.position = Input.mousePosition;
        m_Raycaster.Raycast(m_PointerEventData, results);

        if (results.Count > 0)
        {
            if (results[0].gameObject.CompareTag("TileSlot"))
            {
                _currentSlot = results[0].gameObject.GetComponent<TileSlot>();
                isAreaEmpty =_currentSlot.ConnectedStorage.IsTileAreaEmpty(CurrentMovingItem.AssignedItem.Size, _currentSlot.Coordinats);

                if(isAreaEmpty) _currentSlot.ConnectedStorage.UpdateTileSlotHighlight(CurrentMovingItem.AssignedItem.Size, _currentSlot.Coordinats, true);
                else _currentSlot.ConnectedStorage.UpdateTileSlotHighlight(CurrentMovingItem.AssignedItem.Size, _currentSlot.Coordinats, false);

            }
            else
            {
                _currentSlot = null;
            }
        }
    }

    private void IsMouseStartMoveItem()
    {
        if (!Input.GetMouseButtonDown(0) && !isItemMoving) return;

        List<RaycastResult> results = new List<RaycastResult>();

        m_PointerEventData.position = Input.mousePosition;
        m_Raycaster.Raycast(m_PointerEventData, results);

        if (results.Count > 0)
        {
            if (results[0].gameObject.CompareTag("ItemSlot"))
            {
                CurrentMovingItem = results[0].gameObject.GetComponent<ItemSlot>();
                CurrentMovingItem.GetComponent<Image>().raycastTarget = false;
                CurrentMovingItem.ConnectedStorage.RemoveItemSlotFromTileSlots(CurrentMovingItem);

                isItemMoving = true;
            }
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

        List<RaycastResult> results = new List<RaycastResult>();

        m_PointerEventData.position = Input.mousePosition;
        m_Raycaster.Raycast(m_PointerEventData, results);

        if (results.Count > 0)
        {
            if (results[0].gameObject.CompareTag("TileSlot"))
            {
                _currentSlot = results[0].gameObject.GetComponent<TileSlot>();

                if (_currentSlot.ConnectedStorage.IsTileAreaEmpty(CurrentMovingItem.AssignedItem.Size, _currentSlot.Coordinats))//if (IsTileAreaEmptyForItem(CurrentMovingItem.AssignedItem, _currentSlot))
                {
                    SetItemToEmptyArea(CurrentMovingItem.AssignedItem, _currentSlot);

                    _currentSlot.ConnectedStorage.ReplaceItemSlot(CurrentMovingItem, _currentSlot);
                    _currentSlot.ConnectedStorage.SynchTileSlotsInItemSlot(CurrentMovingItem, _currentSlot.Coordinats);

                    CurrentMovingItem.transform.parent = _currentSlot.ConnectedStorage.TargetItemSlots;
                    CurrentMovingItem.GetComponent<Image>().raycastTarget = true;
                    isItemMoving = false;
                }
                else
                {
                    CurrentMovingItem.ConnectedStorage.ReplaceItemSlot(CurrentMovingItem, CurrentMovingItem.PivotTileSlot);
                    CurrentMovingItem.GetComponent<Image>().raycastTarget = true;
                    isItemMoving = false;
                }
            }
            else
            {
                CurrentMovingItem.ConnectedStorage.ReplaceItemSlot(CurrentMovingItem, CurrentMovingItem.PivotTileSlot);
                CurrentMovingItem.GetComponent<Image>().raycastTarget = true;
                isItemMoving = false;
            }
        }
    }
    #endregion

    private bool IsTileAreaEmptyForItem(ItemBase item, TileSlot currentSlot)
    {
        Vector2Int pivotPosition = currentSlot.Coordinats;

        bool isEmpty = true;

        for (int i = 0; i < item.Size.x; i++)
        {
            for (int l = 0; l < item.Size.y; l++)
            {
                TileSlot slot = currentSlot.ConnectedStorage.ItemSlots.Find(x => x.Coordinats.x == currentSlot.Coordinats.x + i && x.Coordinats.y == currentSlot.Coordinats.y + l);
                if (slot == null)
                {
                    isEmpty = false;
                    return isEmpty;
                }
                else if(slot.AssignedItem != null)
                {
                    isEmpty = false;
                    return isEmpty;
                }
            }
        }

        return isEmpty;
    }
    private void SetItemToEmptyArea(ItemBase item, TileSlot currentSlot)
    {
        Vector2Int pivotPosition = currentSlot.Coordinats;

        for (int i = 0; i < item.Size.x; i++)
        {
            for (int l = 0; l < item.Size.y; l++)
            {
                TileSlot slot = currentSlot.ConnectedStorage.ItemSlots.Find(x => x.Coordinats.x == currentSlot.Coordinats.x + i && x.Coordinats.y == currentSlot.Coordinats.y + l);
                slot.AssignedItem = item;
            }
        }

        currentSlot.ConnectedStorage.Items.Add(item);
    }



}
