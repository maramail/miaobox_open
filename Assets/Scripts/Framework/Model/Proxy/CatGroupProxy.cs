using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LitJson;
using Mono.Data.Sqlite;
using PureMVC.Interfaces;
using PureMVC.Patterns;

public class CatGroupProxy : Proxy
{
    public class CatGroupInfo
    {
        public int id;
        public int catTypeId;
        public int groupId;
        public int userId;
    }

    public new const string NAME = "CatGroupMenuProxy";

    public DbAccess dbAcces;
    private int mUserId = -1;
    private Dictionary<int, List<CatGroupInfo>> mCatGroupInfoDic;
    private Dictionary<int, stat_catRow> mCatStatDic;

    public CatGroupProxy() : base(NAME)
    {
       
    }

    public override void OnRegister()
    {
       
    }
    /// <summary>
    /// 与服务器通讯  获得猫分组信息
    /// 信息内容   标签ID 标签名字
    /// </summary>
    public void SendCatGroupTitle(object data)
    {
        JsonData group = new JsonData();
        int count = 0;
        List<system_catgroupRow> catGroupRows = system_catgroup.GetInstance().rowList;
        for (int i = 0; i < catGroupRows.Count; i++)
        {
            JsonData child = new JsonData();
            child["id"] = catGroupRows[i].id;
            child["name"] = catGroupRows[i].name;
            group[count.ToString()] = child;
            count++;
        }
        group["count"] = count;

        JsonData type = (JsonData)data;
        switch (type["type"].ToString())
        {
            case CatGroupMenuMediator.CAT_GROUP_TITLE:
                SendNotification(CatGroupMenuMediator.CAT_GROUP_TITLE, group);
                break;
            case CatGroupMenuMediator.CAT_MOVE_GROUP_TITLE:
                SendNotification(CatGroupMenuMediator.CAT_MOVE_GROUP_TITLE, group);
                break;
        }
    }
    /// <summary>
    /// 初始化mCatGroupInfoDic mCatStatDic
    /// </summary>
    public void LoadCatGroupInfo()
    {
        Debug.Log("LoadCatGroupInfo()");
        mUserId = (Facade.RetrieveProxy(UserInfoProxy.NAME) as UserInfoProxy).UsertData.Id;
        mCatGroupInfoDic = new Dictionary<int, List<CatGroupProxy.CatGroupInfo>>();
        DbAccess dbAccess = new DbAccess();
        string query = string.Format("SELECT * FROM info_cats WHERE userid={0}", mUserId);
        SqliteDataReader reader = dbAccess.ExecuteQuery(query);
        while (reader.Read())
        {
            CatGroupInfo groupInfo = new CatGroupProxy.CatGroupInfo()
            {
                id = Utils.GetInt(reader["id"]),
                catTypeId = Utils.GetInt(reader["cattypeid"]),
                groupId = Utils.GetInt(reader["groupId"]),
                userId = Utils.GetInt(reader["userid"])
            };
            int groupId = reader.GetInt32(reader.GetOrdinal("groupId"));
            if (!mCatGroupInfoDic.ContainsKey(groupId))
            {
                mCatGroupInfoDic.Add(groupId, new List<CatGroupInfo>() { groupInfo });
            }
            else
            {
                mCatGroupInfoDic[groupId].Add(groupInfo);
            }
        }
        dbAccess.CloseSqlConnection();

        mCatStatDic = new Dictionary<int, stat_catRow>();
        foreach (stat_catRow catStat in stat_cat.GetInstance().rowList)
        {
            mCatStatDic.Add(catStat.id, catStat);
        }
    }

