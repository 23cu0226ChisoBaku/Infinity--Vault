public interface IModel
{
  string ModelName {get;}
  TModel GetModel<TModel>();
}