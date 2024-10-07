/*
Description:List extension method

Author: MAI ZHICONG

Module: None

Update Log: 2024/10/04      Create
*/


using System.Collections.Generic;
using System.Linq;
internal static class ListExtension
{
    internal static void Resize<T>(this List<T> targetList, int size, T replaceValue = default)
    {
        int currentSize = targetList.Count;
        if (currentSize > size)
        {
            targetList.RemoveRange(size, currentSize - size);
        }
        else if (currentSize < size)
        {
            if (targetList.Capacity < size)
            {
                targetList.Capacity = size;
            }
            targetList.AddRange(Enumerable.Repeat(replaceValue, size - currentSize));
        }
    }
}