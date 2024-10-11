using System;
using System.Collections.Generic;
using System.Linq;
using MDesingPattern.MFactory;
using UnityEngine;
using MError;
using MSingleton;

public abstract class AbstractFactoryGroup<T> where T : class
{
    protected Dictionary<string,T> _factoryGroup;
}

public class FactoryGroup : IFactoryGroup,IDisposable
{
    private Dictionary<string,IFactory> _factoryGroup;

    public FactoryGroup()
    {
        _factoryGroup = new Dictionary<string, IFactory>();
    }

    /// <summary>
    /// KeyValuePair������Factory������������R���X�g���N�^
    /// </summary>
    /// <param name="factorys">�ϒ�����(�����������͂�)</param>
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
        // �G���[���b�Z�[�W
        Error errorMsg = null;

        // �L�����`�F�b�N
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
        // ���̓`�F�b�N
        if (string.IsNullOrEmpty(name))
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning($"Invalid input name");
#endif
            return null;
        }

        // �R���e�i�k���`�F�b�N
        if (_factoryGroup == null)
        {
#if UNITY_EDITOR
            UnityEngine.Debug.LogWarning($"Factorys are not initialized");
#endif
            return null;
        }

        // Factory�T��
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

        // ���ɉ�����ꂽ�������Ȃ�
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
        // ���O�̃k���`�F�b�N
        if (string.IsNullOrEmpty(name))
        {
            error = new Error("Null or Empty key",name);

            return false;
        }

        // IFactory�C���X�^���X�̃k���`�F�b�N
        if (!factory.IsAlive())
        {
            error = new Error("Null or invalid factory instance",factory);

            return false;
        }

        // �������O��IFactory��������
        if (_factoryGroup.ContainsKey(name))
        {
            error = new Error("Key already exist",name);

            return false;
        }

        return true;
    }
}

internal sealed class ItemGenerator : Singleton<ItemGenerator>,IMonoItemGenerator<ItemContainer>
{
    private FactoryGroup _itemFactorys;

    public ItemGenerator()
    {
        _itemFactorys = new FactoryGroup(

                        new KeyValuePair<string, IFactory>("Emerald",new EmeraldFactory()),
                        new KeyValuePair<string, IFactory>("Ruby",new RubyFactory())

                                        );
    }
    public ItemContainer GenerateSingleItem(string itemName)
    {
        IFactory foundFactory = _itemFactorys.GetFactory(itemName);

        // ������Ȃ�������null��Ԃ�
        if (foundFactory == null)
        {
            return null;
        }

        var product = foundFactory.GetProduct();

        // ItemContainer���ǂ����𒲂ׂ�
        if (product is ItemContainer itemContainer)
        {
            return itemContainer;
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogError($"Product of {itemName} is not ItemContainer");
#endif
            return null;
        }
    }

    public ItemContainer[] GenerateItems(string itemName,int generateNum)
    {
        IFactory foundFactory = _itemFactorys.GetFactory(itemName);

        // ������Ȃ�������null��Ԃ�
        if (foundFactory == null)
        {
            return null;
        }

        ItemContainer[] itemContainers = new ItemContainer[generateNum];

        for (int i = 0; i < generateNum; ++i)
        {
            var product = foundFactory.GetProduct();

            if (product is ItemContainer itemContainer)
            {
                itemContainers[i] = itemContainer;
            }
            else
            {
#if UNITY_EDITOR
                Debug.LogError($"Product of {itemName} is not ItemContainer");
#endif
                itemContainers[i] = null;
            }
        }

        return itemContainers;
    }
}