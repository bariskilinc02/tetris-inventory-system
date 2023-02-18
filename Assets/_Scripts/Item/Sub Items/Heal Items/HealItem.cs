using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItem : Item
{
    [Header("Heal")]
    public float UseTime;
    
    public int HealAmount;
    public int MaxHealAmount;
}
