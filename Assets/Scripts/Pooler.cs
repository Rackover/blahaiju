using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Pooler
{
    private struct PoolIdentifier
    {
        public object poolMaster;
        public Type requestType;
        public byte id;

        public PoolIdentifier(object poolMaster, Type requestType, byte id)
        {
            this.poolMaster = poolMaster;
            this.requestType = requestType;
            this.id = id;
        }

        public override int GetHashCode()
        {
            return poolMaster.GetHashCode() ^ requestType.GetHashCode() ^ id;
        }
    }

    private static Dictionary<PoolIdentifier, Queue<UnityEngine.Object>> pool = new Dictionary<PoolIdentifier, Queue<UnityEngine.Object>>();

    private static Dictionary<float, YieldInstruction> yieldPool = new Dictionary<float, YieldInstruction>();

    public static void Pool<T>(object poolMaster, T resource, byte poolID = 0) where T : Component
    {
        Queue<UnityEngine.Object> pool = GetPool(new PoolIdentifier(poolMaster, typeof(T), poolID));

        resource.gameObject.SetActive(false);

        lock (pool)
        {
            pool.Enqueue(resource);
        }
    }

    public static T DePool<T>(object poolMaster, T example, byte poolID = 0) where T : Component
    {
        Queue<UnityEngine.Object> pool = GetPool(new PoolIdentifier(poolMaster, typeof(T), poolID));

        if (pool.Count > 0)
        {
            lock (pool)
            {
                return pool.Dequeue() as T;
            }
        }

        if (example == null)
        {
            throw new Exception($"Tried to depool with NULL example!");
        }

        T obj = UnityEngine.Object.Instantiate(example, example.transform.parent).GetComponent<T>();

        return obj;
    }

    public static void DestroyPool<T>(object poolMaster, byte poolID = 0) where T : Component
    {
        Queue<UnityEngine.Object> pool = GetPool(new PoolIdentifier(poolMaster, typeof(T), poolID));

        lock (pool)
        {
            pool.Clear();
        }
    }


    public static void DestroyPools(object poolMaster = null)
    {
        lock (pool)
        {
            List<PoolIdentifier> toRemove = new List<PoolIdentifier>();

            foreach (PoolIdentifier key in pool.Keys)
            {
                if (poolMaster == null || key.poolMaster == poolMaster)
                {
                    toRemove.Add(key);

                    while (pool[key].Count > 0)
                    {
                        UnityEngine.Object obj = pool[key].Dequeue();

                        if (obj == null)
                        {
                            Debug.LogWarning($"Uh-uh! There's a null object of type {obj?.GetType()} in the pool of {key.poolMaster}... {obj} this should never happen! Something is wrong with your pool management.");
                        }
                        else if (obj is Component component)
                        {
                            UnityEngine.Object.Destroy(component.gameObject);
                        }
                        else if (obj is GameObject go)
                        {
                            UnityEngine.Object.Destroy(go);
                        }
                    }
                }
            }

            foreach (PoolIdentifier key in toRemove)
            {
                pool.Remove(key);
            }
        }
    }

    public static YieldInstruction WaitForSeconds(float seconds)
    {
        if (yieldPool.ContainsKey(seconds))
        {
            return yieldPool[seconds];
        }

        yieldPool[seconds] = new WaitForSeconds(seconds);

        return yieldPool[seconds];
    }

    private static Queue<UnityEngine.Object> GetPool(PoolIdentifier poolID)
    {
        if (poolID.poolMaster == null)
        {
            throw new NullReferenceException($"Poolmaster cannot be null!");
        }

        if (!pool.ContainsKey(poolID))
        {
            lock (pool)
            {
                pool.Add(poolID, new Queue<UnityEngine.Object>());
            }
        }

        return pool[poolID];
    }
}
