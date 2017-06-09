using System;
using System.Collections.Generic;
using PureMVC.Patterns;
using Mono.Data.Sqlite;
using UnityEngine;
using System.Text;


public class BuildBlueprintProxy : Proxy
{
    public static new string NAME = "BuildBlueprintProxy";
    //获取玩家身上的建筑
    private UserInfoProxy userInfoProxy;

//    public DbAccess dbAcces = new DbAccess();

    private Dictionary<int,stat_blueprintRow > mBuildBlueprintDict = new Dictionary<int, stat_blueprintRow>();
    private Dictionary<int, stat_buildingRow> mBuildDict = new Dictionary<int, stat_buildingRow>();



    public BuildBlueprintProxy ():base(NAME)
    {
        
    }

    public override void OnRegister()
    {
        userInfoProxy = AppFacade.getInstance.RetrieveProxy(UserInfoProxy.NAME) as UserInfoProxy;

        foreach (stat_blueprintRow blueprint  in stat_blueprint.GetInstance().rowList)
        {
            mBuildBlueprintDict[blueprint.id] = blueprint;
            
        }
        foreach (stat_buildingRow build in stat_building.GetInstance().rowList)
        {
            mBuildDict[build.id] = build;
        }
    }

    public override void OnRemove()
    {
        //移除的时候，把本身的变量也移除掉
        userInfoProxy = null;
//        dbAcces = null;
        mBuildBlueprintDict = null;
        mBuildDict = null;
    }

    /// <summary>
    /// 注册proxy 的时候，也把数据从数据库中读取到内存中，这样每次显示的时候不用再次读取
    /// 不好的地方是如果数据库修改的话，需要维护（由于本次读取的是静态数据表， 不存在在运行时的变化）
    /// </summary>
    private void initBuildingBlueprintData ()
    {
//        string query = string.Format("SELECT * FROM stat_blueprint");
//        SqliteDataReader reader = dbAcces.ExecuteQuery(query);
//        while (reader.Read())
//        {
//            BuildingBlueprintVO vo = new BuildingBlueprintVO();
//            vo.ID = reader.GetInt32(reader.GetOrdinal("id"));
//            vo.type = reader.GetInt32(reader.GetOrdinal("type"));
//            vo.name = reader.GetString(reader.GetOrdinal("name"));
//            vo.description = reader.GetString(reader.GetOrdinal("description"));
//            vo.pointto = reader.GetInt32(reader.GetOrdinal("pointto"));
//            vo.const_exp = reader.GetInt32(reader.GetOrdinal("const_exp"));
//            vo.cost_diamond = reader.GetInt32(reader.GetOrdinal("cost_diamond"));
//            vo.weekday = reader.GetInt32(reader.GetOrdinal("weekday"));
//            vo.timegap = reader.GetInt32(reader.GetOrdinal("timegap"));
//            //加入到缓存数组中
//            if (mBuildBlueprintDict.ContainsKey(vo.ID))
//            {
//                mBuildBlueprintDict[vo.ID].Add(vo);
//            }
//            else
//            {
//                List<BuildingBlueprintVO> list = new List<BuildingBlueprintVO>();
//                list.Add(vo);
//                mBuildBlueprintDict[vo.ID] = list;
//            }
//        }
//        dbAcces.CloseSqlConnection();

//        foreach (stat_blueprintRow  blueprint in stat_blueprint.GetInstance().rowList)
//        {
//            if (mBuildBlueprintDict.ContainsKey(blueprint.type))
//            {
//                mBuildBlueprintDict[blueprint.type].Add(blueprint);
//            }
//            else
//            {
//                List<stat_blueprintRow> list = new List<stat_blueprintRow>();
//                list.Add(blueprint);
//                mBuildBlueprintDict[blueprint.type] = list;
//            }
//        }







        //获取 建筑蓝图成功，发送消息
    }
    /// <summary>
    /// 发送静态蓝图消息
    /// </summary>
    public void sendBuildingBlueprintData()
    {
        SendNotification(BuildingBlueprintMediator.BUILDING_BLUEPRINT_DATA, mBuildBlueprintDict);
    }
    /// <summary>
    /// 发送静态建筑消息
    /// </summary>
    public void sendBuildStatData()
    {
        SendNotification(BuildingBlueprintMediator.BUILD_STAT_DATA, mBuildDict);

    }
    /// <summary>
    /// 发送建筑消息 存储在玩家数据库中每次都需要读取
    /// </summary>
    public void sendBuildData()
    {
        string building = userInfoProxy.getBlueprintsbudin();
        if (  building.Equals(""))
        {
            building = null;
        }
        Debug.Log("sendBuildData()");
        SendNotification(BuildingBlueprintMediator.BUILDING_DATA,building);
    }
   /// <summary>
   /// 发送模型初始的信息
   /// </summary>
    public void sendBulidModeldata()
    {


        userInfoProxy.sendinitmodeldata();
       

    }
    /// <summary>
    /// 发送改变模型的信息 
    /// </summary>
    public void sendchangemodeldata(BuildModelVo  _vo)
    {
        BuildModelVo vo = userInfoProxy.getmodeldata(_vo .ModeltrsId);
        SendNotification(BuildingModelMediator.CHANGEMODEL, vo);
    }
    /// <summary>
    /// 购买蓝图 ,  花费经验和钻石，获取建筑
    /// </summary>
    public void purchaseBlueprint(System. Object data)
    {
        
        List<int > selectedItems = (List<int >)data;
        int needEXP = 0;
        int needDiamond = 0;
        StringBuilder build = new StringBuilder( userInfoProxy.getBlueprintsbudin());
        bool isRemoveFirstChar = false;
        if (build.Length <= 0)
            isRemoveFirstChar = true;
        foreach(int id  in selectedItems)
        {
            stat_blueprintRow blueprint = mBuildBlueprintDict[id];
            needEXP += blueprint.cost_exp;
            needDiamond += blueprint.cost_diamond;
            build.AppendFormat(",{0}", id);
        }
        //移除第一个分号
        if (build.Length >0 && isRemoveFirstChar )
        {
            build.Remove(0, 1);
        }


        int ownExp = userInfoProxy.getEXP();
       
        int ownDiamond = userInfoProxy.getDiamond();

        if (needEXP <= ownExp && needDiamond <= ownDiamond)
        {
            userInfoProxy.SetDiamond(ownDiamond - needDiamond);
            userInfoProxy.setEXP(ownExp - needEXP);
            
            userInfoProxy.setBlueprintsbudin(build.ToString());
            //刷新宝石，建筑信息
            
            AppFacade.GetInstance().SendNotification(NotiConst.GET_USER_INFO_VALUE);
           
            SendNotification(BuildingBlueprintMediator.PURCHASE_BLUEPRINT_RESULT, true);
             
        }
        else
        {
            SendNotification(BuildingBlueprintMediator.PURCHASE_BLUEPRINT_RESULT, false);
        }

    }

}

