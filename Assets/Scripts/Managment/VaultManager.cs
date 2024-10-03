using UnityEngine;
using MSingleton;
using System.Linq;
using MLib;

public class VaultManager : SingletonMono<VaultManager>
{
    public void InitItem()
    {
        var itemContainers = FindObjectsByType<InteractableObj>(FindObjectsSortMode.None).OfType<IItemSettable>().ToArray();
        
        itemContainers.Shuffle();

        // KeyItem‚ðƒZƒbƒg
        itemContainers[0].SetItem(new Item { name = "KeyItem"});
        for(int i = 1; i < itemContainers.Length; ++i)
        {
            itemContainers[i].SetItem(new Item { name = "None"});
        }
    }
}