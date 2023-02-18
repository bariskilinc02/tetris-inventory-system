using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] private TextMeshProUGUI valueText;

    private void Awake()
    {
        Rect = GetComponent<RectTransform>();
        SpriteRect = transform.GetChild(0).GetComponent<RectTransform>();
    }

    private void Update()
    {
        ControlItem();
        UpdateMaterial();
        UpdateUI();
    }

    public void ChangeDirection()
    {
        Rect.sizeDelta = new Vector2(AssignedItem.Size.x * 80, AssignedItem.Size.y * 80);

        SpriteRect.rotation = AssignedItem.Direction ? Quaternion.Euler(0,0,90) : Quaternion.Euler(0, 0, 0);
        //SpriteRect.localScale = AssignedItem.Direction ? new Vector3((float)AssignedItem.Size.y / (float)AssignedItem.Size.x, (float)AssignedItem.Size.x / (float)AssignedItem.Size.y, 1) : new Vector3(1, 1, 1);
    }

    public void FitVisualTileSlot(TileSlot tileSlot)
    {
        Rect.sizeDelta = tileSlot.GetComponent<RectTransform>().sizeDelta;
        SpriteRect.rotation = Quaternion.Euler(0, 0, 0);
        UpdateImage();
    }

    public void FitVisualSingleTileSlot(SingleTileSlot singleTileSlot)
    {
        Rect.sizeDelta = singleTileSlot.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta;
        SpriteRect.rotation = Quaternion.Euler(0, 0, 0);
        UpdateImage();
    }
    public void FixVisual()
    {
        Rect.sizeDelta = new Vector2(AssignedItem.Size.x * 80, AssignedItem.Size.y * 80);
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

    private void UpdateUI()
    {
        if (Time.frameCount % 30 == 0) return;

        valueText.gameObject.SetActive(true);
        if (AssignedItem is BulletItem bulletItem)
        {
            valueText.text = bulletItem.Quantity.ToString();
        }
        else if (AssignedItem is ArmorItem armorItem)
        {
            valueText.text = armorItem.Durability.ToString("0,0") + "/" + armorItem.MaxDurability.ToString("0,0");
        }
        else if (AssignedItem is HealItem healItem)
        {
            if (healItem.MaxHealAmount == 1)
            {
                valueText.gameObject.SetActive(false);
            }
            else
            {
                valueText.text = healItem.HealAmount.ToString("0") + "/" + healItem.MaxHealAmount.ToString("0");
            }
           
        }
        else
        {
            valueText.gameObject.SetActive(false);
        }
       
    }
    private void ControlItem()
    {
        if (AssignedItem == null)
        {
            Destroy(gameObject);
        }
    }

    public void SetParent(Tile tile)
    {
        gameObject.transform.parent = tile.ConnectedStorage.TargetItemSlots;
    }
}
