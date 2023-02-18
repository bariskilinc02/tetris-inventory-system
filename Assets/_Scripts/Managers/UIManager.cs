using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance;

    [Header("Menu")]
    [SerializeField] private GameObject menu;
    
    [Header("Player")]
    [SerializeField] private GameObject stash;
    [SerializeField] private GameObject character;
    [SerializeField] private GameObject equipment;
    
    [Header("Trader")]
    [SerializeField] private GameObject tradeStash;
    
    [Header("Buttons")]
    [SerializeField] private Button characterButton;
    [SerializeField] private Button tradersButton;
    [SerializeField] private Button menuButton;
    private void Awake()
    {
        Instance = this;
        
        characterButton.onClick.AddListener(LoadCharacterMenu);
        tradersButton.onClick.AddListener(LoadTraderMenu);
        menuButton.onClick.AddListener(LoadMenu);
    }

    public void LoadCharacterMenu()
    {
        menu.SetActive(false);
        
        stash.SetActive(true);
        character.SetActive(true);
        equipment.SetActive(true);
        
        PlayerManager.Instance.RefreshCharacterPage();
        PlayerManager.Instance.RefreshEquipmentPage();
        PlayerManager.Instance.RefreshStashPage();
    }
    
    public void LoadTraderMenu()
    {
        menu.SetActive(false);
        
        tradeStash.SetActive(true);
        stash.SetActive(true);
        equipment.SetActive(true);
        
        TradeManager.Instance.RefreshTraderPage();
        PlayerManager.Instance.RefreshEquipmentPage();
        PlayerManager.Instance.RefreshStashPage();
    }

    public void LoadMenu()
    {
        menu.SetActive(true);
        
      
        character.SetActive(false);
        stash.SetActive(false);
        equipment.SetActive(false);
        tradeStash.SetActive(false);
        
    }
    
}
