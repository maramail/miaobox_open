using PureMVC.Patterns;
using PureMVC.Interfaces;
using Mono.Data.Sqlite;
using UnityEngine;

public class CustomerProxy : Proxy,IProxy {

    public new const string NAME = "CustomerProxy";

    public CustomerSpawnVO SpawnData { get; set; }
    public CustomerInfoVO Temp_CustomerInfoData { get; set; }

    private UserInfoProxy userInfoProxy;

    public CustomerProxy() : base(NAME)
    {
        SpawnData = new CustomerSpawnVO();
        
    }

    public override void OnRegister()
    {
        Init();
    }

    //Test
    public void Init()
    {
        userInfoProxy = Facade.RetrieveProxy(UserInfoProxy.NAME) as UserInfoProxy;
        DbAccess dbAccess = new DbAccess();
        SqliteDataReader reader = dbAccess.SelectWhere("info_spheres",
                             new string[] { "customer_current" },
                             new string[] { "userid" },
                             new string[] { " = " },
            new string[] { userInfoProxy.UsertData.Id.ToString() });

        if (reader.Read())
        {
            SpawnData.CurrentCustomer = Utils.GetInt(reader["customer_current"]);
        }
        dbAccess.CloseSqlConnection();
        
        SpawnData.RefreshTime = AppConst.CUSTOM_REFRESH_TIME;
        SpawnData.DefaultSpawnNum = AppConst.DEFALUT_SPAWN_NUM;
        SpawnData.VisibleCustomer = AppConst.VISIBLE_CUSTOMER;
        SpawnData.MaxCustomer = AppConst.DEFAULT_MAX_CUSTOMER;
    }

    /// <summary>
    /// 刷新顾客生成相关数据（如刷新时间，生成个数，最大顾客数量等）
    /// </summary>
    public void RefreshCustomerSpawnValue()
    {
        

        if (SpawnData.MaxCustomer - SpawnData.CurrentCustomer > SpawnData.DefaultSpawnNum)
        {
            SpawnData.SpawnNum = SpawnData.DefaultSpawnNum;
        }
        else
        {
            SpawnData.SpawnNum = SpawnData.MaxCustomer - SpawnData.CurrentCustomer;
        }

        Facade.SendNotification(CustomerMediator.REFRESH_CUSTOMER_SPAWN_VALUE, SpawnData);
    }

    /// <summary>
    /// 更新当前顾客数量
    /// </summary>
    /// <param name="currentCustomer"></param>
    public void SetCurrentCustomer(int currentCustomer)
    {
        SpawnData.CurrentCustomer = currentCustomer;
        RefreshCustomerSpawnValue();
    }

    /// <summary>
    /// 获取这种id的猫可以获取的赏金
    /// </summary>
    /// <param name="cattypeid"></param>
    public void AddCustomerModel(int cattypeid)
    {
        ManorInfoProxy manorInfoProxy = Facade.RetrieveProxy(ManorInfoProxy.NAME) as ManorInfoProxy;
        Temp_CustomerInfoData = new CustomerInfoVO(cattypeid, manorInfoProxy.catDic[cattypeid].Money);
        Facade.SendNotification(CustomerMediator.GENERATE_CUSTOMER, Temp_CustomerInfoData);
    }

    /// <summary>
    /// 更新可承载顾客数量
    /// </summary>
    /// <returns></returns>
    public void CaculateMaxCustomer()
    {
        RefreshCustomerSpawnValue();
    }

    public void SaveToDb()
    {
        if (userInfoProxy.UsertData.Id != -1)
        {
            DbAccess dbAccess = new DbAccess();
            dbAccess.UpdateInto("info_spheres",
                new string[] { "customer_current" },
                new string[] { SpawnData.CurrentCustomer.ToString() },
                "userid", userInfoProxy.UsertData.Id.ToString());
            dbAccess.CloseSqlConnection();
        }
    }

}
