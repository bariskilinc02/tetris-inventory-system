using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public ItemBase ItemBase;
    void Start()
    {
        //ItemBase = new StorageItem().Construct("chest");
        ItemBase = ItemDataBase.Instance.Items.Find(x => x.Id == "chest");
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(ItemDataBase.Instance.ReturnClassType("chest"));
        //StorageItem.TileSize = new Vector2Int(1,1);
        //Debug.Log(StorageItem.TileSize);
    }
}
