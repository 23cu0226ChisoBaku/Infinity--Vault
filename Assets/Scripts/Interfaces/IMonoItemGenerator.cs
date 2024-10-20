/// <summary>
/// MonoBehaviour Itemを生成するインターフェース
/// TODO リファクタリングする予定
/// </summary>
/// <typeparam name="T">MonoBehaviourを継承したItemContainer</typeparam>
internal interface IMonoItemGenerator<T> where T : ItemContainer
{
    /// <summary>
    /// アイテムを取得
    /// </summary>
    /// <param name="itemName">アイテムの名前</param>
    /// <returns>アイテムコンテナ</returns>
    public T GenerateSingleItem(string itemName);

    /// <summary>
    /// 一定の数のアイテムを配列に入れて返す
    /// </summary>
    /// <param name="itemName">アイテムの名前</param>
    /// <param name="generateNum">生成する数</param>
    /// <returns>アイテムコンテナの配列</returns>
    public T[] GenerateItems(string itemName, int generateNum);
}