using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Inventory ConnectedInventory;
    public Item ItemBase;

    public string itemId;
    void Start()
    {
        //ItemBase = new StorageItem().Construct("chest");
        ItemBase = Instantiate(ItemDataBase.Instance.Items.Find(x => x.Id == "chest"));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            AddItemToInventory();
        }
        //Debug.Log(ItemDataBase.Instance.ReturnClassType("chest"));
        //StorageItem.TileSize = new Vector2Int(1,1);
        //Debug.Log(StorageItem.TileSize);
    }

    public void AddItemToInventory()
    {
        if (ConnectedInventory == null) return;

        ConnectedInventory.AddItem_Auto(itemId);
    }
}
