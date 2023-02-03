using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemSlotProperties", menuName = "ScriptableObject/Item Slot Properties", order = 1)]
public class ItemSlotProperties : ScriptableObject
{
    public Color DefaultColor;
    public Color HighligtedColor;
    public Color RedColor;

    public Color WeaponColor;
    public Color StorageColor;
    public Color ModColor;
    public Color MagazineColor;
    public Color BulletColor;
}
