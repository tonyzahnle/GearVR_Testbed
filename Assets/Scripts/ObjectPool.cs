using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool {
    // Pool spawning intensive objects so we're not instantiating constantly
    Dictionary<string, List<PoolableObject>> objects;

    // Just incase we do need to instantiate later on.  
    // Log warning so we are aware of when these are used.
    Dictionary<string, GameObject> sourceObjects;

    GameObject parent; // For organizational purposes in the hierarchy
    

    public void Initialize(List<GameObject> manifest, int initialPoolSize, GameObject par = null)
    {
        foreach(GameObject gObj in manifest)
        {
            Initialize(gObj, initialPoolSize);
        }
    }

    public void Initialize(GameObject prefab, int initialPoolSize, GameObject par = null)
    {
        // Pre-emptive error handling
        if(prefab == null)
        {
            Debug.LogError("Trying to init memorypool with null prefab");
            return;
        }

        if (objects == null)
            objects = new Dictionary<string, List<PoolableObject>>();
        if (sourceObjects == null)
            sourceObjects = new Dictionary<string, GameObject>();
        if(parent == null)
        {
            if (par != null)
                parent = par;
            else
                parent = new GameObject(prefab.name + "ObjectPool");
        }
        

        List<PoolableObject> lpObjs = new List<PoolableObject>();
        PoolableObject pObj = prefab.GetComponent<PoolableObject>();
        string key = pObj.GetType().ToString();

        if (!sourceObjects.ContainsKey(key))
        {
            sourceObjects.Add(key, prefab);
            if (!objects.ContainsKey(key))
            {
                // Let's start instantiating
                GameObject tempObj;
                for (int i = 0; i < initialPoolSize; ++i)
                {
                    tempObj = GameObject.Instantiate(prefab);
                    tempObj.transform.SetParent(parent.transform);
                    pObj = tempObj.GetComponent<PoolableObject>();
                    lpObjs.Add(pObj);
                    tempObj.SetActive(false); // Hide object
                }

                objects.Add(key, lpObjs);
            }
            else
            {
                Debug.LogError("Trying to add to objectDictionary with already existing key: " + key);
            }
        }
        else
        {
            Debug.LogError("Trying to add to sourceDictionary with already existing key: " + key);
        }

        
    }

    public PoolableObject GetAvailableObject(string type)
    {
        List<PoolableObject> lpObjs;
        if(objects.TryGetValue(type, out lpObjs))
        {
            if (lpObjs.Count > 0)
            {
                PoolableObject pObj = lpObjs[0];
                lpObjs.RemoveAt(0);
                pObj.Activate();
                return pObj;
            }
            else
            {
                Debug.LogWarning("No Available object in pool of type: " + type + "\nInstantiating new object");
                GameObject gObj = sourceObjects[type];
                GameObject tempObj = GameObject.Instantiate(gObj);
                tempObj.transform.SetParent(parent.transform);
                PoolableObject pObj = tempObj.GetComponent<PoolableObject>();
                pObj.Activate();
                return pObj;
            }
        }
        else
        {
            Debug.LogError("Trying to get object of type: " + type + " when type was never initialized in memory pool.");
            return null;
        }
        
    }

    public void ReturnObjectToPool(PoolableObject pObj)
    {
        string type = pObj.GetType().ToString();
        objects[type].Add(pObj);
    }
}
