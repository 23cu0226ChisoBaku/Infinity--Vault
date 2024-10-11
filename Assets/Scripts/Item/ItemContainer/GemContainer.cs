using UnityEngine;

public abstract class GemContainer : ItemContainer
{
    public EGemType GemType {get;protected set;}
    public int Worth {get;protected set;}

    private void Awake()
    {
        var collider2D = GetComponent<Collider2D>();

    }

    public override void OnPick(IItemGetable getable)
    {
        if (getable.IsAlive())
        {
            getable.GetItem(this);
        }
        Destroy(gameObject);
    }

}