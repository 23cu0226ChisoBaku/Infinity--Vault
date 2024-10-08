using UnityEngine;

public class GemContainer : ItemContainer
{
    public EGemType GemType;
    public override void OnPick(IItemGetable getable)
    {
        if (getable.IsAlive())
        {
            getable.GetItem(_itemInstance.Info);
        }
    }

    private void Awake()
    {
        switch(GemType)
        {
            case EGemType.Emerald:
            {
                _itemInstance = new Emerald();
            }
            break;
            default:
            {
#if UNITY_EDITOR
                Debug.LogError("Unsupported gem type");
#endif
            }
            break;
        }
    }
}