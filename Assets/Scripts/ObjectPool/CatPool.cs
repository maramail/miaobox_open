/********************************************************************
	created:	2016/09/08
	created:	8:9:2016   22:09
	filename: 	F:\Users\Administrator\Projects\MiaoBox\MiaoBoxMVC\Assets\Scripts\ObjectPool\CatPool.cs
	file path:	F:\Users\Administrator\Projects\MiaoBox\MiaoBoxMVC\Assets\Scripts\ObjectPool
	file base:	CatPool
	file ext:	cs
	author:		Zhou Jingren
	
	purpose:	cat的对象池 inactive时自动回收
*********************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 创建所有猫种类的对象池
/// 每0.1秒会刷新激活组（暂时以隐藏代表对象消失，而不是销毁）
/// </summary>
public class CatPool : MonoBehaviour
{
    private Dictionary<int, ObjectPool> mEffectPools;
    private float mLastDestructTime;
    public static CatPool mInstance = null;

    private int[] mCurrentCats = new int[] { 5,6,7 }; //做好prefab的猫id 临时存储

    public static CatPool GetInstance()
    {
        if (mInstance == null)
            mInstance = GameObject.Find("CatPool").GetComponent<CatPool>();
        return mInstance;
    }
    public void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        mEffectPools = new Dictionary<int, ObjectPool>();
        string catPrefix = "Characters/";
        foreach (int id in mCurrentCats)
        {
            GameObject prefab = Resources.Load(catPrefix + "cat" + id) as GameObject;
            if (prefab == null)
            {
                Debug.LogError("Invalid VFX root: " + catPrefix + "cat" + id);
            }
            mEffectPools.Add(id, new ObjectPool());
            mEffectPools[id].Init("cat" + id, prefab, 0, 0);
            mEffectPools[id].doNotDestruct = true;
            mEffectPools[id].SetRoot(gameObject);
        }
    }

    void Update()
    {
        Loop(Time.deltaTime);
    }
    public ObjectPool GetCatPool(int catId)
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
