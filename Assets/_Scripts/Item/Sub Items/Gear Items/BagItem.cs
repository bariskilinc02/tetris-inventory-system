using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bag Item", menuName = "ScriptableObjects/BagItem Item", order = 1)]
public class BagItem : StorageItem
{
    public override float GetTotalWeight()
    {
        float tempWeight = ItemWeight;
        
        for (int i = 0; i < Storage.Items.Count; i++)
        {
            tempWeight += Storage.Items[i].GetTotalWeight();
        }
        
        return tempWeight;
    }
}
