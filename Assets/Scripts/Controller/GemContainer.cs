using UnityEngine;

public class GemController : MonoBehaviour,IPickable
{
    public EGemType GemType;
    public Gem Gem;

    public ItemInfo Info => Gem.Info;

    public void OnPick(IItemGetable getable)
    {
        if (getable.IsAlive())
        {
            getable.GetItem(Gem.Info);
        }
    }

    private void Awake()
    {
        switch(GemType)
        {
            case EGemType.Emerald:
            {
                Gem = new Emerald();
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