using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupplyBox_Behavior : MonoBehaviour
{

    [SerializeField]
    private DeliveryItem supplyItem;

    private GameManager gM;

    [SerializeField]
    private SpriteRenderer supplySprite;

    // Start is called before the first frame update
    void Start()
    {
        gM = FindObjectOfType<GameManager>();

        //Debug.Log("Held item is " + supplyItem.name);

        supplySprite.sprite = supplyItem.itemSprite;
    }

    public DeliveryItem GetSupplyItem()
    {
        return supplyItem;
    }

}
