using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Throwables 
{
    public static Dictionary<int, List<Throwable>> throwables = new Dictionary<int, List<Throwable>>();

    public static void AddToObjectPool(Throwable throwable)
    {
        if (!throwables.ContainsKey(throwable.specificID))
        {
            throwables.Add(throwable.specificID, new List<Throwable>());
            throwables[throwable.specificID].Add(throwable);
            return;
        }
        throwables[throwable.specificID].Add(throwable);
    }
}
