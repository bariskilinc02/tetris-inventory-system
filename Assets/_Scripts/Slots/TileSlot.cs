using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileSlot : MonoBehaviour
{
    public Tile ConnectedTile;
    public StorageBase ConnectedStorage;
    public Item AssignedItem;
    public Vector2Int Coordinats;

    [HideInInspector] public bool isHighLight, isRedLight;
    [HideInInspector] public Image Image;

    public void SetConnectedStorage(StorageBase storageBase)
    {
        ConnectedStorage = storageBase;
    }

    public void SetConnectWithTiles()
    {
        ConnectedTile = ConnectedStorage.Tiles.Find(x => x.Coordinats == Coordinats);
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

    private void UpdateColor(Color color)
    {
        Image.color = color;
        isHighLight = false;
        isRedLight = false;
    }
}
