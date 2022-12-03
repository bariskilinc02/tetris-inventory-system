using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "ScriptableObjects/Level", order = 1)]
public class ItemBase: ScriptableObject
{
    public string Name;
    public string Id;

    public Vector2Int Size;
    public Vector2Int Coordinat;

    public Sprite Sprite;

    public int Quantity;
    public int MaxQuantity;

    public float ItemWeight;
    public float TotalWeight;
}
