using System.Collections.Generic;
using UnityEngine;

using MDesingPattern.MFactory;
using MSingleton;

public abstract class AbstractFactoryGroup<T> where T : class
{
    protected Dictionary<string,T> _factoryGroup;
}

internal sealed class ItemGenerator : Singleton<ItemGenerator>,IMonoItemGenerator<ItemContainer>
{
    private IFactoryGroup _itemFactorys;

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

        // Œ©‚Â‚©‚ç‚È‚©‚Á‚½‚çnull‚ð•Ô‚·
        if (foundFactory == null)
        {
            return null;
        }

        var product = foundFactory.GetProduct();

        // ItemContainer‚©‚Ç‚¤‚©‚ð’²‚×‚é
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

        // Œ©‚Â‚©‚ç‚È‚©‚Á‚½‚çnull‚ð•Ô‚·
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