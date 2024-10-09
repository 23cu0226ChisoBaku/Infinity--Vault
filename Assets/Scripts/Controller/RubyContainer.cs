using UnityEngine;

public sealed class RubyContainer : GemContainer
{
    public override void InitProduct()
    {
        _gemType = EGemType.Ruby;
        _itemInstance = new Ruby();
    }
}