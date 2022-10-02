using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static T GetRandom<T>(this List<T> list)
    {
        if (list.Count == 0) return default;

        int rand = UnityEngine.Random.Range(0, list.Count);
        return list[rand];
    }
    public static T ExtractRandom<T>(this List<T> list)
    {
        if (list.Count == 0) return default;
        
        int rand = UnityEngine.Random.Range(0, list.Count);
        var result = list[rand];
        list.RemoveAt(rand);
        return result;
    }
    
    public static bool TryExtractRandom<T>(this List<T> list, out T item)
    {
        item = default;
        if (list.Count == 0) return false;
        
        int rand = UnityEngine.Random.Range(0, list.Count);
        item = list[rand];
        list.RemoveAt(rand);
        return true;
    }

    public static T ExtractAt<T>(this List<T> list, int index)
    {
        var obj = list[index];
        list.RemoveAt(index);
        return obj;
    }
}