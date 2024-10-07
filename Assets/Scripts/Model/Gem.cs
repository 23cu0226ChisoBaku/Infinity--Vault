public enum EGemType
{
    None = 0,
    Emerald ,       // エメラルド (価値レベル1)
    Ruby,           // ルビー     (価値レベル2)
    Sapphire,       // サファイア (価値レベル3)
    Diamond,        // ダイヤモンド(価値レベル4)
}
public class Gem : Item
{
    protected Gem(EGemType gemType)
        : base(EItemType.Worth,gemType.ToString())
    { }


}