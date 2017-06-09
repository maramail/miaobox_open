using System;
using UnityEngine;



public class BuildingBlueprintInfo : MonoBehaviour
{
    public UILabel name;
    public UILabel Exp;
    public UILabel Diamond;
    public UIToggle toggle;
    public GameObject infoPanel;
    

    public BuildingBlueprintMenu.BuildingBlueprintType BuildingtType
    {
        get;
        set;
    }
    /// <summary>
    /// 建筑蓝图数据
    /// </summary>
    /// <value>The blueprint data.</value>
    public stat_blueprintRow blueprintData
    {
        get;
        set;
    }
    /// <summary>
    /// 建筑数据
    /// </summary>
    public  stat_buildingRow bulitData
    {
        get;
        set;
    }
    /// <summary>
    /// 存储建筑ID， 显示建筑的时候使用
    /// </summary>
    /// <value>The building I.</value>
    public int buildingID
    {
        get;
        set;
    }

    
    void Start()
    {
        UIEventListener.Get(gameObject).onClick += onClickItem;
    }

    void onDisable()
    {
        UIEventListener.Get(gameObject).onClick -= onClickItem;
    }

    public void showItem()
    {
        if (BuildingtType == BuildingBlueprintMenu.BuildingBlueprintType.Blueprint)
        {
            name.text = blueprintData.name;
            if (blueprintData.cost_exp > 0)
            {
                Exp.gameObject.SetActive(true);
                Exp.text = String.Format("{0}EXP", blueprintData.cost_exp);
            }
            else
            {
                Exp.gameObject.SetActive(false);
            }
            if (blueprintData.cost_diamond > 0)
            {
                Diamond.gameObject.SetActive(true);
                Diamond.text = String.Format("{0}钻石", blueprintData.cost_diamond);
            }
            else
            {
                Diamond.gameObject.SetActive(false);
            }
            toggle.group = 0; //设置其可以多选

        }
        else if(BuildingtType == BuildingBlueprintMenu.BuildingBlueprintType.Building)
        {
            name.text = bulitData.name;
            Exp.gameObject.SetActive(false);
            Diamond.gameObject.SetActive(false);
            toggle.value = false;
            toggle.group = 10;  //只能单选
        }
        toggle.value = false;
    }

    public void onClickItem(GameObject sender)
    {
        Debug.Log("onClickItem");
        if (sender == null)
        {
            for (int i = 0; i < infoPanel.transform.childCount; i++)
            {
                infoPanel.transform.GetChild(i).gameObject.SetActive(false);

            }
            infoPanel.transform.Find("CancelMoveButton").gameObject.SetActive(true);
            return;
        }
        else
        {

            for (int i = 0; i < infoPanel.transform.childCount; i++)
            {
                infoPanel.transform.GetChild(i).gameObject.SetActive(true);

            }

        }
        if (BuildingtType == BuildingBlueprintMenu.BuildingBlueprintType.Blueprint)
        {
            if (infoPanel)
            {
                infoPanel.transform.Find("CatName").GetComponent<UILabel>().text = blueprintData.name;
                infoPanel.transform.Find("CatInfo").GetComponent<UILabel>().text = blueprintData.description;
                //显示模型
            }
        }
        else if (BuildingtType == BuildingBlueprintMenu.BuildingBlueprintType.Building)
        {
            if (infoPanel)
            {
                infoPanel.transform.Find("CatName").GetComponent<UILabel>().text = bulitData.name;
                infoPanel.transform.Find("CatInfo").GetComponent<UILabel>().text = bulitData.description;
                //显示模型
            }

        }
        
    }
    /// <summary>
    /// 选择当前的item
    /// </summary>
    public void onSelectedItem()
    {
        if (BuildingtType == BuildingBlueprintMenu.BuildingBlueprintType.Blueprint)
        {
            if (UIToggle.current.value)
            {
                if (!BuildingBlueprintMenu.SelectedItem.Contains(blueprintData.id))
                {
                    BuildingBlueprintMenu.SelectedItem.Add(blueprintData.id);
                    onClickItem(null);
                    Debug.Log("++++增加ID ： " + blueprintData.id + " , 当前有 " + BuildingBlueprintMenu.SelectedItem.Count);
                }

            }
            else
            {
                if (BuildingBlueprintMenu.SelectedItem.Contains(blueprintData.id))
                {
                    BuildingBlueprintMenu.SelectedItem.Remove(blueprintData.id);
                    Debug.Log("----移除ID ： " + blueprintData.id + " , 当前有 " + BuildingBlueprintMenu.SelectedItem.Count);
                }
            }
        }
        else
        {
            if (UIToggle.current.value)
            {
                BuildingBlueprintMenu.buildingSelectItem = buildingID;
                onClickItem(null);
                Debug.Log(">>>>设置ID ： " + buildingID);
                //测试存储建筑与模型信息
               
            }
        }
    }
    /// <summary>
    /// 移除选择的item
    /// </summary>
    public void cancelSelectedItem()
    {
        toggle.value = false;

    }

}


