/********************************************************************
	created:	2016/08/18
	created:	18:8:2016   23:03
	filename: 	F:\Users\Administrator\Projects\MiaoBox\miaobox\MiaoBoxMVC\Assets\Scripts\Framework\Model\Proxy\UserInfoProxy.cs
	file path:	F:\Users\Administrator\Projects\MiaoBox\miaobox\MiaoBoxMVC\Assets\Scripts\Framework\Model\Proxy
	file base:	UserInfoProxy
	file ext:	cs
	author:		Zhou Jingren
	
	purpose:	操作和用户相关数据
*********************************************************************/
using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using LitJson;
using Mono.Data.Sqlite;
using System.Collections.Generic;

public class UserInfoProxy : Proxy, IProxy
{
    public new const string NAME = "UserInfoProxy";

    //用于存储用户相关信息 切记不能存储为公共的，这是私密数据。只能通过方法或者属性设置
    private new UserInfoVO Data ;

    public UserInfoVO UsertData
    {
        get{
            return Data;
        }
    }

    private bool isFirstGet = true;
    

    public UserInfoProxy() : base(NAME)
    {
        Data = new UserInfoVO();
    }

    //存储模型位置与建筑信息
    public  Dictionary<int, BuildModelVo> BuildModelVoDic = new Dictionary<int, BuildModelVo>() ;
 

    public void Init()
    {
        DbAccess dbAccess = new DbAccess();
        string query = string.Format("SELECT * FROM info_users WHERE id={0}", Data.Id);
        SqliteDataReader reader = dbAccess.ExecuteQuery(query);
        if (reader.Read())
        {
            Data.PlayerName = Utils.GetString(reader["playername"]);
            Data.Level = Utils.GetInt(reader["lv"]);
            Data.Gold = Utils.GetInt(reader["gold"]);
            Data.Diamond = Utils.GetInt(reader["diamond"]);
            Data.Exp = Utils.GetInt(reader["exp"]);
            Data.Blueprint = Utils.GetString(reader["blueprints_budin"]);
            Data.facility1 = Utils.GetInt(reader["facility1"]);
            Data.facility2 = Utils.GetInt(reader["facility2"]);
            Data.facility3 = Utils.GetInt(reader["facility3"]);
            Data.facility4 = Utils.GetInt(reader["facility4"]);
        }
        dbAccess.CloseSqlConnection();
       
    }

 
    public void SetGold(int value)
    {
        Data.Gold = value;

        if (!AppConst.USER_CACHE)
        {
            DbAccess dbAccess = new DbAccess();
            dbAccess.UpdateInto("info_users",
                new string[] { "gold" },
                new string[] { Data.Gold.ToString() },
                "id", Data.Id.ToString());
            dbAccess.CloseSqlConnection();
        }

        RefreshUserInfoValue();
    }

    public void SetDiamond(int value)
    {
        Data.Diamond = value;

        if (!AppConst.USER_CACHE)
        {
            DbAccess dbAccess = new DbAccess();
            dbAccess.UpdateInto("info_users",
                new string[] { "diamond" },
                new string[] { Data.Diamond.ToString() },
                "id", Data.Id.ToString());
            dbAccess.CloseSqlConnection();
        }

        RefreshUserInfoValue();
    }

    public void setEXP(int value)
    {
       
        Data.Exp = value ;
        
        if(!AppConst.USER_CACHE)
        {
            DbAccess dbAccess = new DbAccess();
            dbAccess.UpdateInto("info_users", new string[] { "exp" }, 
                new string[] { Data.Exp.ToString() },
                "id" , Data.Id.ToString());
            dbAccess.CloseSqlConnection();
        }
        RefreshUserInfoValue();
    }

