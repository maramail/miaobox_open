/********************************************************************
	created:	2016/08/18
	created:	18:8:2016   22:21
	filename: 	f:\users\administrator\projects\miaobox\miaobox\miaoboxmvc\assets\scripts\scene\myscenemanager.cs
	file path:	f:\users\administrator\projects\miaobox\miaobox\miaoboxmvc\assets\scripts\scene
	file base:	myscenemanager
	file ext:	cs
	author:		Zhou Jingren
	
	purpose:	场景管理Manager
*********************************************************************/
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using PureMVC.Patterns;
using PureMVC.Interfaces;

/// <summary>
/// 场景加载类  To Do：异步加载 卸载资源等
/// </summary>
public class SceneMediator : Mediator
{
    public new const string NAME = "SceneMediator";

    //public const string LOAD_SCENE = "LOAD_SCENE";

    public SceneMediator():base(NAME)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public override IEnumerable<string> ListNotificationInterests
    {
        get
        {
            IList<string> list = new List<string>();
        //    list.Add(SceneMediator.LOAD_SCENE);;
            return list;
        }
    }

    public override void HandleNotification(INotification notification)
    {
        //switch ((string)notification.Body)
        //{
        //    case SceneConst.LoginScene:
        //        SceneManager.LoadScene(SceneConst.LoginScene);
        //        break;
        //    case SceneConst.GameScene:
        //        SceneManager.LoadScene(SceneConst.GameScene);
        //        break;
        //    default : break;
        //} 
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
        if (scene.name == SceneConst.GameScene)
        {
            //打开游戏界面的时候，需要打开的界面需要注册
            AppFacade.getInstance.SendNotification(NotiConst.INIT_MAIN_MENU_VIEW);
            AppFacade.getInstance.SendNotification(NotiConst.INIT_CUSTOMER＿VIEW);
            AppFacade.getInstance.SendNotification(NotiConst.INIT_EMPLOYEE_VIEW);
            AppFacade.getInstance.SendNotification(NotiConst.INIT_CAT_GROUP_MENU_VIEW);
            AppFacade.getInstance.SendNotification(NotiConst.INIT_BUILDING_BLUEPRINT);
            AppFacade.getInstance.SendNotification(NotiConst.INITBUILDINGCHANGE);//注册模型改变
          
        }
        if (scene.name==SceneConst.BattleScene)
        {

            AppFacade.getInstance.SendNotification(NotiConst.INITBATTLEVIEW);   //注册战斗场景
            BulitPool.GetInstance().DestructAll();
            CatPool.GetInstance().DestructAll();

        }
    }
}
