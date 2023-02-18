using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StoragePageCreator : MonoBehaviour
{
    public static StoragePageCreator Instance;

    #region Components
    [Header("Parents")]
    public GameObject UIParent;

    [Header("Prefabs")]
    public GameObject StoragePage;

    [Header("Storage Page")]
    public GameObject Page;

    [Header("Inputs")]
    public Item CurrentItem;
    public StorageBase CurrentInventory;
    public Storage CurrentStorage;


    #endregion
    
    public List<StorageScreen> storageScreens;

    private void Awake()
    {
        Instance = this;
    }
    
    #region Create
    public void Create(Item item)
    {
        
        storageScreens = new List<StorageScreen>();
        storageScreens = FindObjectsOfType<StorageScreen>().ToList();

        if(!storageScreens.Exists(x => x.CurrentItem == item))
        {
            CreateFrame();
            Page.GetComponent<StorageScreen>().UpdateUI(item);
        }
        
        storageScreens = FindObjectsOfType<StorageScreen>().ToList();
    }

    public void CreateFrame()
    {
        Page = Instantiate(StoragePage, UIParent.transform);
    }

    #endregion




}
