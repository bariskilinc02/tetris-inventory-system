using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mod Item", menuName = "ScriptableObjects/Weapon Items/ModItem", order = 1)]
public class ModItem: Item
{
    [Header("ModItem")]
    public Types.GunModType ModType;

    public float Ergonomics;
    public float VerticalRecoil;
    public float HorizontalRecoil;
    
    public List<Item> CompatibleItems;

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