    /// <summary>
    /// 与服务器通讯 获得猫分组内容信息（分组内 猫的 名字 等等）
    /// 
    /// data 组ID（测试用）
    /// </summary>
    public void SendCatGroupInfo(object data)
    {
        JsonData group = (JsonData)data;
        int groupId = (int)group["id"];

        JsonData cat = new JsonData();
        int count = 0;
        if (mCatGroupInfoDic.ContainsKey(groupId))
        {
            foreach (CatGroupInfo groupInfo in mCatGroupInfoDic[groupId])
            {
                int cattype = groupInfo.catTypeId;
                int id = groupInfo.id;
                stat_catRow catStat = mCatStatDic[cattype];
                JsonData child = new JsonData();
                child["id"] = id;
                child["cattypeid"] = cattype;
                child["catName"] = catStat.name;
                child["lv"] = catStat.lv;       
                child["atk"] = catStat.atk;
                child["groupId"] = groupId;
                child["about"] = catStat.about;
                child["cooldown"] = catStat.cooldown;
                cat[count.ToString()] = child;
                count++;
            }
        }

        cat["count"] = count;
        cat["groupid"] = groupId;
        SendNotification(CatGroupMenuMediator.CAT_GROUP_INFO, cat);
    }


    /// <summary>
    /// 发送战斗场景 猫分组信息 
    /// </summary>
    /// <param name="data"></param>
    public void SendBattleGroupInfo(object data )
    {
        if(mCatGroupInfoDic==null)
        {

            LoadCatGroupInfo();

        }
        JsonData group = (JsonData)data;
        int groupId = (int)group["id"];

        JsonData cat = new JsonData();
        int count = 0;
        if (mCatGroupInfoDic.ContainsKey(groupId))
        {
            foreach (CatGroupInfo groupInfo in mCatGroupInfoDic[groupId])
            {
                int cattype = groupInfo.catTypeId;
                int id = groupInfo.id;
                stat_catRow catStat = mCatStatDic[cattype];
                JsonData child = new JsonData();
                child["id"] = id;
                child["cattypeid"] = cattype;
                child["catName"] = catStat.name;
                child["lv"] = catStat.lv;
               
                child["atk"] = catStat.atk;
                child["groupId"] = groupId;
                child["about"] = catStat.about;
                cat[count.ToString()] = child;
                count++;
            }
        }

        cat["count"] = count;
        cat["groupid"] = groupId;
        SendNotification(BattleMediator.BATTLE_CAT_GROUP_INFO, cat);
    }
    public void SwitchCatGroup(object data)
    {
        JsonData cat = (JsonData)data;

        int newId = (int)cat["newId"];
        int oldId = (int)cat["oldId"];
        int count = (int)cat["count"];
        bool islimit = limitCatGroupNum(newId, count);
        if (islimit)
        {
            SendNotification(CatGroupMenuMediator.CAT_SWITCH_GROUP_FAIL);

        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                int id = (int)cat[i.ToString()];
                foreach (CatGroupInfo groupInfo in mCatGroupInfoDic[oldId])
                {
                    if (groupInfo.id == id)
                    {
                        Debug.Log("%%%%%" + groupInfo.id);
                        mCatGroupInfoDic[oldId].Remove(groupInfo);
                        if (mCatGroupInfoDic.ContainsKey(newId))
                        {
                            mCatGroupInfoDic[newId].Add(groupInfo);
                        }
                        else
                        {
                            mCatGroupInfoDic.Add(newId, new List<CatGroupInfo>() { groupInfo });
                        }
                        groupInfo.groupId = newId;
                        break;
                    }
                }
            }


        }

