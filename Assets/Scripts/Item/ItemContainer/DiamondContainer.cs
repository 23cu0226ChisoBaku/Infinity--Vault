public class DiamondContainer : GemContainer
{
    private static readonly int DIAMOND_WORTH = 500;
     public override void InitProduct()
    {
        GemType = EGemType.Diamond;
        Worth = DIAMOND_WORTH;
        _itemName = "Diamond_Worth";
    }
}