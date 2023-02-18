using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PageDragController : MonoBehaviour
{
    private GameObject CurrentPage;
    
    private GraphicRaycaster m_Raycaster;
    private PointerEventData m_PointerEventData;
    private EventSystem m_EventSystem;

    private bool isPageMoving;


    private void Awake()
    {
        m_EventSystem = EventSystem.current;
        m_Raycaster = GameObject.FindWithTag("MainCanvas").GetComponent<GraphicRaycaster>();
        m_PointerEventData = new PointerEventData(m_EventSystem);
    }

    private void Update()
    {
        if (!isPageMoving)
        {
            StartDrag();
        }
        else
        {
            OnDrag();
            StopDrag();
        }
     
    }

    private void StartDrag()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        List <RaycastResult> results = SendRay();

        if (results.Count < 1) return;

        
        if (results[0].gameObject.CompareTag("MovablePage"))
        {
            GameObject targetPage = results[0].gameObject;
            CurrentPage = targetPage.transform.parent.parent.gameObject;
            
            isPageMoving = true;
        }
        
        
    }

    private void OnDrag()
    {
        if (isPageMoving && CurrentPage != null)
        {
            CurrentPage.transform.position = Input.mousePosition;
        }
    }
    
    private void StopDrag()
    {
        if (!Input.GetMouseButtonUp(0)) return;

        isPageMoving = false;
    }
    
    private List<RaycastResult> SendRay()
    {
        List<RaycastResult> results = new List<RaycastResult>();

        m_PointerEventData.position = Input.mousePosition;
        m_Raycaster.Raycast(m_PointerEventData, results);

        return results;
    }
}
