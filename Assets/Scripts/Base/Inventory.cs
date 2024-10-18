/*
Description:Infinity Vaule Inventory

Author: MAI ZHICONG

Module: namespace IV.Item

Update Log: 2024/10/04      Create
*/

using System.Collections.Generic;

using UnityEngine;

// Infinity Vault Model
// TODO
// まだ使っていない
namespace IV 
{
  namespace Item
  {
    internal class Inventory<T> where T : class,new()
    {
      protected internal readonly static int INVENTORY_DEFAULT_CAPACITY;
      private int _itemListCapacity;
      protected List<T> _itemList;

      // 一回だけ実行するコンストラクタ
      static Inventory()
      {
        INVENTORY_DEFAULT_CAPACITY = 2;
      }

      public Inventory()
      {
        ItemListCapacity = INVENTORY_DEFAULT_CAPACITY; 
        _itemList = new List<T>();
      }
      public Inventory(int Capacity)
        : this()
      {
        ItemListCapacity = Capacity;
        _itemList.Resize(ItemListCapacity);
      }
      public int ItemListCapacity
      {
        get => _itemListCapacity;
        protected set
        {
            _itemListCapacity = value;
        }
      }
      public int ItemCount => _itemList.Count;
      public T[] ItemList => _itemList.ToArray();

      #region Operator Overload
      public T this[int index]
      {
        get
        {
          if (index >= ItemListCapacity)
          {
#if UNITY_EDITOR
            Debug.LogError($"Index:{index} is out of bounds!! Max capacity:{ItemListCapacity}");
#endif
            return default;
          }
          else
          {
            return _itemList[index];
          }
        }
        set
        {
          if (index >= ItemListCapacity)
          {
#if UNITY_EDITOR
            Debug.LogError($"Index:{index} is out of bounds!! Max capacity:{ItemListCapacity}");
#endif
          }
          else
          {
            _itemList[index] = value;
          }
        }
      }
      #endregion
      // End of Operator Overload
    } 
  }
}
