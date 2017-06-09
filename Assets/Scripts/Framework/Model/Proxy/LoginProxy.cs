/********************************************************************
	created:	2016/08/18
	created:	18:8:2016   22:20
	filename: 	f:\users\administrator\projects\miaobox\miaobox\miaoboxmvc\assets\scripts\framework\model\proxy\loginproxy.cs
	file path:	f:\users\administrator\projects\miaobox\miaobox\miaoboxmvc\assets\scripts\framework\model\proxy
	file base:	loginproxy
	file ext:	cs
	author:		Zhou Jingren
	
	purpose:	登陆Proxy
*********************************************************************/
using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using LitJson;
using Mono.Data.Sqlite;

public class LoginProxy : Proxy, IProxy
{
    public new const string NAME = "LoginProxy";

    public TempLogRegDataVO tempData;

    public LoginProxy() : base(NAME) {
    }

    public void SendRegister(TempLogRegDataVO data)
    {
        /**********
         * 需要改成Json与服务器通讯
         **********/
        
        DbAccess dbAccess = new DbAccess();
        string userName = data.UserName;//string.Format("'{0}'",data.UserName);
        string passWord = data.Password;//string.Format("'{0}'", data.Password);
        dbAccess.InsertIntoSpecific("info_users",
            new string[] { "type","playername", "passwd", "lv", "vip", "gold", "diamond", "exp" },
            new string[] { "1",userName, passWord, "0", "0", "100", "5","0" });
        //dbAccess.CloseSqlConnection();

        int id = -1;
        SqliteDataReader reader = dbAccess.SelectWhere("info_users",
                                                        new string[] { "id" },
                                                        new string[] { "playername", "passwd" },
                                                        new string[] { " = "," = "},
                                                        new string[] { userName, passWord });

        if (reader.Read())
        {
            id = Utils.GetInt(reader["id"]);
        }

        dbAccess.InsertIntoSpecific("info_spheres",
                                    new string[] { "userid", "customer_current" },
                                    new string[] { id.ToString(), "0" });
        dbAccess.CloseSqlConnection();


        tempData = new TempLogRegDataVO();
        tempData.status = ErrorCode.SUCCESS;

        SendNotification(LoginViewMediator.REGISTER_RESPONSE, tempData);
    }
        
    //请求登陆
    public void SendLogin(TempLogRegDataVO data)
    {
        //不论成功失败都会添加用户Id数据
        UserInfoProxy userInfoProxy = AppFacade.getInstance.RetrieveProxy(UserInfoProxy.NAME) as UserInfoProxy;

        DbAccess dbAccess = new DbAccess();
        string userName = data.UserName;
        string password = data.Password;
        string query = string.Format("SELECT id FROM info_users WHERE playername='{0}' AND passwd='{1}'", userName, password);
        SqliteDataReader reader = dbAccess.ExecuteQuery(query);
        if (reader.Read())
        {
            int id = Utils.GetInt(reader["id"]);
            userInfoProxy.UsertData.Id = id;
            tempData = new TempLogRegDataVO();
            tempData.status = ErrorCode.SUCCESS;

            LocalSaveData.LoginUserName = userName;  
            LocalSaveData.LoginPassword = password;  
        }
        else
        {
            tempData = new TempLogRegDataVO();
            userInfoProxy.UsertData.Id = -1;
            tempData.status = ErrorCode.INVALID_LOGIN_INFO;
        }
        dbAccess.CloseSqlConnection();

        SendNotification(LoginViewMediator.LOGIN_RESPONSE, tempData);
    }

}