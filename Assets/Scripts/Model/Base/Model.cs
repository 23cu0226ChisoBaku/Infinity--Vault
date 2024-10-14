using UnityEngine.Assertions;

namespace MMVCFramework
{
    namespace Model
    {
        public abstract class Model<T> : IModel
        {
            protected T _model;

            public abstract string ModelName {get;}

            public TModel GetModel<TModel>()
            {
                if (_model is TModel tModel)
                {
                    return tModel;
                }
                else
                {
                    Assert.IsFalse(true, $"Can not find model in {ModelName}.Type : {typeof(TModel)}");
                    return default;
                }
            }
        }
    }
}