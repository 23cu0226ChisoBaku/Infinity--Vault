using System;
using System.Collections.Generic;
using System.Linq;
using MDesingPattern.MFactory;
using MError;

public class FactoryGroup : IFactoryGroup,IDisposable
{
    private Dictionary<string,IFactory> _factoryGroup;

    public FactoryGroup()
    {
        _factoryGroup = new Dictionary<string, IFactory>();
    }

    /// <summary>
    /// KeyValuePair引数でFactoryを初期化するコンストラクタ
    /// </summary>
    /// <param name="factorys">可変長引数(何個も入れられるはず)</param>
    public FactoryGroup(params KeyValuePair<string,IFactory>[] factorys)
        : this()
    {
        for(int idx = 0; idx < factorys.Length; ++ idx)
        {
            AddFactory(factorys[idx].Key, factorys[idx].Value);
        }
    }

    public FactoryGroup(IList<KeyValuePair<string,IFactory>> factoryList)
        : this(factoryList.ToArray())
    {

    }

    ~FactoryGroup()
    {
        Dispose(false);
    }

    public void AddFactory(string name, IFactory factory)
    {
        // エラーメッセージ
        Error errorMsg = null;

        // 有効性チェック
        if (!ValidationAddCheck(name,factory,out errorMsg))
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

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            GC.SuppressFinalize(this);
        }

        // 既に解放されたら解放しない
        if ((_factoryGroup == null) || (_factoryGroup.Count == 0))
        {
            return;
        }
        else
        {
            _factoryGroup.Clear();
            _factoryGroup = null;
        }
    }

    private bool ValidationAddCheck(string name, IFactory factory, out Error error)
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

        return true;
    }
}