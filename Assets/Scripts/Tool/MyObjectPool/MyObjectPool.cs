using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTool.ObjectPool
{
    public class MyObjectPool<T> where T : Enum
    {
        private Dictionary<T, Queue<GameObject>> poolDictionary;
        private Dictionary<T, GameObject> prefabDictionary;

        /// <summary>
        /// 生成的对象的父物体
        /// </summary>
        private GameObject parent;

        public MyObjectPool(Dictionary<T, GameObject> prefabDictionary, GameObject parent = null)
        {
            this.parent = parent;
            this.prefabDictionary = prefabDictionary;
            poolDictionary = new Dictionary<T, Queue<GameObject>>();
            foreach (T key in prefabDictionary.Keys)
            {
                poolDictionary[key] = new Queue<GameObject>();
            }
        }

        public GameObject GetObject(T key)
        {
            if (poolDictionary[key].Count == 0)
            {
                GameObject newObject;
                if (parent != null)
                    newObject = GameObject.Instantiate(prefabDictionary[key], parent.transform);
                else
                    newObject = GameObject.Instantiate(prefabDictionary[key]);
                poolDictionary[key].Enqueue(newObject);
            }
            GameObject obj = poolDictionary[key].Dequeue();
            obj.SetActive(true);
            return obj;
        }

        public void ReturnObject(T key, GameObject obj)
        {
            obj.SetActive(false);
            poolDictionary[key].Enqueue(obj);
        }

    }
}

