using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PropertyPageManager : MonoBehaviour
{
    public static PropertyPageManager Instance;

    public GameObject UIParent;
    
    [Header("Prefabs")]
    public GameObject PropertyPage;

    [Header("Page")]
    public GameObject Page;

    private List<PropertyScreen> propertyScreens;

    private void Awake()
    {
        Instance = this;
    }


    public void Create(Item item)
    {
        propertyScreens = new List<PropertyScreen>();
        propertyScreens = FindObjectsOfType<PropertyScreen>().ToList();

        if(!propertyScreens.Exists(x => x.currentItem == item))
        {
            CreateFrame();
            Page.GetComponent<PropertyScreen>().UpdateUI(item);
        }

    }
    

    
    public void CreateFrame()
    {
        Page = Instantiate(PropertyPage, UIParent.transform);
    }

}
