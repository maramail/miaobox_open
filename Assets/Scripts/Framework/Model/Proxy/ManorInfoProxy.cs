 using UnityEngine;
using System.Collections.Generic;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using Mono.Data.Sqlite;

/// <summary>
/// 操作游戏相关数据
/// </summary>
public class ManorInfoProxy : Proxy, IProxy
{

    public new const string NAME = "ManorInfoProxy";

    public Dictionary<int, CatVO> catDic { get; set; }

    public ManorInfoProxy() : base(NAME)
    {
        Init();
    }

    private void Init()
    {
        InitcatDic();
    }

    /// <summary>
    /// 初始化猫id和猫各个属性的对应关系
    /// </summary>
    private void InitcatDic()
    {
        catDic = new Dictionary<int, CatVO>();
        foreach (stat_catRow catStat in stat_cat.GetInstance().rowList)
        {
            CatVO catVO = new CatVO();
            catVO.Id = catStat.id;
            catVO.Name = catStat.name;
            catVO.Level = catStat.lv;
            
            catVO.Evo = catStat.evo;
            
            catVO.Atk = catStat.atk;
         
           
            catVO.About = catStat.about;
            catVO.Money = catStat.paymoney;
            catVO.HirePrice = catStat.hireexp;
            catDic.Add(catStat.id, catVO);
        }
    }



}
