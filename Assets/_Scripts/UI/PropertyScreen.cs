using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PropertyScreen : MonoBehaviour
{
    public Item currentItem;

    [Header("Page")]
    [SerializeField] private TextMeshProUGUI header;
    [SerializeField] private Image itemSprite;
    [SerializeField] private TextMeshProUGUI itemWeightText;
    
    [SerializeField] private GameObject page;
    [SerializeField] private GameObject background;
    [SerializeField] private GameObject scrollView;
    [SerializeField] private GameObject itemSlots;
    [SerializeField] private Button closeButton;
    
    [SerializeField] private GameObject tilesParent;
    [SerializeField] private GameObject propertiesParent;
    
    private List<SingleTileSlot> tileSlots;
    
    [Header("Prefabs")]
    [SerializeField] private GameObject propertyPagePrefab;
    [SerializeField] private GameObject singleTileSlotPrefab;
    [SerializeField] private GameObject itemSlotPrefab;
    
    public void UpdateUI(Item item)
    {
        currentItem = item;
        header.text = currentItem.Name;
        itemSprite.sprite = currentItem.Sprite;
        closeButton.onClick.AddListener(ClosePage);
        itemWeightText.text = (item.GetTotalWeight() / 1000f).ToString("0.00") + " kg";
    
        // slot size ,ergo, vertical rec, horiz rec
        if (currentItem is WeaponItem weaponItem)
        {
            CreateWeaponTileSlots();
            CreateWeaponItemSlots();
            
            page.GetComponent<RectTransform>().sizeDelta 
                = new Vector2(500, 150 + CalculatePropertyAreaSize(weaponItem.Ergonomics, weaponItem.HorizontalRecoil, weaponItem.VerticalRecoil) 
                + CalculateTileSlotArea(weaponItem.SubModItems.Count));
            scrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollView.GetComponent<RectTransform>().sizeDelta.x,CalculateTileSlotArea(weaponItem.SubModItems.Count));
            
            CloseAllPropertySlots();
            DrawPropertySlot(weaponItem.Ergonomics, "Ergonomics");
            DrawPropertySlot(weaponItem.VerticalRecoil, "Vertical Recoil");
            DrawPropertySlot(weaponItem.HorizontalRecoil, "Horizontal Recoil");
            // ergo, vertical rec, horiz rec
        }
        else if (currentItem is ModItem modItem)
        {
            CreateModTileSlots();
            CreateModItemSlots();
            
            page.GetComponent<RectTransform>().sizeDelta 
                = new Vector2(500, 150 + CalculatePropertyAreaSize(modItem.Ergonomics, modItem.HorizontalRecoil, modItem.VerticalRecoil)
                                       + CalculateTileSlotArea(modItem.SubModItems.Count));
            
            scrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollView.GetComponent<RectTransform>().sizeDelta.x,CalculateTileSlotArea(modItem.SubModItems.Count));
            
            CloseAllPropertySlots();
            DrawPropertySlot(modItem.Ergonomics, "Ergonomics");
            DrawPropertySlot(modItem.VerticalRecoil, "Vertical Recoil");
            DrawPropertySlot(modItem.HorizontalRecoil, "Horizontal Recoil");
       
            //propertiesParent.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollView.GetComponent<RectTransform>().sizeDelta.x, 58 * 4 );
            // ergo, vertical rec, horiz rec
        }
        else if (currentItem is StorageItem storageItem)
        {
            page.GetComponent<RectTransform>().sizeDelta 
                = new Vector2(500, 150 + CalculatePropertyAreaSize(1));
            scrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollView.GetComponent<RectTransform>().sizeDelta.x,0);
            
            CloseAllPropertySlots();
            DrawPropertySlot(storageItem.Storage.TileSize.x * storageItem.Storage.TileSize.y, "Size");
        }
        else
        {
            page.GetComponent<RectTransform>().sizeDelta 
                = new Vector2(500, 150 + CalculatePropertyAreaSize(1));
            scrollView.GetComponent<RectTransform>().sizeDelta = new Vector2(scrollView.GetComponent<RectTransform>().sizeDelta.x,0);
            
            CloseAllPropertySlots();
        }
    }

    private int CalculateTileSlotArea(int slotCount)
    {
        int areaSize = 0;
        if (slotCount == 0)
        {
            areaSize = 0;
        }
        else if(slotCount is > 0 and <= 4)
        {
            areaSize = 100;
        }
        else if(slotCount is > 4 and <= 8)
        {
            areaSize = 200;
        }
        return areaSize;
    }
    
    private int CalculatePropertySlotArea(Item item)
    {
        int areaSize = 0;
        
        return areaSize;
    }

    private bool DrawPropertySlot(float value, string title)
    {
        if (value != 0)
        {
            for (int i = 0; i < propertiesParent.transform.childCount; i++)
            {
                if (!propertiesParent.transform.GetChild(i).gameObject.activeSelf)
                {
                   GameObject slot = propertiesParent.transform.GetChild(i).gameObject;
                   
                   slot.SetActive(true);
                   slot.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = title;
                   slot.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = value.ToString("0");
                   break;
                }
                
             
            }
            return false;
        }
        else
        {
            return true;
        }
    }

    private int CalculatePropertyAreaSize(params float[] values)
    {
        int size = 0;
        for (int i = 0; i < values.Length; i++)
        {
            if (values[i] != 0)
            {
                 size += 1;
            }
        }

        return size * 58;
    }
    private void CloseAllPropertySlots()
    {
        for (int i = 0; i < propertiesParent.transform.childCount; i++)
        {
            propertiesParent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    #region Weapon
    private void CreateWeaponTileSlots()
    {
        tileSlots = new List<SingleTileSlot>();
        if (currentItem is WeaponItem weaponItem)
        {
            for (int i = 0; i < weaponItem.SubModItems.Count; i++)
            {
                GameObject tileSlot = Instantiate(singleTileSlotPrefab, tilesParent.transform);
                SingleTileSlot singleTileSlot = tileSlot.GetComponent<SingleTileSlot>();
                singleTileSlot.ConnectedParentItem = currentItem;
                singleTileSlot.ConnectedSubItem = weaponItem.SubModItems[i];
                
                tileSlots.Add(singleTileSlot);
            }
        }
    }
    
    private void CreateWeaponItemSlots()
    {
        if (currentItem is WeaponItem weaponItem)
        {
            for (int i = 0; i < weaponItem.SubModItems.Count; i++)
            {
                if (weaponItem.SubModItems[i].ModItem != null)
                {
                    GameObject itemSlotGameObject = Instantiate(itemSlotPrefab, tilesParent.transform);
                    ItemSlot itemSlot = itemSlotGameObject.GetComponent<ItemSlot>();
                    

                    itemSlot.AssignedItem = weaponItem.SubModItems[i].ModItem;
                    itemSlot.ConnectToSingleTileSlot(tileSlots[i]);
                    itemSlot.FitVisualSingleTileSlot(itemSlot.ConnectedSingleTileSlot);
                }
            }
        }
    }
    #endregion

    #region Mod
    private void CreateModTileSlots()
    {
        tileSlots = new List<SingleTileSlot>();
        if (currentItem is ModItem weaponItem)
        {
            for (int i = 0; i < weaponItem.SubModItems.Count; i++)
            {
                GameObject tileSlot = Instantiate(singleTileSlotPrefab, tilesParent.transform);
                SingleTileSlot singleTileSlot = tileSlot.GetComponent<SingleTileSlot>();
                singleTileSlot.ConnectedParentItem = currentItem;
                singleTileSlot.ConnectedSubItem = weaponItem.SubModItems[i];
                
                tileSlots.Add(singleTileSlot);
            }
        }
    }
    
    private void CreateModItemSlots()
    {
        if (currentItem is ModItem weaponItem)
        {
            for (int i = 0; i < weaponItem.SubModItems.Count; i++)
            {
               
                if (weaponItem.SubModItems[i].ModItem != null)
                {
                    GameObject itemSlotGameObject = Instantiate(itemSlotPrefab, tilesParent.transform);
                    ItemSlot itemSlot = itemSlotGameObject.GetComponent<ItemSlot>();
                    

                    itemSlot.AssignedItem = weaponItem.SubModItems[i].ModItem;
                    itemSlot.ConnectToSingleTileSlot(tileSlots[i]);
                    itemSlot.FitVisualSingleTileSlot(itemSlot.ConnectedSingleTileSlot);
                }
            }
        }
    }
    

    #endregion
    
    
    public void ClosePage()
    {
        Destroy(page);
    }
}
