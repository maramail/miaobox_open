using UnityEngine;
using System.Collections;
using PureMVC.Patterns;
using PureMVC.Interfaces;

public class InitbuildingchangeViewCommand : SimpleCommand
{

    public override void Execute(INotification notification)
    {
        UIBaseBehaviour<BuildingModelMediator>.CreateUI<BuildingChangeCtrl>();
    }
}
