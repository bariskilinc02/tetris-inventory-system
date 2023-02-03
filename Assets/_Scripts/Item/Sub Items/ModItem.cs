using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mod Item", menuName = "ScriptableObjects/ModItem", order = 1)]
public class ModItem: Item
{
    public Types.GunModType ModType;
    
    public List<Item> CompatibleItems;
}
