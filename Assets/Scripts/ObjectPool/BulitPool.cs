using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulitPool : MonoBehaviour {

    private Dictionary<int, ObjectPool> mEffectPools;
    private float mLastDestructTime;
    public static BulitPool mInstance = null;

    private int[] mCurrentCats = new int[] { 1,2,3,4 ,5,6}; //做好prefab的建筑id 临时存储

    public static BulitPool GetInstance()
    {
        if (mInstance == null)
            mInstance = GameObject.Find("BulitPool").GetComponent<BulitPool>();
        return mInstance;
    }
    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        mEffectPools = new Dictionary<int, ObjectPool>();
        string BuiltPrefix = "Bulit/";
        foreach (int id in mCurrentCats)
        {
            GameObject prefab = Resources.Load(BuiltPrefix + "Bulit" + id) as GameObject;
            if (prefab == null)
            {
                Debug.LogError("Invalid VFX root: " + BuiltPrefix + "Bulit" + id);
            }
            mEffectPools.Add(id, new ObjectPool());
            mEffectPools[id].Init("Bulit" + id, prefab, 0, 0);
            mEffectPools[id].doNotDestruct = true;
            mEffectPools[id].SetRoot(gameObject);
        }
    }

    void Update()
    {
        Loop(Time.deltaTime);
    }
    public ObjectPool GetBulitPool(int catId)
    {
        return mEffectPools[catId];
    }
    public void Loop(float deltaTime)
    {
        if (Time.time - mLastDestructTime > 0.1f)
        {
            mLastDestructTime = Time.time;
            foreach (ObjectPool p in mEffectPools.Values)
            {
                if (null != p)
                    p.AutoDestruct();
            }
        }
    }
    public void DestructAll()
    {
        foreach (ObjectPool pool in mEffectPools.Values)
        {
            pool.DestructAll();
        }
    }

}
