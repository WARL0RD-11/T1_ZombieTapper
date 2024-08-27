using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesiredItemBubble : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    private SpriteRenderer itemRenderer;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UpdateItemSprite(Sprite sprite)
    {
        //Debug.Log("Set the item bubble sprite");
        itemRenderer.sprite = sprite;
    }
}
