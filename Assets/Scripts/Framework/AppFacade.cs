using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using PureMVC.Interfaces;
public class AppFacade : Facade, IFacade
{
    private static AppFacade _instance;
    public static AppFacade getInstance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new AppFacade();
            }
            return _instance;
        }
    }
    protected override void InitializeController()
    {
        base.InitializeController();

        //Init MVC 包括数据读取命令、注册Proxy(搭建Model)命令、注册Mediator(SceneMediator)、显示登录UI界面命令
        RegisterCommand(NotiConst.STARTUP, typeof(StartupCommand));

        //init && show View
        RegisterCommand(NotiConst.INIT_MAIN_MENU_VIEW, typeof(InitMainMenuUICommand));
        RegisterCommand(NotiConst.INIT_CAT_GROUP_MENU_VIEW, typeof(InitCatGroupMenuCommand));
        RegisterCommand(NotiConst.INIT_CUSTOMER＿VIEW, typeof(InitCustomerCommand));
        RegisterCommand(NotiConst.INIT_EMPLOYEE_VIEW,typeof(InitEmployeeCommand));
        RegisterCommand(NotiConst.INIT_BUILDING_BLUEPRINT, typeof(InitBuildingBlueprintMenuCommand));
        RegisterCommand(NotiConst.INITBUILDINGCHANGE, typeof(InitbuildingchangeViewCommand));// 注册模型改变命令
        RegisterCommand(NotiConst.INITBATTLEVIEW, typeof(InitBattleCommand));//注册战斗
        //Login & Register
        RegisterCommand(NotiConst.LOGIN_REQUEST, typeof(LoginCommand));
        RegisterCommand(NotiConst.REGISTER_REQUEST, typeof(LoginCommand));

        //UserInfo
        RegisterCommand(NotiConst.GET_USER_INFO_VALUE, typeof(RefreshUserInfoValueCommand));
        RegisterCommand(NotiConst.SET_GOLD, typeof(RefreshUserInfoValueCommand));
        RegisterCommand(NotiConst.SET_EXP, typeof(RefreshUserInfoValueCommand));

        //Cat Group
        RegisterCommand(NotiConst.GET_CAT_GROUP_DATA, typeof(ShowCatGroupCommand));
        RegisterCommand(NotiConst.GET_CAT_GROUP_TITLE, typeof(ShowCatGroupCommand));
        RegisterCommand(NotiConst.GET_CAT_GROUP_INFO, typeof(ShowCatGroupCommand));
        RegisterCommand(NotiConst.CAT_SWITCH_GROUP, typeof(ShowCatGroupCommand));
        RegisterCommand(NotiConst.CAT_DELETE, typeof(ShowCatGroupCommand));
        RegisterCommand(NotiConst.CAT_GROUP_CLOSE, typeof(ShowCatGroupCommand));

        //Customer
        RegisterCommand(NotiConst.GET_CUSTOMER_SPAWN_VALUE,typeof(RefreshManorValueCommand));
        RegisterCommand(NotiConst.SET_CURRENT_CUSTOMER, typeof(RefreshManorValueCommand));
        RegisterCommand(NotiConst.ADD_CUSTOMER_MODEL,typeof(AddCustomerModelCommand));

        //Employee
        RegisterCommand(NotiConst.ADD_EMPLOYEE_MODEL,typeof(AddEmployeeModelCommand));
        RegisterCommand(NotiConst.EMPLOY_EMPLOYEE,typeof(EmployEmployeeCommand));

        //Buildblueprint
        RegisterCommand(NotiConst.GET_BUILDINGBLUEPRINT_DATA , typeof(ShowBuildingBlueprintCommand));
        RegisterCommand(NotiConst.GET_STATBUILD_DATA, typeof(ShowBuildingBlueprintCommand));
        RegisterCommand(NotiConst.GET_BUILDING_DATA, typeof(ShowBuildingBlueprintCommand));
        RegisterCommand(NotiConst.GET_CHNAGE_MODEL_DATA, typeof(ShowBuildingBlueprintCommand));//获取模型改变模型
        RegisterCommand(NotiConst.GET_BUILDING_MODEL_DATA, typeof(ShowBuildingBlueprintCommand)); //获取模型初始值数据
        RegisterCommand(NotiConst.SET_BUILDING_MODEL_DATA, typeof(AddChangeModelCommand)); //设置模型与位置信息
        RegisterCommand(NotiConst.PURCHASE_BLUEPRINT, typeof(ShowBuildingBlueprintCommand));

        //Battle 
        RegisterCommand(NotiConst.GET_BATTLE_CAT_GROUP_INFO,typeof(ShowBattleCatGropCommand));

    }
    public void StartUp()
    {
        SendNotification(NotiConst.STARTUP);
    }
    public void QuitGameAndSave()
    {
        (RetrieveProxy(UserInfoProxy.NAME) as UserInfoProxy).SaveToDb();
        (RetrieveProxy(CustomerProxy.NAME) as CustomerProxy).SaveToDb();
    }
}