using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Types
{
    public enum HighlightType
    {
        None,
        Highlight,
        Red
    }
   public enum ItemType
    {
        Common,
        Storage,
        Bag,
        Vest,
        BodyArmor,
        Weapon,
        Mod,
        HeadWear,
        Pistol,
        Melee,
        Heal,
        Food,
        Drink,
        Barter,
        Ammunition,
        
    }
   
   public enum GunModType
   {
       Sight,
       Foregrip,
       Flashlight,
       Pistolgrip,
       Stock,
       Suppressor,
       FlashHider,
       MuzzleBrake,
       Magazine,
       Bullet,
   }
   
   public enum BulletType
   {
       _7x62_39,
       _5x56_45,
       
   }

    public enum WeaponType
    {
        AssaultRifle,
        Pistol,
        Melee,
    }
}
