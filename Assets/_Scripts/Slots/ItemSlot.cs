using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{

    public StorageBase ConnectedStorage;
    public Item AssignedItem;
    public Tile PivotTileSlot;

    public List<Tile> ConnectedTileSlots;

    [HideInInspector] public RectTransform Rect;
    [HideInInspector] public RectTransform SpriteRect;

    private void Awake()
    {
        Rect = GetComponent<RectTransform>();
        SpriteRect = transform.GetChild(0).GetComponent<RectTransform>();
    }
    
    public void ChangeDirection()
    {
        Rect.sizeDelta = new Vector2(AssignedItem.Size.x * 100, AssignedItem.Size.y * 100);

        SpriteRect.rotation = AssignedItem.Direction ? Quaternion.Euler(0,0,90) : Quaternion.Euler(0, 0, 0);
        //SpriteRect.localScale = AssignedItem.Direction ? new Vector3((float)AssignedItem.Size.y / (float)AssignedItem.Size.x, (float)AssignedItem.Size.x / (float)AssignedItem.Size.y, 1) : new Vector3(1, 1, 1);
    }

    public void FitVisual(TileSlot tileSlot)
    {
        Rect.sizeDelta = tileSlot.GetComponent<RectTransform>().sizeDelta;
        SpriteRect.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void FixVisual()
    {
        Rect.sizeDelta = new Vector2(AssignedItem.Size.x * 100, AssignedItem.Size.y * 100);
        SpriteRect.rotation = AssignedItem.Direction ? Quaternion.Euler(0,0,90) : Quaternion.Euler(0, 0, 0);
        
    }
}
