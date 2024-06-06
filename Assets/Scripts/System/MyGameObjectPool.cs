using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameObjectPool : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> m_pool;
    public PrefabList prefabList;

    private static MyGameObjectPool instance;
    public static MyGameObjectPool Instance { get { return instance; } }
    private void Awake()
    {
        instance = this;
        m_pool = new Dictionary<string, Queue<GameObject>>();
        if (prefabList == null)
        {
            Debug.LogError("prefabList is null");
        }
    }
    /// <summary>
    /// 根据MapObjectType从对象池中获得物体，可能为空
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject GetByMapObjectType(MapObjectType type)
    {
        GameObject obj = prefabList.GetPrefabByType(type);
        if (obj != null)
        {
            string name = type.ToString();
            //Debug.Log(name);
            if (m_pool.ContainsKey(name))
            {
                if (m_pool[name].Count > 0)
                {
                    GameObject res = m_pool[name].Dequeue();
                    res.SetActive(true);
                    return res;
                }
                else
                {
                    GameObject res = Instantiate(obj, transform);
                    return res;
                }
            }
            else
            {
                GameObject res = obj;

                if (res != null)
                {
                    //Debug.Log($"create {type} poll");
                    m_pool.Add(name, new Queue<GameObject>());
                    res = Instantiate(res, transform);
                    return res;
                }
                else { return null; }
            }
        }
        return null;
    }
    /// <summary>
    /// 从对象池中获得物体，可能为空
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public GameObject Get<T>() where T : class
    {
        Type t = typeof(T);
        string name = t.Name;
        if (m_pool.ContainsKey(name))
        {
            if (m_pool[name].Count > 0)
            {
                GameObject obj = m_pool[name].Dequeue();
                obj.SetActive(true);
                return obj;
            }
            else
            {
                GameObject obj = prefabList.GetPrefabs<T>();
                obj = Instantiate(obj, transform);
                return obj;
            }
        }
        else
        {
            GameObject obj = prefabList.GetPrefabs<T>();

            if (obj != null)
            {
                Debug.Log($"create {name} poll");
                m_pool.Add(name, new Queue<GameObject>());
                obj = Instantiate(obj, transform);
                return obj;
            }
            else {
                Debug.LogError($"No gameobejct has {typeof(T)} component");
                return null;
            }
        }
    }
    /// <summary>
    /// 归还物体
    /// </summary>
    /// <param name="obj"></param>
    public void Return(GameObject obj,MapObjectType type)
    {
        if (obj != null)
        {
            string name = type.ToString();
            if (m_pool.ContainsKey(name))
            {
                obj.SetActive(false);
                m_pool[name].Enqueue(obj);
            }
        }
    }

    /// <summary>
    /// 归还物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    public void Return<T>(GameObject obj) where T : class
    {
        Type t = typeof(T);
        string name = t.Name;
        if (m_pool.ContainsKey(name))
        {
            obj.SetActive(false);
            m_pool[name].Enqueue(obj);
        }
    }

}
