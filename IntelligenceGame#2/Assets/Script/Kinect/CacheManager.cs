using UnityEngine;
using System;
using System.Collections.Generic;

public class CacheManager : MonoBehaviour
{
    private class Pair
    {
        public float time;
        public object obj;

        public Pair(float time, object obj)
        {
            this.time = time;
            this.obj = obj;
        }
    }

    private static CacheManager instance = null;

    private Dictionary<object, Pair> cached = null;
    
    private int count;

    public static CacheManager Instance
    {
        get
        {
            if (instance)
                return instance;
            else
                return instance = new GameObject("CacheManager").AddComponent<CacheManager>();
        }
    }

    private CacheManager()
    {
        this.cached = new Dictionary<object, Pair>();
    }

    public void Update()
    {
        count += 1;
        
        if (count % 30 == 0)
        {
            Debug.Log("cached Count : " + cached.Count);
            List<object> Keys = new List<object>();
            foreach (KeyValuePair<object, Pair> pair in this.cached)
            {
                if (Time.time - pair.Value.time >= 1.0F)
                    Keys.Add(pair.Key);
            }
            foreach (object obj in Keys)
                this.cached.Remove(obj);
            System.GC.Collect();
        }
    }

    public T Get<T>(string resourcePath) where T : class
    {
        
        Pair pair = null;
        this.cached.TryGetValue(resourcePath, out pair);
        if (pair == null)
        {
            this.cached.Add(resourcePath, new Pair(0.0F, Resources.Load(resourcePath)));
            return this.Get<T>(resourcePath);
        }
        else
        {
            pair.time = Time.time;
            return pair.obj as T;
        }
    }
    public T Get<T>(GameObject gameObject) where T : Component
    {
        Pair pair = null;
        this.cached.TryGetValue(gameObject, out pair);
        if (pair == null)
        {
            this.cached.Add(gameObject, new Pair(0.0F, new Dictionary<Type, object>()));
            return this.Get<T>(gameObject);
        }
        else
        {
            Dictionary<Type, object> components = pair.obj as Dictionary<Type, object>;
            object value = null;
            components.TryGetValue(typeof(T), out value);
            if (value == null)
            {
                components.Add(typeof(T), gameObject.GetComponent<T>());
                return this.Get<T>(gameObject);
            }
            else
            {
                pair.time = Time.time;
                return value as T;
            }
        }
    }
    public T[] Gets<T>(GameObject gameObject) where T : Component
    {
        Pair pair = null;
        this.cached.TryGetValue(gameObject, out pair);
        if (pair == null)
        {
            this.cached.Add(gameObject, new Pair(0.0F, new Dictionary<Type, object>()));
            return this.Gets<T>(gameObject);
        }
        else
        {
            Dictionary<Type, object> components = pair.obj as Dictionary<Type, object>;
            object value = null;
            components.TryGetValue(typeof(T[]), out value);
            if (value == null)
            {
                components.Add(typeof(T[]), gameObject.GetComponents<T>());
                return this.Gets<T>(gameObject);
            }
            else
            {
                pair.time = Time.time;
                return value as T[];
            }
        }
    }
}