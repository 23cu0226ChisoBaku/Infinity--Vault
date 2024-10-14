using System;
using System.Collections;
using System.Collections.Generic;

namespace MLibrary
{
    public static partial class MLib
    {
        public static void Shuffle<T>(this T[] array)
        {
            Random rng = new Random();  
            int n = array.Length;
            while (n > 1)
            {
                --n;
                int k = rng.Next(n + 1);
                Swap(ref array[k], ref array[n]);
            }
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            Random rng = new Random();  
            int n = list.Count;
            while (n > 1)
            {
                --n;
                int k = rng.Next(n + 1);
                T temp = list[k];
                list[k] = list[n];
                list[n] = temp;
            }
        }

        internal static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

    }

    
}