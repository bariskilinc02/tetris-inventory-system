using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeManager : MonoBehaviour
{
    public static TradeManager Instance;
    [SerializeField] private TraderInventory traderInventory;
    [SerializeField] private Trader currentTrader;

    [SerializeField] private Inventory stash;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ConnectAndCreateInventory();
    }

    public void ConnectAndCreateInventory()
    {
        traderInventory.LoadTraderInventory(currentTrader.ItemList);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ConnectAndCreateInventory();
        }
    }

    public void Buy(ItemSlot itemSlot)
    {
        stash.AddItem_Auto(itemSlot.AssignedItem.Id);
    }

    public void RefreshTraderPage()
    {
        ConnectAndCreateInventory();
    }
    
    
}
