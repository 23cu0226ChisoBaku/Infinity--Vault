using UnityEngine;
using MSingleton;
using System.Linq;
using MLibrary;

public class VaultManager : SingletonMono<VaultManager>
{
    public void InitItem()
    {
        var itemContainers = FindObjectsByType<InteractableObj>(FindObjectsSortMode.None).OfType<IVault>().ToArray();
        
        itemContainers.Shuffle();

        // KeyItemをそのうちの一個の容器に入れる
        // itemContainers[0].InitVault();
        for(int i = 0; i < itemContainers.Length; ++i)
        {
            var vaultInfo = new VaultInfo();

            vaultInfo.Difficulty = (EPuzzleDifficulty)Random.Range(0,(int)EPuzzleDifficulty.DifficultyCount);
            vaultInfo.ItemCount = 10 * (int)vaultInfo.Difficulty + 15;
            vaultInfo.ItemInfo = new ItemInfo{ Name = "Random Item"};
            itemContainers[i].InitVault(vaultInfo);
        }
    }
}