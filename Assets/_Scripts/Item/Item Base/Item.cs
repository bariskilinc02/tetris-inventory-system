using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/BaseItem", order = 1)]
public class Item: ScriptableObject
{
    public Types.ItemType ItemType;

    public string Name;
    public string Id;

    public Vector2Int Size;
    public Vector2Int Coordinate;

    public Sprite Sprite;

    public bool Direction;

    [SerializeField] private int quantity;
    public int Quantity
    {
        get => quantity;
        set
        {
            if (value == 0)
            {
                quantity = 0;
                Destroy(this);
            }
            else
            {
                quantity = value;   
            }
        }
    }

    public int MaxQuantity;

    public float ItemWeight;
    public float TotalWeight;

    public void ChangeItemDirection()
    {
        Vector2Int currentSize = Size;
        Direction = !Direction;

        Size = new Vector2Int(currentSize.y, currentSize.x);
    }
}
