using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Item", menuName = "ScriptableObjects/Weapon Items/WeaponItem", order = 1)]
public class WeaponItem : Item
{
    public float Ergonomics;
    public float VerticalRecoil;
    public float HorizontalRecoil;
    
    public List<SubModItem> SubModItems;

    public bool CheckItemIsCompatible(Item item, out SubModItem subModItem)
    {
        bool isCompatible = false;
        subModItem = null;
        for (int i = 0; i < SubModItems.Count; i++)
        {
            if (SubModItems[i].IsAvailable(item, this))
            {
                subModItem = SubModItems[i];
                isCompatible = true;
                break;
            }
        }

        
        return isCompatible;
        
    }
    
    public override float GetTotalWeight()
    {
        float tempWeight = ItemWeight;
        
        for (int i = 0; i < SubModItems.Count; i++)
        {
            if (SubModItems[i].ModItem != null)
            {
                tempWeight += SubModItems[i].ModItem.GetTotalWeight();
            }
            
        }
        
        return tempWeight;
    }
}

[System.Serializable]
public class SubModItem
{
    public List<Types.GunModType> AvailableTypes;
    public Item ModItem;

    public bool IsAvailable(Item item, Item parentItem)
    {
        bool isAvailable = false;
        
        if (ModItem != null)
        {
            isAvailable = false;
        }
        else
        {
            if (item is ModItem modItem)
            {
                if (AvailableTypes.Exists(x => x == modItem.ModType))
                {
                   
                    if (modItem.CompatibleItems.Exists(x => x.Id == parentItem.Id))
                    {
                        Debug.Log("ss");
                        isAvailable = true;
                        Debug.Log("is done");
                    }
                }
               
            
            }
        }

        return isAvailable;
    }
    
    
}