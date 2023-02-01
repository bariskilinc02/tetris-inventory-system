using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemBehaviour
{
    public static Item CreateNewItem(string id)
    {
        Item Item = UnityEngine.Object.Instantiate(ItemDataBase.Instance.Items.Find(x => x.Id == id));

        return Item;
    }

    public static bool IsType<T>(T type) where T : Item
    {

        return true;
    }

    public static T GetItem<T>(Item item) where T : Item, new()
    {
        T newItem = (T)item; 
        return newItem;
    }
}
