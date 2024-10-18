/// <summary>
/// モデルインターフェース
/// </summary>
public interface IModel
{
  string ModelName {get;}
  TModel GetModel<TModel>();
}