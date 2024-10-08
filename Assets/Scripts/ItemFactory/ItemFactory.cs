using System;
using System.Collections.Generic;
using System.Linq;
using MDesingPattern.MFactory;
using UnityEngine;
//using MError;
public class Error
    {
        private string _errorMsg;
        private object _errorTarget;

        public Error(string errorMsg, object target)
        {
            _errorMsg = errorMsg;
            _errorTarget = target;
        }

        public string What()
        {
            return _errorMsg;
        }

        public object ErrorSource()
        {
            return _errorTarget;
        }
    }

public abstract class FactoryGroup : IAbstractFactory
{
    private Dictionary<string,IFactory> _factoryGroup;

    public FactoryGroup()
    {
        _factoryGroup = new Dictionary<string, IFactory>();
    }

    public FactoryGroup(params KeyValuePair<string,IFactory>[] factorys)
        : this()
    {
        for(int idx = 0; idx < factorys.Length; ++ idx)
        {
            AddFactory(factorys[idx].Key, factorys[idx].Value);
        }
    }

    public FactoryGroup(Dictionary<string,IFactory> factorys)
    {
        _factoryGroup = factorys;
    }

    public void AddFactory(string name, IFactory factory)
    {
        // エラーメッセージ
        Error errorMsg = null;

        // 有効性チェック
        if (!ValidationCheck(name,factory,out errorMsg))
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogError(errorMsg.What());
            return;
#endif
        }
        else
        {
            _factoryGroup.Add(name,factory);
        }
    }

    public IFactory GetFactory(string name)
    {
                // 入力チェック
        if (string.IsNullOrEmpty(name))
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning($"Invalid input name");
#endif
            return null;
        }

        // コンテナヌルチェック
        if (_factoryGroup == null)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning($"Factorys are not initialized");
#endif
            return null;
        }

        // Factory探し
        if (_factoryGroup.TryGetValue(name, out IFactory factory))
        {
            return factory;
        }
        else
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning($"Can't find Factory by name {name}");
#endif
            return null;
        }
    }

        private bool ValidationCheck(string name, IFactory factory, out Error error)
    {
        error = null;
        // 名前のヌルチェック
        if (string.IsNullOrEmpty(name))
        {
            error = new Error("Null or Empty key",name);

            return false;
        }

        // IFactoryインスタンスのヌルチェック
        if (!factory.IsAlive())
        {
            error = new Error("Null or invalid factory instance",factory);

            return false;
        }

        // 同じ名前のIFactoryだったら
        if (_factoryGroup.ContainsKey(name))
        {
            error = new Error("Key already exist",name);

            return false;
        }

        // 安全性チェック完了
        return true;
    }
}
public sealed class ItemGenerator : MonoBehaviour
{   
    
}