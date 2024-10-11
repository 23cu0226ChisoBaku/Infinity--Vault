using UnityEngine;

public sealed class RubyContainer : GemContainer
{
    private static readonly int RUBY_WORTH = 100;
    public override void InitProduct()
    {
        GemType = EGemType.Ruby;
        Worth = RUBY_WORTH;
        _itemName = "Ruby_Worth";
    }
}