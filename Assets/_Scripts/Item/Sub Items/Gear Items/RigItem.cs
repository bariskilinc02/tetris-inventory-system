using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Rig Item", menuName = "ScriptableObjects/Rig Item", order = 1)]
public class RigItem : StorageItem
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
