using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public static PlayerManager Instance;
    [SerializeField] private Inventory stash;
    [SerializeField] private EquipmentStorage headWear;
    [SerializeField] private EquipmentStorage bodyArmor;
    [SerializeField] private EquipmentStorage primaryWeapon;
    [SerializeField] private EquipmentStorage secondaryWeapon;
    [SerializeField] private EquipmentStorage pistol;
    [SerializeField] private EquipmentStorage melee;
    
    [SerializeField] private EquipmentStorage rig;
    [SerializeField] private EquipmentPage rigPage;
    [SerializeField] private EquipmentStorage backpack;
    [SerializeField] private EquipmentPage backpackPage;
    private void Awake()
    {
        Instance = this;
    }

    public void RefreshCharacterPage()
    {
        headWear.RefreshInventoryPage();
        bodyArmor.RefreshInventoryPage();
        primaryWeapon.RefreshInventoryPage();
        secondaryWeapon.RefreshInventoryPage();
        pistol.RefreshInventoryPage();
        melee.RefreshInventoryPage();
    }

    public void RefreshEquipmentPage()
    {
        rig.RefreshInventoryPage();
        rigPage.RefreshPage();
        backpack.RefreshInventoryPage();
        backpackPage.RefreshPage();
    }

    public void RefreshStashPage()
    {
        stash.RefreshInventoryPage();
    }
}