    public void setBlueprintsbudin(string value)
    {
        
        Data.Blueprint = value ;
        Debug.Log(value);
        if(!AppConst.USER_CACHE)
        {
            DbAccess dbAccess = new DbAccess();
            dbAccess.UpdateInto("info_users", new string[] { "blueprints_budin" }, 
                new string[] { Data.Blueprint  } ,
                "id" , Data.Id.ToString());
            dbAccess.CloseSqlConnection();
        }
        RefreshUserInfoValue();
    }
    /// <summary>
    /// 设置模型建筑与位置信息
    /// </summary>
    /// <param name="vo"></param>
    public void setfacility(BuildModelVo vo)
    {
        switch (vo.ModeltrsId)
        {
            case 1:
                Data.facility1 = vo.Modelid;
                break;
            case 2:
                Data.facility2 = vo.Modelid;
                break;
            case 3:
                Data.facility3 = vo.Modelid;
                break;
            case 4:
                Data.facility4= vo.Modelid;
                break;
            default: break; 

        }
        consumeBlueprint(vo.Modelid);
        if (!AppConst.USER_CACHE)
        {
            DbAccess dbAccess = new DbAccess();
            dbAccess.UpdateInto("info_users", new string[] { "blueprints_budin", "facility1", "facility2", "facility3", "facility4" },
                new string[] { Data.Blueprint,Data.facility1 .ToString(),Data .facility2.ToString(),Data .facility3.ToString(),Data.facility4.ToString() },
                "id", Data.Id.ToString());
            dbAccess.CloseSqlConnection();
        }

        RefreshUserInfoValue();
        setbuildmodedic();

    }


    private void setbuildmodedic()
    {
        for (int i = 1; i < 5; i++)
        {
            if (!BuildModelVoDic.ContainsKey(i))
            {
                BuildModelVo Vo = new BuildModelVo();
                Vo.ModeltrsId = i;
                switch (i)
                {
                    case 1:
                        Vo.Modelid = Data.facility1;
                        break;
                    case 2:
                        Vo.Modelid = Data.facility2;
                        break;
                    case 3:
                        Vo.Modelid = Data.facility3;
                        break;
                    case 4:
                        Vo.Modelid = Data.facility4;
                        break;
                }
                BuildModelVoDic.Add(i, Vo);
            }
            else
            {
                switch (i)
                {
                    case 1:
                        BuildModelVoDic[i].Modelid = Data.facility1;
                        break;
                    case 2:
                        BuildModelVoDic[i].Modelid = Data.facility2;
                        break;
                    case 3:
                        BuildModelVoDic[i].Modelid = Data.facility3;
                        break;
                    case 4:
                        BuildModelVoDic[i].Modelid = Data.facility4;
                        break;
                }

            }

        }
        

    }
    /// <summary>
    /// 发送模型初始值信息
    /// </summary>
    public void sendinitmodeldata()
    {

        setbuildmodedic();
        SendNotification(BuildingModelMediator.INITBUILTMODEL, BuildModelVoDic);
        
       
        
    }
    /// <summary>
    /// 读取玩家模型与位置信息
    /// </summary>
    /// <returns></returns>
    public BuildModelVo getmodeldata(int modelras)
    {

        if (BuildModelVoDic.ContainsKey(modelras))
        {   
            
            return BuildModelVoDic[modelras];
        }
        else
        {

            return null;
        }
        

    }
    /// <summary>
    /// 读取玩家持有的建筑蓝图
    /// </summary>
    /// <returns>The blueprintsbudin.</returns>
    public string getBlueprintsbudin()
    {
        // test

        //  return "1,4,3";

        //        return  Data.Blueprint;

        //DbAccess dbAccess = new DbAccess();
        //SqliteDataReader reader = dbAccess.SelectWhere("info_users", new string[] {"blueprints_budin" },
        //    new string[] { "id" }, new string[] {"="} , new string[]{Data.Id.ToString()});
        //if (reader.Read())
        //{
        //    Data.Blueprint = reader.GetString(reader.GetOrdinal("blueprints_budin"));
        //}
        //dbAccess.CloseSqlConnection();
      
        return Data.Blueprint;
         
    }
    /// <summary>
    /// 消耗蓝图
    /// </summary>
    /// <param name="Bliprintid"></param>
    public void consumeBlueprint(int Blueprintid)
    {
        string aim = Blueprintid.ToString();
        string[] temp = Data.Blueprint.Split(',');
        for (int i = 0; i < temp.Length; i++)
        {
            if (aim.Equals(temp[i]))
            {
                Debug.Log(temp[i]);
                int index_aim= Data.Blueprint.IndexOf(aim);

                if (index_aim == 0)
                {
                    if (Data.Blueprint.Length > 1)
                    {
                        Data.Blueprint = Data.Blueprint.Remove(index_aim, 2);

                    }
                    else
                    {
                        Data.Blueprint = Data.Blueprint.Remove(index_aim);
                    }
                   
                }
                else  
                {
                    index_aim--;
                    Data.Blueprint = Data.Blueprint.Remove(index_aim, 2);
                }
                
               
                 
                Debug.Log(Data.Blueprint);
                break;
            }
        }

    }

