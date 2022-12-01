using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileSlot : MonoBehaviour
{
    public StorageBase ConnectedStorage;
    public ItemBase AssignedItem;
    public Vector2Int Coordinats;

    [HideInInspector] public bool isHighLight, isRedLight;
    [HideInInspector] public Image Image;

    public void SetConnectedStorage(StorageBase storageBase)
    {
        ConnectedStorage = storageBase;
    }
    private void Awake()
    {
        Image = GetComponent<Image>();
    }

    private void Update()
    {
        if (isHighLight) UpdateColor(Color.green);
        else if (isRedLight) UpdateColor(Color.red);
        else UpdateColor(Color.black);
    }

    private void LateUpdate()
    {
        //isHighLight = false;
    }

    private void UpdateColor(Color color)
    {
        Image.color = color;
        isHighLight = false;
        isRedLight = false;
    }
}
