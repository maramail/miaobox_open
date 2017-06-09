//using System;
using PureMVC.Patterns;
using PureMVC.Interfaces;


/// <summary>
/// 显示建筑蓝图菜单， 对建筑进行操作
/// 其实这只是对 BuildingBlueprintMenu 进行出初始化，并且 注册 BuildingBlueprintMediator
/// 在使用时候 就可以直接通过 mediator 操作 BuildingBlueprintMenu 进行显示隐藏等等
/// </summary>

public class InitBuildingBlueprintMenuCommand :SimpleCommand
{
   
    public override void Execute(INotification notification)
    {
        UIBaseBehaviour<BuildingBlueprintMediator>.CreateUI<BuildingBlueprintMenu>();
    }
}