    /// <summary>
    /// 获取当前的经验
    /// </summary>
    /// <returns>The EX.</returns>
    public int getEXP()
    {

        return Data.Exp;
        //DbAccess dbAccess = new DbAccess();
        //SqliteDataReader reader = dbAccess.SelectWhere("info_users", new string[] { "exp" },
        //    new string[] { "id" }, new string[] { "=" }, new string[] { Data.Id.ToString() });
        //if (reader.Read())
        //{
        //    Data.Exp = reader.GetInt32(reader.GetOrdinal("exp"));
        //}
        //dbAccess.CloseSqlConnection();

        
    }
    /// <summary>
    /// 获取钻石数目
    /// </summary>
    /// <returns>The diamond.</returns>
    public int getDiamond()
    {
        return 40;
    }

    /// <summary>
    /// 刷新用户数据(金币钻石体力等)
    /// </summary>
    public void RefreshUserInfoValue()
    {
        if (isFirstGet)
        {
            Init();
            isFirstGet = false;
        }

        if (!AppConst.USER_CACHE)
        {
            DbAccess dbAccess = new DbAccess();
            string query = string.Format("SELECT * FROM info_users WHERE id={0}", Data.Id);
            SqliteDataReader reader = dbAccess.ExecuteQuery(query);
            if (reader.Read())
            {
                Data.PlayerName = Utils.GetString(reader["playername"]);
                Data.Level = Utils.GetInt(reader["lv"]);
                Data.Gold = Utils.GetInt(reader["gold"]);
                Data.Diamond = Utils.GetInt(reader["diamond"]);
                Data.Exp = Utils.GetInt(reader["exp"]);
                Data.Blueprint = Utils.GetString(reader["blueprints_budin"]);
                Data.facility1 = Utils.GetInt(reader["facility1"]);
                Data.facility2 = Utils.GetInt(reader["facility2"]);
                Data.facility3 = Utils.GetInt(reader["facility3"]);
                Data.facility4 = Utils.GetInt(reader["facility4"]);
            }
            dbAccess.CloseSqlConnection();
            
        }

        Facade.SendNotification(MainMenuUIMediator.REFRESH_USER_INFO_VALUE, Data);
    }

    /// <summary>
    /// 缓存写回数据库 不必每次修改缓存都写回
    /// </summary>
    public void SaveToDb()
    {
        if (Data.Id != -1)
        {
            DbAccess dbAccess = new DbAccess();
            dbAccess.UpdateInto("info_users",
                new string[] { "lv","gold", "diamond","exp" , "blueprints_budin","facility1", "facility2", "facility3", "facility4" },
                new string[] { Data.Level.ToString(),Data.Gold.ToString(), Data.Diamond.ToString(),Data.Exp.ToString(), Data.Blueprint ,Data.facility1.ToString(), Data.facility2.ToString() ,Data.facility3.ToString(), Data.facility4.ToString() },
                "id", Data.Id.ToString());
            dbAccess.CloseSqlConnection();
        }
    }

    /// <summary>
    /// 用模板于读取每个类型的值
    /// </summary>
    /// <returns>The user data.</returns>
    /// <param name="key">Key.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
 /*   private  T getUserData<T>(string key)
    { 
        DbAccess dbAccess = new DbAccess();
        string query = string.Format("SELECT * FROM info_users WHERE id={0}", Data.Id);
        SqliteDataReader reader = dbAccess.ExecuteQuery(query);
        T blueprint = new T();
        if (reader.Read())
        {
            if (typeof(T) == "int")
            {
                blueprint = reader.GetInt32(reader.GetOrdinal(key));
            }
            else if (typeof(T) == "string")
            {
                blueprint = reader.GetString(reader.GetOrdinal(key));
            }
        }
        dbAccess.CloseSqlConnection();
        return blueprint;
    }
*/
}
