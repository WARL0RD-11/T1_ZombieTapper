using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeliveryItem", menuName = "ScriptableObjects/DeliveryItem", order = 1)]
public class DeliveryItem : ScriptableObject
{
    //Name of the item
    //Might not be neccessary
    [SerializeField]
    public string itemName;

    //Sprite of the item
    //I do not remember if this is the correct format but I can fix later.
    [SerializeField]
    public Sprite itemSprite;

    [SerializeField]
    public Weapon weapon;
}
