/********************************************************************
	created:	2016/08/18
	created:	18:8:2016   22:22
	filename: 	f:\users\administrator\projects\miaobox\miaobox\miaoboxmvc\assets\scripts\framework\view\uibasebehavior.cs
	file path:	f:\users\administrator\projects\miaobox\miaobox\miaoboxmvc\assets\scripts\framework\view
	file base:	uibasebehavior
	file ext:	cs
	author:		Zhou Jingren
	
	purpose:    所有ui的基类 包含生成，注册mediator，销毁等功能
*********************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using PureMVC.Interfaces;
/// <summary>
/// 所有ui的基类 包含生成，注册mediator，销毁等功能
/// To Do：UI的资源回收，层级管理
/// (作用：节约了Command代码量，将其中的通用部分取出，比较精髓的一块代码)
/// </summary>
public class UIBaseBehaviour<T> where T : Mediator, IMediator, new()
{
    
    private static Dictionary<Type, MonoBehaviour> UICacheDic = new Dictionary<Type, MonoBehaviour>();


    //通过 tag 读取 gameobject 并且注册相应的mediator 
    public static void CreateUI<U>() where U : MonoBehaviour
    {
        T mMediator;
        foreach (Type t in UICacheDic.Keys)
        {
            Debug.Log(t);
        } 
        if (!UICacheDic.ContainsKey(typeof(U)))
        {
            GameObject UIObject = UILinker.CreateUIObject(typeof(U).ToString());
            U uiBehavior = UIObject.GetComponent<U>();
            mMediator = new T();
            mMediator.ViewComponent = uiBehavior;
            AppFacade.getInstance.RegisterMediator(mMediator);
            UICacheDic.Add(typeof(U), uiBehavior);
        }
        UICacheDic[typeof(U)].enabled = true;
    }

}


