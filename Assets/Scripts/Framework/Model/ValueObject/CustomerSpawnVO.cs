using UnityEngine;
using System.Collections;

public class CustomerSpawnVO  {

    public CustomerSpawnVO()
    { }

    public CustomerSpawnVO(int maxCustomer,int currentCustomer,int visibleCustomer, int spawnNum,int defaultSpawnNum,float refreshTime)
    {
        this.MaxCustomer = maxCustomer;
        this.CurrentCustomer = currentCustomer;
        this.VisibleCustomer = visibleCustomer;
        this.SpawnNum = spawnNum;
        this.DefaultSpawnNum = defaultSpawnNum;
        this.RefreshTime = refreshTime;
    }

    /// <summary>
    /// 最大顾客数
    /// (自动属性)
    /// </summary>
    public int MaxCustomer { get; set; }

    /// <summary>
    ///当前顾客数 
    ///(自动属性)
    /// </summary>
    public int CurrentCustomer { get; set; }

    /// <summary>
    /// 可见顾客数
    /// (自动属性)
    /// </summary>
    public int VisibleCustomer { get; set; }

    /// <summary>
    /// 顾客生成数(小于等于默认生成数)
    /// (自动属性)
    /// </summary>
    public int SpawnNum { get; set; }

    /// <summary>
    /// 默认顾客生成数
    /// (自动属性)
    /// </summary>
    public int DefaultSpawnNum { get; set; }

    /// <summary>
    /// 顾客生成时间间隔
    /// (自动属性)
    /// </summary>
    public float RefreshTime { get; set; }
}
