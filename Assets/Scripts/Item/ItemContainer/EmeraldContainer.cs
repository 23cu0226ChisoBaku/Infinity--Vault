public class EmeraldContainer : GemContainer
{
    private static readonly int EMERALD_WORTH = 50;
    public override void InitProduct()
    {
        GemType = EGemType.Emerald;
        Worth = EMERALD_WORTH;
        _itemName = "Emerald_Worth";
    }
}