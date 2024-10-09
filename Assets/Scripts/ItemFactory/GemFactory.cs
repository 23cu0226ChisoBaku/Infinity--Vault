using MDesingPattern.MFactory;
using UnityEngine;
using UnityEngine.Assertions;

internal abstract class GemFactory : IFactory
{
    private GameObject _gemPrefab;

    public GemFactory(string path)
    {
        _gemPrefab = Resources.Load<GameObject>(path);
    }
    public IProduct GetProduct()
    {
        GameObject gem = Object.Instantiate(_gemPrefab);

        IProduct gemProduct = gem.GetComponent<IProduct>();

#if UNITY_EDITOR
        Assert.IsNotNull(gemProduct, $"{gem.name} is not inherited IProduct");
#endif
        if (gemProduct.IsAlive())
        {
            gemProduct.InitProduct();
            return gemProduct;
        }
        else
        {
            return null;
        }
    }
}