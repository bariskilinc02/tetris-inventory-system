using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new item list", menuName = "ScriptableObjects/Item List")]
public class ItemList : ScriptableObject
{
    public List<Item> Items;
}
