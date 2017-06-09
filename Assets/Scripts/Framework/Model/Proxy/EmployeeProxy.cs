using PureMVC.Patterns;
using PureMVC.Interfaces;
using Mono.Data.Sqlite;
using UnityEngine;

public class EmployeeProxy : Proxy, IProxy
{
    public new const string NAME = "EmployeeProxy";

    public UserInfoProxy userInfoProxy;
    public ManorInfoProxy manorInfoProxy;
    public EmployeeInfoVO Temp_EmployeeInfoData;
    public CatGroupProxy catgroupproxy;
    private EnumGlobal.FailType failtype = EnumGlobal.FailType.Money;

    public EmployeeProxy() : base(NAME)
    {

    }
  
    public override void OnRegister()
    {
        userInfoProxy = Facade.RetrieveProxy(UserInfoProxy.NAME) as UserInfoProxy;
        manorInfoProxy = Facade.RetrieveProxy(ManorInfoProxy.NAME) as ManorInfoProxy;
        catgroupproxy = Facade.RetrieveProxy(CatGroupProxy.NAME) as CatGroupProxy;
    }

    public void AddEmployeeModel(int cattypeid)
    {
        ManorInfoProxy manorInfoProxy = Facade.RetrieveProxy(ManorInfoProxy.NAME) as ManorInfoProxy;
        Temp_EmployeeInfoData = new EmployeeInfoVO(
                                                       cattypeid, 
                                                       manorInfoProxy.catDic[cattypeid].Name,
                                                       manorInfoProxy.catDic[cattypeid].Level, 
                                                       manorInfoProxy.catDic[cattypeid].Evo, 
                                                       manorInfoProxy.catDic[cattypeid].Iq, 
                                                       manorInfoProxy.catDic[cattypeid].Atk, 
                                                       manorInfoProxy.catDic[cattypeid].React,
                                                       manorInfoProxy.catDic[cattypeid].Skill,
                                                       manorInfoProxy.catDic[cattypeid].About,
                                                       manorInfoProxy.catDic[cattypeid].HirePrice
                                                  );
        Facade.SendNotification(EmployeeMediator.GENERATE_EMPLOYEE, Temp_EmployeeInfoData);
    }

    public void EmployEmployee(int cattypeid)
    {

        if (manorInfoProxy.catDic[cattypeid].HirePrice <= userInfoProxy.UsertData.Gold)
        {
            bool islimt = catgroupproxy.limitCatGroupNum(5, 1);
            if (islimt)
            {
                failtype = EnumGlobal.FailType.LimitNum;

                Facade.SendNotification(EmployeeMediator.EMPLOY_FAILURE, failtype);

            }
            else
            {
                DbAccess dbAccess = new DbAccess();
                dbAccess.InsertIntoSpecific("info_cats",
                                           new string[] { "cattypeid", "groupId", "userid" },
                   new string[] { cattypeid.ToString(), "5", userInfoProxy.UsertData.Id.ToString() });
                SqliteDataReader read = dbAccess.ReadMaxIDFromTable("info_cats");
                int id = Utils.GetInt(read[0]);
                dbAccess.CloseSqlConnection();
                catgroupproxy.addCatByEmloyee(id, 5, cattypeid, userInfoProxy.UsertData.Id);
                userInfoProxy.SetGold(userInfoProxy.UsertData.Gold - manorInfoProxy.catDic[cattypeid].HirePrice);

                Facade.SendNotification(EmployeeMediator.EMPLOY_SUCCESS);

            }
            
        }
        else
        {
            failtype = EnumGlobal.FailType.Money;

            Facade.SendNotification(EmployeeMediator.EMPLOY_FAILURE, failtype);
        }


    }

}

