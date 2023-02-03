using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SingleTileSlot : MonoBehaviour
{
    public Item ConnectedParentItem;
    public SubModItem ConnectedSubItem;
    
    [HideInInspector] public bool isHighlight, isRedLight;
    [HideInInspector] public Image Image;

    private void Awake()
    {
        Image = GetComponent<Image>();
    }

    private void Update()
    {
        if (isHighlight) UpdateColor(Color.green);
        else if (isRedLight) UpdateColor(Color.red);
        else UpdateColor(Color.black);
    }

    private void UpdateColor(Color color)
    {
        Image.color = color;
        isHighlight = false;
        isRedLight = false;
    }
}