        JsonData group = new JsonData();
        group["id"] = newId;
        SendNotification(CatGroupMenuMediator.CAT_SWITCH_GROUP, group);
    }
    /// <summary>
    /// 限制猫分组人数
    /// </summary>
    public  bool limitCatGroupNum(int groupid,int addnum)
    {

        if (mCatGroupInfoDic == null) { LoadCatGroupInfo(); }
     //  LoadCatGroupInfo();


       
        switch (groupid)
        {
            case 1:
                if (!mCatGroupInfoDic.ContainsKey(groupid))
                {
                    return limitCondition(5, 0, addnum);

                }
                return limitCondition(5, mCatGroupInfoDic[groupid].Count, addnum);
               
            case 2:
                if (!mCatGroupInfoDic.ContainsKey(groupid))
                {
                    return limitCondition(5, 0, addnum);

                }
                return  limitCondition(5, mCatGroupInfoDic[groupid].Count, addnum);
                
            case 3:
                if (!mCatGroupInfoDic.ContainsKey(groupid))
                {
                    return limitCondition(5, 0, addnum);

                }
                return limitCondition(5, mCatGroupInfoDic[groupid].Count, addnum);
                
            case 4:
                if (!mCatGroupInfoDic.ContainsKey(groupid))
                {
                    return limitCondition(1, 0, addnum);

                }
                return  limitCondition(1, mCatGroupInfoDic[groupid].Count, addnum);
                
            case 5:
                if (!mCatGroupInfoDic.ContainsKey(groupid))
                {
                    return limitCondition(9, 0, addnum);

                }
                return  limitCondition(9, mCatGroupInfoDic[groupid].Count, addnum);
                
            default:
                return false;
                



        }
        
        
    }


    private bool  limitCondition(int maxNum,int currentNum,int addNum)
    {
        int end = currentNum + addNum;
        if (end > maxNum)
        {
            return true;

        }
        return false;


    }
    /// <summary>
    /// 添加雇佣的猫，存入缓存字典
    /// </summary>
    /// <param name="id"></param>
    /// <param name="groupid"></param>
    /// <param name="typeid"></param>
    /// <param name="userid"></param>
    public void  addCatByEmloyee(int id ,int groupid,int typeid,int userid)
    {
        CatGroupInfo info = new CatGroupInfo();
        info.id = id;
        info.groupId = groupid;
        info.catTypeId = typeid;
        info.userId = userid;
        if (!mCatGroupInfoDic.ContainsKey(info.groupId))
        {
            mCatGroupInfoDic.Add(info.groupId, new List<CatGroupInfo>() {info });
        }
        else
        {
            mCatGroupInfoDic[info.groupId].Add(info);
        }

    }
    public void DeleteCat(object data)
    {
        JsonData catarray = (JsonData)data;

        int count = (int)catarray["count"];

        for (int i = 0; i < count; i++)
        {
            int id = (int)catarray[i.ToString()];
            foreach (KeyValuePair<int, List<CatGroupInfo>> kvp in mCatGroupInfoDic)
            {
                foreach (CatGroupInfo groupInfo in kvp.Value)
                {
                    if (groupInfo.id == id)
                    {
                        kvp.Value.Remove(groupInfo);
                        break;
                    }
                }
            }
        }

        JsonData showGroup = new JsonData();
        showGroup["group"] = catarray["group"];
        SendNotification(CatGroupMenuMediator.CAT_DELETE, showGroup);
    }

   

    /// <summary>
    /// 缓存写回数据库 不必每次修改缓存都写回
    /// </summary>
    public void SaveToDb()
    {
        Debug.Log("CatGroupInfoSaved");
        if (mUserId != -1)
        {
            dbAcces = new DbAccess();
            dbAcces.SqlRequest(string.Format("delete from info_cats where userid={0} ", mUserId));
            foreach (KeyValuePair<int, List<CatGroupInfo>> kvp in mCatGroupInfoDic)
            {
                foreach (CatGroupInfo groupInfo in kvp.Value)
                {
                    string[] values = new string[]
                    {
                        groupInfo.id.ToString(),
                        groupInfo.catTypeId.ToString(),
                        groupInfo.groupId.ToString(),
                        groupInfo.userId.ToString(),
                        "0",
                        string.Format("'{0}'",System.DateTime.Now.ToString()),
                        "0",
                        "0",
                        "0"
                    };
                    dbAcces.InsertInto("info_cats", values);
                }
            }
            dbAcces.CloseSqlConnection();
        }
    }

    //     public override void OnRemove()
    //     {
    //         dbAcces.CloseSqlConnection();
    //     }
}
