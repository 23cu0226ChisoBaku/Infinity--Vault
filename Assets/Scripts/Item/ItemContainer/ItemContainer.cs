using System.Collections;
using MDesingPattern.MFactory;
using UnityEngine;

public abstract class ItemContainer : MonoBehaviour, IPickable,IProduct
{
    protected string _itemName = "ItemName_ItemType";
    public abstract void InitProduct();
    public abstract void OnPick(IItemGetable getable);


    // TODO temp code
    protected virtual void Awake()
    {
        StartCoroutine(AvoidPlayerAtFirst());
        var collider2D = GetComponent<Collider2D>();
        collider2D.excludeLayers |= LayerMask.GetMask("Player");
    }

    // TODO temp code
    protected virtual IEnumerator AvoidPlayerAtFirst()
    {
        float tempTime = 1f;

        while(tempTime > 0f)
        {
            tempTime -= Time.deltaTime;
            yield return null;
        }
        var collider2D = GetComponent<Collider2D>();
        collider2D.excludeLayers &= 0;

        yield break;
    }
}