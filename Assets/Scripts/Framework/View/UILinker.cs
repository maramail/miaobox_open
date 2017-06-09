/********************************************************************
	created:	2016/08/18
	created:	18:8:2016   22:24
	filename: 	F:\Users\Administrator\Projects\MiaoBox\miaobox\MiaoBoxMVC\Assets\Scripts\Framework\View\UILinker.cs
	file path:	F:\Users\Administrator\Projects\MiaoBox\miaobox\MiaoBoxMVC\Assets\Scripts\Framework\View
	file base:	UILinker
	file ext:	cs
	author:		Zhou Jingren
	
	purpose:	ui的配置文件，包含文件位置等
*********************************************************************/
using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// ui的配置文件，包含文件位置等
/// </summary>
public class UILinker
{
    public static GameObject CreateUIObject(string behaviorType)
    {
        GameObject gameobject = null;
        switch (behaviorType)
        {
            case "LoginView":
                //prefabPath = "UI/LoginMenu/LoginMenu";
                gameobject = GameObject.FindGameObjectWithTag("LoginView");
                break;
            case "MainMenuUIView":
                //prefabPath = "UI/MainMenu/Prefab/MainMenuUI";
                gameobject = GameObject.FindGameObjectWithTag("MainMenuView");
                break;
            case "CatGroupMenuView":
                gameobject = GameObject.FindGameObjectWithTag("CatGroupMenuView");
                //prefabPath = "UI/CatGroupMenu/CatGroupMenu";
                break;
            case "MessageView":
                //prefabPath = "UI/MessageNotification/MessageNotification";
                break;
            case "BuildingBlueprintMenu":
                gameobject = GameObject.FindGameObjectWithTag("BuidlingBlueprint");
                break;
            case "BuildingChangeCtrl":
                gameobject = GameObject.FindGameObjectWithTag("BuildingChangeCtrl");
                Debug.Log("BuildingChangeCtrl!!~~~~~~~~~~~~~~~~");
                break;
            case "BattleView":
                gameobject = GameObject.FindGameObjectWithTag("BattleView");
                break;
            default:
                Debug.LogError("Undifined UI Type: " + behaviorType);
                return null;
        }
        //prefab = Resources.Load(prefabPath);
        //return GameObject.Instantiate(prefab) as GameObject;
        return gameobject;
    }
}
