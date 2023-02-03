using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mod Item", menuName = "ScriptableObjects/ModItem", order = 1)]
public class ModItem: Item
{
    [Header("ModItem")]
    public Types.GunModType ModType;
    
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
}
