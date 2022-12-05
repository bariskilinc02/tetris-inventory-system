using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase Instance;

    public List<Item> Items;
    private void Awake()
    {
        Instance = this;

        Items = Resources.LoadAll<Item>("ItemDataBase").ToList();
    }

    public Type ReturnClassType(string id)
    {
        var item = Items.Find(x => x.Id == id);
        Type type = item.GetType();
        return type;
    }

    public Item CreateInstanceOfItem(string id)
    {
        var item = Instance.Items.Find(x => x.Id == id);
        return item;
    }

}
