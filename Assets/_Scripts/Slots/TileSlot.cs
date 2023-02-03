using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileSlot : MonoBehaviour
{
    public Tile ConnectedTile;
    public StorageBase ConnectedStorage;
    public Vector2Int Coordinates;

    [HideInInspector] public bool isHighlight, isRedLight;
    [HideInInspector] public Image Image;

    public void SetConnectedStorage(StorageBase storageBase)
    {
        ConnectedStorage = storageBase;
    }

    public void ConnectToTile()
    {
        ConnectedTile = ConnectedStorage.Tiles.Find(x => x.Coordinats == Coordinates);
    }

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
