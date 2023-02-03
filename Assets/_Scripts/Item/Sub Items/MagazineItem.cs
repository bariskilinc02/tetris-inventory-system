using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Magazine Item", menuName = "ScriptableObjects/Magazine Item", order = 1)]
public class MagazineItem : ModItem
{
    [Header("Magazine")]
    public int MagazineSize;
    public List<Item> Bullets;
    public Types.BulletType BulletType;

    public bool IsCompatible(Item item)
    {
        bool isCompatible = false;
      
        if (item is BulletItem bulletItem)
        {
            if (BulletType == bulletItem.BulletType)
            {
                if (Bullets.Count < MagazineSize)
                {
                    if (item.Quantity > 0)
                    {
                        isCompatible = true;
                    }
                }
            }
        }
        
        return isCompatible;
    }

    public bool LoadBullet(Item item)
    {
        if (IsCompatible(item))
        {
            Bullets.Add(ItemDataBase.Instance.CreateInstanceOfItem(item.Id));
            item.Quantity -= 1;
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void LoadAllBullet(Item item)
    {
        int loopCount = item.Quantity;
        for (int i = 0; i < loopCount; i++)
        {
            if (!LoadBullet(item))
            {
                break;
            }
        
        }
       
    }
}
