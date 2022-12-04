using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Storage Item", order = 1)]
public class StorageItem : Item
{
    public Storage Storage = new Storage();

    private void Awake()
    {
        Storage.Init();
    }

}
