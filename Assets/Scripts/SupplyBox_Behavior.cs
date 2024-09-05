using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SupplyBox_Behavior : MonoBehaviour
{

    [SerializeField]
    private DeliveryItem supplyItem;

    private GameManager gM;

    [SerializeField]
    private SpriteRenderer supplySprite;

    [SerializeField]
    private int SupplyItemCount;

    private int currentItemCount;

    [SerializeField]
    private float cooldownTime;

    private bool canTake;

    [SerializeField]
    private bool isInfinite;

    [SerializeField]
    private TextMeshPro countText;

    // Start is called before the first frame update
    void Start()
    {
        gM = FindObjectOfType<GameManager>();

        //Debug.Log("Held item is " + supplyItem.name);

        currentItemCount = SupplyItemCount;

        supplySprite.sprite = supplyItem.itemSprite;

        canTake = true;

        countText.text = currentItemCount.ToString();

        if(isInfinite)
        {
            countText.enabled = false;
        }
    }

    public DeliveryItem GetSupplyItem()
    {
        return supplyItem;
    }

    public bool TakeItem()
    {
        if (canTake)
        {
            if (!isInfinite)
            {
                --currentItemCount;
                countText.text = currentItemCount.ToString();
            }
            if (currentItemCount <= 0)
            {
                canTake = false;
                StartCoroutine(BoxCooldownFinished());
                supplySprite.enabled = false;

            }
            return true;
        }
        {
            return false;
        }
    }

    public void BoxStartCooldown()
    {
        supplySprite.enabled = false;
    }

    public IEnumerator BoxCooldownFinished()
    {
        yield return new WaitForSeconds(cooldownTime);
        supplySprite.enabled=true;
        canTake = true;
        currentItemCount = SupplyItemCount;
        countText.text = currentItemCount.ToString();
    }

}
