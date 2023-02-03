using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private ItemSlotProperties ItemSlotProperties;
    [SerializeField] private Image BackGroundImage;
    
    public SingleTileSlot ConnectedSingleTileSlot;

    public StorageBase ConnectedStorage;
    public Item AssignedItem;
    public Tile PivotTileSlot;

    public List<Tile> ConnectedTileSlots;

    [HideInInspector] public RectTransform Rect;
    [HideInInspector] public RectTransform SpriteRect;

    private Types.HighlightType HighlightType;
    
    [HideInInspector] public bool isVisualUpdated;
    [HideInInspector] public bool isHighlight;
    [HideInInspector] public bool isRedLight;

    private void Awake()
    {
        Rect = GetComponent<RectTransform>();
        SpriteRect = transform.GetChild(0).GetComponent<RectTransform>();
    }

    private void Update()
    {
        ControlItem();
        UpdateMaterial();

        // if (isHighlight) UpdateColor(Color.green);
        // else if (isRedLight) UpdateColor(Color.red);
        // else UpdateColor(Color.black);
    }

    public void ChangeDirection()
    {
        Rect.sizeDelta = new Vector2(AssignedItem.Size.x * 100, AssignedItem.Size.y * 100);

        SpriteRect.rotation = AssignedItem.Direction ? Quaternion.Euler(0,0,90) : Quaternion.Euler(0, 0, 0);
        //SpriteRect.localScale = AssignedItem.Direction ? new Vector3((float)AssignedItem.Size.y / (float)AssignedItem.Size.x, (float)AssignedItem.Size.x / (float)AssignedItem.Size.y, 1) : new Vector3(1, 1, 1);
    }

    public void FitVisual(TileSlot tileSlot)
    {
        Rect.sizeDelta = tileSlot.GetComponent<RectTransform>().sizeDelta;
        SpriteRect.rotation = Quaternion.Euler(0, 0, 0);
        UpdateImage();
    }

    public void FitVisual(SingleTileSlot singleTileSlot)
    {
        Rect.sizeDelta = singleTileSlot.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
        SpriteRect.rotation = Quaternion.Euler(0, 0, 0);
        UpdateImage();
    }
    public void FixVisual()
    {
        Rect.sizeDelta = new Vector2(AssignedItem.Size.x * 100, AssignedItem.Size.y * 100);
        SpriteRect.rotation = AssignedItem.Direction ? Quaternion.Euler(0,0,90) : Quaternion.Euler(0, 0, 0);
        UpdateImage();

    }

    public void ConnectToSingleTileSlot(SingleTileSlot singleTileSlot)
    {
        ConnectedSingleTileSlot = singleTileSlot;
        PivotTileSlot = null;
        AssignedItem.Coordinate = new Vector2Int(0,0);
        transform.position = singleTileSlot.transform.GetChild(0).position;
        transform.parent = singleTileSlot.transform.GetChild(0);
        ConnectedStorage = null;
    }

    public void UpdateImage()
    {
        transform.GetChild(0).GetComponent<Image>().sprite = AssignedItem.Sprite;

    }
    
    private void UpdateColor(Color color)
    {
        isVisualUpdated = false;
        
        BackGroundImage.color = color;
        isHighlight = false;
        isRedLight = false;
    }
    
    public void HighlightItem()
    {
        isVisualUpdated = false;

        HighlightType = Types.HighlightType.Highlight;
    }
    
    public void RedLightItem()
    {
        isVisualUpdated = false;
        
        HighlightType = Types.HighlightType.Red;
    }
    
    private void UpdateMaterial()
    {
        if(isVisualUpdated)
            return;
        
        isVisualUpdated = true;


        if (HighlightType == Types.HighlightType.None)
        {
            Debug.Log("Updated");
            if (AssignedItem is StorageItem)
            {
                BackGroundImage.color = ItemSlotProperties.StorageColor;
            }
            else if (AssignedItem is WeaponItem)
            {
                BackGroundImage.color = ItemSlotProperties.WeaponColor;
            }
            else if (AssignedItem is MagazineItem)
            {
                BackGroundImage.color = ItemSlotProperties.MagazineColor;
            }
            else if (AssignedItem is BulletItem)
            {
                BackGroundImage.color = ItemSlotProperties.BulletColor;
            }
            else if (AssignedItem is ModItem)
            {
                BackGroundImage.color = ItemSlotProperties.ModColor;
            }
            else
            {
                BackGroundImage.color = ItemSlotProperties.DefaultColor;
            }
        }
        else if (HighlightType == Types.HighlightType.Highlight)
        {
            BackGroundImage.color = ItemSlotProperties.HighligtedColor;
            HighlightType = Types.HighlightType.None;
            isVisualUpdated = false;
        }
        else if (HighlightType == Types.HighlightType.Red)
        {
            BackGroundImage.color = ItemSlotProperties.RedColor;
            HighlightType = Types.HighlightType.None;
            isVisualUpdated = false;
        }
        
    }

    
    private void ControlItem()
    {
        if (AssignedItem == null)
        {
            Destroy(gameObject);
        }
    }
}
