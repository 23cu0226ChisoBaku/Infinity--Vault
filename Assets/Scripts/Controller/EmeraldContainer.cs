using UnityEngine;

public class EmeraldContainer : GemContainer
{
    public override void InitProduct()
    {
        _gemType = EGemType.Emerald;
        _itemInstance = new Emerald();
    }
}