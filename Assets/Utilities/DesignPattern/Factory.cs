
namespace MDesingPattern
{
  namespace MFactory
  {
    /// <summary>
    /// 製品インターフェース
    /// </summary>
    public interface IProduct
    {
      /// <summary>
      /// 製品を初期化する
      /// </summary>
      void InitProduct();
    }
    /// <summary>
    /// ファクトリーインターフェース
    /// </summary>
    public interface IFactory
    {
      /// <summary>
      /// 製品を生産
      /// </summary>
      /// <returns>作った製品</returns>
      IProduct GetProduct();
    }

    /// <summary>
    /// ファクトリーインターフェース
    /// </summary>
    /// <typeparam name="T">製品の型</typeparam>
    public interface IFactory<T>
    {
      /// <summary>
      /// 製品を生産
      /// </summary>
      /// <returns>作った製品</returns>
      T GetProduct();
    }

    /// <summary>
    /// ファクトリーグループインターフェース
    /// </summary>
    public interface IFactoryGroup
    {
      /// <summary>
      /// ファクトリーを探す
      /// </summary>
      /// <param name="name">ファクトリーの名前</param>
      /// <returns>IFactoryインターフェース</returns>
      IFactory GetFactory(string name);
      /// <summary>
      /// ファクトリーを追加
      /// </summary>
      /// <param name="name">ファクトリーの名前</param>
      /// <param name="factory">IFactoryインターフェース</param>
      void AddFactory(string name, IFactory factory);
    }

    #region Unity Mono Factory
    /// <summary>
    /// Unity用GameObject製品インターフェース
    /// </summary>
    public interface IMonoProduct
    {
      /// <summary>
      /// 製品のgameObject
      /// </summary>
      UnityEngine.GameObject gameObject {get;}
      /// <summary>
      /// 製品を初期化
      /// </summary>
      void InitMonoProduct();
    }

    /// <summary>
    /// Unity用GameObject製品ファクトリーインターフェース
    /// </summary>
    public interface IMonoFactory
    {
      /// <summary>
      /// 製品を生産
      /// </summary>
      /// <returns>作ったUnity用GameObject製品</returns>
      IMonoProduct GetMonoProduct();
    }

    public interface IMonoFactoryGroup
    {
        IMonoFactory GetMonoFactory(string name);
        void AddMonoFactory(string name, IMonoFactory monoFactory);
    }
    #endregion
    // End of Unity Mono Factory
  }
}