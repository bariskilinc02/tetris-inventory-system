using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet Item", menuName = "ScriptableObjects/Weapon Items/Bullet Item", order = 1)]
public class BulletItem : ModItem
{
    public Types.BulletType BulletType;

    public void CombineBullet(Item item)
    {
        if (item is BulletItem bulletItem)
        {
            if (Quantity < MaxQuantity)
            {
                int emptySpace = MaxQuantity - Quantity;

                if (emptySpace >= item.Quantity)
                {
                    Quantity += item.Quantity;
                    item.Quantity = 0;
                }
                else
                {
                    Quantity += emptySpace;
                    item.Quantity -= emptySpace;
                }
            }
        }
    }
    
    public override float GetTotalWeight()
    {
        float tempWeight = ItemWeight * Quantity;

        return tempWeight;
    }
}
