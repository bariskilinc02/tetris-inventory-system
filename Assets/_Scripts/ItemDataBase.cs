using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemDataBase : MonoBehaviour
{
    public static ItemDataBase Instance;

    public List<ItemBase> Items;
    private void Awake()
    {
        Instance = this;

        Items = Resources.LoadAll<ItemBase>("ItemDataBase").ToList();
    }

}
