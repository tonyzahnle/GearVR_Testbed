using UnityEngine;
using System.Collections;
using System;

public class PoolableObject : MonoBehaviour {

    public Action<PoolableObject> onObjectExpired;

    bool bActive;

    public bool Active
    {
        get { return bActive; }
    }

    protected void Expire()
    {
        Debug.Log("Object expiring... " + name);
        bActive = false;
        gameObject.SetActive(false);
        GameManager.Instance.StartCoroutine(DelayedExpire()); // Can't delay expire self, throw towards our manager?
    }

    IEnumerator DelayedExpire()
    {
        yield return 0; // Wait a frame before actually expiring the object
        if (onObjectExpired != null)
            onObjectExpired(this);
    }

    public void Activate()
    {
        bActive = true;
        gameObject.SetActive(true);
    }
}
