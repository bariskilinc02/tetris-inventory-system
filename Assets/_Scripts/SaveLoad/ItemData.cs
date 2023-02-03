using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public string ItemId = string.Empty;
    public Vector2Int ItemCoordinate = new Vector2Int();
    public bool Direction = new bool();
    public StorageData StorageData = new StorageData();
    public SubData SubData = new SubData();
    public MagazineData MagazineData = new MagazineData();
}
