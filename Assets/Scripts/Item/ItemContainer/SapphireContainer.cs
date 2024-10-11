public class SapphireContainer : GemContainer
{
    private static readonly int SAPPHIRE_WORTH = 250;
    public override void InitProduct()
    {
        GemType = EGemType.Sapphire;
        Worth = SAPPHIRE_WORTH;
        _itemName = "Sapphire_Worth";
    }
}