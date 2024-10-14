using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace MMVCFramework
{
    namespace Model
    {
        public abstract class Models : IModel
        {
            private Dictionary<Type,IModel> _modelsDic;

            public Models()
            {
                _modelsDic = new Dictionary<Type, IModel>();
            }

            public abstract string ModelName{get;}
            public void Add<TModel>(TModel model) where TModel : IModel
            {
                var modelType = typeof(TModel);

                if (!_modelsDic.ContainsKey(modelType))
                {
                    _modelsDic.Add(modelType,model);
                }
                else
                {
                    Assert.IsFalse(true, $"Can not Add Model {model.ModelName} because it is already exist");
                }
            }

            public TModel GetModel<TModel>()
            {
                foreach(var model in _modelsDic)
                {
                    TModel found = default;
                    var foundType = typeof(TModel);

                    if (model.Value is Models models)
                    {
                        found = models.GetModel<TModel>();
                    }
                    else
                    {
                        if (model.Key == foundType)
                        {
                            found = (TModel)model.Value;
                        }
                    }

                    if (!EqualityComparer<TModel>.Default.Equals(found,default(TModel)))
                    {
                        return found;
                    }
                }

                Assert.IsFalse(true, $"Can not find model in {ModelName}.Type : {typeof(TModel)}");
                return default;
            }
        }
    }
}