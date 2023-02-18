using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightClickMenu : MonoBehaviour
{
      public static RightClickMenu Instance;
      
      public Item selectedItem;

      [SerializeField] private GameObject menu;


      [SerializeField] private Button inspectButton;
      [SerializeField] private Button openButton;
      [SerializeField] private Button unloadMagazineButton;
      [SerializeField] private Button useButton;
      [SerializeField] private Button discardButton;
      
      private void Awake()
      {
            Instance = this;

            inspectButton.onClick.AddListener(Inspect);
            openButton.onClick.AddListener(Open);
            unloadMagazineButton.onClick.AddListener(Unload);
            useButton.onClick.AddListener(Use);
            discardButton.onClick.AddListener(DiscardItem);
      }

      public void OpenMenu(Item item)
      {
            selectedItem = item;
            
            menu.SetActive(true);
            menu.transform.position = Input.mousePosition;
            menu.transform.SetAsLastSibling();

            if (selectedItem is StorageItem or BagItem)
            {
                  menu.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                  menu.transform.GetChild(1).gameObject.SetActive(false);
            }

            if (selectedItem is MagazineItem magazineItem)
            {
                  menu.transform.GetChild(2).gameObject.SetActive(true);
            }
            else
            {
                  menu.transform.GetChild(2).gameObject.SetActive(false);
            }
            
            if (selectedItem is HealItem healItem)
            {
                  menu.transform.GetChild(3).gameObject.SetActive(true);
            }
            else
            {
                  menu.transform.GetChild(3).gameObject.SetActive(false);
            }
            
      }

      private void Inspect()
      {
            PropertyPageManager.Instance.Create(selectedItem);
            CloseMenu();
      }
      
      private void Open()
      {
            StoragePageCreator.Instance.Create(selectedItem);
            CloseMenu();
      }

      private void Unload()
      {
            if (selectedItem is MagazineItem magazineItem)
            {
                  magazineItem.UnloadBullet();
            }
      }
      
      private void Use()
      {
            if (selectedItem is MagazineItem magazineItem)
            {
                  magazineItem.UnloadBullet();
            }
      }

      private void DiscardItem()
      {
            CloseMenu();
      }

      public void CloseMenu()
      {
            StartCoroutine(CloseMenuRoutine());
      }

      private IEnumerator CloseMenuRoutine()
      {
            yield return new WaitForSeconds(0.1f);
            menu.SetActive(false);
      }
   
    
}
