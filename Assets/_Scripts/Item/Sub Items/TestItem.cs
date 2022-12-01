using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/LevelTest", order = 1)]
public class TestItem : ItemBase
{

    public TestItem() 
    {

    }

    public TestItem(TestItem item /*, new fields */) : base(item)
    {

    }
}
