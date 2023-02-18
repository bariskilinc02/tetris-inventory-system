using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Storage Item", order = 1)]
public class StorageItem : Item
{
    public Storage Storage = new Storage();
    public List<Types.ItemType> AvailableItems;

    private void Awake()
    {
        Storage.Init();
    }

    public bool isAvailable(Item item)
    {
        return AvailableItems.Exists(x => x == item.ItemType);
    }
    
    public bool IsSubItem(Item item)
    {
        bool isSubItem = false;
        if (item == this)
        {
            isSubItem = true;
            return isSubItem;
        }
        
        for (int i = 0; i < Storage.Items.Count; i++)
        {
            if (Storage.Items.Exists(x => x == item))
            {
                isSubItem = true;
            }
            else if ( Storage.Items[i] is StorageItem storageItem)
            {
                isSubItem = storageItem.IsSubItem(item);
            }
            
           
        }
        return isSubItem;
    }
    
    public override float GetTotalWeight()
    {
        float tempWeight = ItemWeight;
        
        for (int i = 0; i < Storage.Items.Count; i++)
        {
            tempWeight += Storage.Items[i].GetTotalWeight();
        }
        
        return tempWeight;
    }

}
