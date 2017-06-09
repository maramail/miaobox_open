/********************************************************************
	created:	2016/08/23
	created:	23:8:2016   22:05
	filename: 	F:\Users\Administrator\Projects\MiaoBox\MiaoBoxMVC\Assets\Scripts\Framework\View\CatGroupMenu\CatGroupMenuView.cs
	file path:	F:\Users\Administrator\Projects\MiaoBox\MiaoBoxMVC\Assets\Scripts\Framework\View\CatGroupMenu
	file base:	CatGroupMenuView
	file ext:	cs
	author:		Zhou Jingren
	
	purpose:	Cat分组菜单
*********************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using Mono.Data.Sqlite;

public class CatGroupMenuView : MonoBehaviour
{
    public GameObject catGroupMenuGameObject;

    public GameObject CancelBtn;
    public GameObject moveButton;
    public EnumGlobal.MenuType menutype;

    public GameObject infoPanel;

    public GameObject catGrid;
    public GameObject GroupGrid;

    public GameObject catItem;
    public GameObject GroupTitle;

    public GameObject ConfirmPanel;
    public GameObject DeleteCloseBtn;
    public GameObject DeleteOkBtn;

    //出售按钮，在猫分组界面不需要显示
    public GameObject SellBtn;
    //用于缓存标题，不用每次在读取
//    private List<system_catgroupRow> m_TitleList;   

    private static ArrayList selectedItems;
    
    

    void Start()
    {
        selectedItems = new ArrayList();
        UIEventListener.Get(moveButton).onClick += OnBtnClick;
        UIEventListener.Get(CancelBtn).onClick += OnBtnClick;
        UIEventListener.Get(DeleteCloseBtn).onClick += OnBtnClick;
        UIEventListener.Get(DeleteOkBtn).onClick += OnBtnClick;
         

        //        SendGroupTitleRequest();
    }

    

    void OnBtnClick(GameObject go)
    {
       
        if (go == CancelBtn &&menutype==EnumGlobal.MenuType.CatGroupMenu)
        {
            Debug.Log("+++++++++++++");
            AppFacade.GetInstance().SendNotification(NotiConst.CAT_GROUP_CLOSE);
            Debug.Log("-------------");
            catGroupMenuGameObject.SetActive(false);
            menutype = EnumGlobal.MenuType.Null;
        }
        else if (go == DeleteCloseBtn && menutype == EnumGlobal.MenuType.CatGroupMenu)
        {
           
            DeleteCancelButton();
            ConfirmPanel.SetActive(false);
        }
        else if (go == DeleteOkBtn && menutype == EnumGlobal.MenuType.CatGroupMenu)
        {
            DeleteOkButton();
            ConfirmPanel.SetActive(false);
        }
        else if (go == moveButton && menutype == EnumGlobal.MenuType.CatGroupMenu)
        {
            Debug.Log("测试1");
            OnMoveButtonChicked(go);
        }

    }
    /*
        猫分组是固定的，所以不需要请求数据，可以直接拿来用
    */
  /*  /// <summary>
    /// Sends the group title request.
    /// </summary>
    private void SendGroupTitleRequest()
    {
        // 请求猫分组标签
        JsonData data = new JsonData();
        data["type"] = CatGroupMenuMediator.CAT_GROUP_TITLE;
        AppFacade.GetInstance().SendNotification(NotiConst.GET_CAT_GROUP_TITLE, data);
    } 
    /// <summary>
    /// 接收猫分组标题的返回
    /// </summary>
    /// <param name="data">Data.</param>
    private void receiveGroupTitleResponse(object data)
    {
        Debug.Log("receiveGroupTitleResponse " + data.ToString());
        JsonData group = (JsonData)data;

        int count = (int)group["count"];
        for (int i = 1; i <= count; i++)
        {
            JsonData json = group[i];
            system_catgroupRow title = new system_catgroupRow(); 
            title.id = i;
            title.name = (string)json["name"];
            m_TitleList.Add(title);
        }
    }
    */

    /// <summary>
    ///  显示猫 标签
    /// </summary>
    public void ShowCatGroupTitle()
    {
        Debug.Log("ShowCatGroupTitle ");
        List<system_catgroupRow> titleList = system_catgroup.GetInstance().rowList;

        int childCount = GroupGrid.transform.childCount;
        // 先提前创建相同数目的title出然后在赋值
        if (childCount < titleList.Count)
        {
            for (int i = 0; i < titleList.Count - childCount; ++i)
            {
                NGUITools.AddChild(GroupGrid, GroupTitle);
            }
        }
        for(int i=0 ; i< titleList.Count ; ++i)
        {
            GameObject titleGo = GroupGrid.transform.GetChild(i).gameObject;
            titleGo.SetActive(true);
            titleGo.GetComponent<GroupInfo>().GroupId =titleList[i].id;
            titleGo.transform.Find("content").GetComponent<UILabel>().text =titleList[i].name;

            UIToggle toggle = titleGo.GetComponent<UIToggle>();

            EventDelegate ed = new EventDelegate(this, "ShowCatsItem");
            ed.parameters[0].obj = titleGo;
            toggle.onChange.Add(ed);
            if (i == 0 && toggle.value)
            {
                ShowCatsItem(titleGo);
            }
            else
            {
                toggle.value = i == 0;
            }
        }
        if (childCount > titleList.Count)
        {
            for (int i = childCount - titleList.Count-1; i < childCount; ++i)
            {
                GroupGrid.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        GroupGrid.GetComponent<UIGrid>().repositionNow = true;
    }

    /// <summary>
    /// 显示猫内容
    /// </summary>
    /// <param name="data"></param>
    public void ShowCatGroupInfo(object data)
    {
        JsonData cats = (JsonData)data;
        int count = (int)cats["count"];
        
        for (int i = 0; i < count; i++)
        {
            JsonData catInfo = cats[i];
            GameObject obj = NGUITools.AddChild(catGrid, catItem);
            CatInfo cat = obj.GetComponent<CatInfo>();
            if (cat == null)
            {
                cat = obj.AddComponent<CatInfo>();
            }
            cat.Id = (int)catInfo["id"];
            cat.CatTypeid = (int)catInfo["cattypeid"];
            cat.Name = (string)catInfo["catName"];
            cat.Level = (int)catInfo["lv"];
            cat.Atk = (int)catInfo["atk"];
            cat.GroupId = (int)catInfo["groupId"];

            UIEventListener.Get(obj).onClick = ShowCatInfo;
            UIToggle toggle = obj.transform.Find("SelectButton").GetComponent<UIToggle>();
            EventDelegate ed = new EventDelegate(this, "OnSelectClicked");
            ed.parameters[0].obj = obj;
            toggle.onChange.Add(ed);
            obj.transform.Find("NameLabel").GetComponent<UILabel>().text = obj.GetComponent<CatInfo>().Name;
        }
        catGrid.GetComponent<UIGrid>().repositionNow = true;
    }
    /// <summary>
    /// 根据数据库获取的数据,实例化标签列表
    /// </summary>
    public void ShowCatsItem(GameObject sender)
    {

        if (!moveButton.activeInHierarchy)
        {
            moveButton.SetActive(true);
        }

        if (sender.GetComponent<UIToggle>().value)
        {
            //对比一下当前列表的第一个子对象,如果groupId等于sender的groupId,
            //如果不同就全清除掉,并且清除掉已经选择的数据

            if (catGrid.transform.childCount != 0 && catGrid.transform.GetChild(0).GetComponent<CatInfo>().GroupId != sender.GetComponent<GroupInfo>().GroupId)
            {
                for (int i = 0; i < catGrid.transform.childCount; i++)
                {
                    Destroy(catGrid.transform.GetChild(i).gameObject);
                }
            }
            int id = sender.GetComponent<GroupInfo>().GroupId;

            // 请求分组数据
            JsonData data = new JsonData();
            data["id"] = id;
            AppFacade.GetInstance().SendNotification(NotiConst.GET_CAT_GROUP_INFO, data);
        }
        ShowCatInfo(null);
        selectedItems.Clear();                        //避免存放选择失效
    }

    private void ShowCatInfo(GameObject sender)
    {
        if (sender == null)
        {
            for (int i = 0; i < infoPanel.transform.childCount; i++)
            {
                infoPanel.transform.GetChild(i).gameObject.SetActive(false);

            }
            CancelBtn.SetActive(true);
            return;
        }
        else
        {

            for (int i = 0; i < infoPanel.transform.childCount; i++)
            {
                infoPanel.transform.GetChild(i).gameObject.SetActive(true );

            }

        }
        CatInfo cat = sender.GetComponent<CatInfo>();
        Transform modelpos = infoPanel.transform.Find("ModelPos").GetComponent<Transform>();
        if (modelpos.childCount > 0)
        {
            GameObject catmodelbefore = modelpos.GetChild(0).gameObject;
            catmodelbefore.layer = 8;
            catmodelbefore.transform.SetChildLayer(8);
            Destroy(catmodelbefore.GetComponent<EmployeeRandomAnimation>());
            catmodelbefore.SetActive(false);


        }
        GameObject catmodel = CatPool.GetInstance().GetCatPool(cat.CatTypeid).CreateObject(modelpos.position);
        catmodel.layer = 9;
        catmodel.transform.SetChildLayer(9);
        catmodel.AddComponent<EmployeeRandomAnimation>();
        catmodel.transform.rotation = modelpos.rotation;
        catmodel.transform.parent = modelpos;
        infoPanel.transform.Find("CatName").GetComponent<UILabel>().text = cat.Name;
        infoPanel.transform.Find("CatInfo").GetComponent<UILabel>().text = 
            (
            "lv:" + cat.Level+ "\n"
            + "atk:" + cat.Atk + "\n"
            + "groupId:" + cat.GroupId
            )
            ;

    }

    public void OnSelectClicked(GameObject sender)
    {
        if (sender.transform.Find("SelectButton").GetComponent<UIToggle>().value)
        {
            selectedItems.Add(sender);
        }
        else
        {
            if (selectedItems.Contains(sender))
            {
                selectedItems.Remove(sender);
            }
        }
    }

    /// <summary>
    /// 移动按钮 数据响应
    /// </summary>
    /// <param name="data"></param>
    public void CatMoveTitle(object data)
    {
        JsonData group = (JsonData)data;
        int count = (int)group["count"];
        for (int i = 0; i < count; ++i)
        {
            JsonData json = group[i.ToString()];
            int id = (int)json["id"];
            string name = (string)json["name"];

            GameObject obj = NGUITools.AddChild(catGrid, catItem);
            obj.AddComponent<GroupInfo>().GroupId = id;
            obj.transform.Find("NameLabel").GetComponent<UILabel>().text = name;
            obj.transform.FindChild("SelectButton").gameObject.SetActive(false);
            UIEventListener.Get(obj).onClick = OnGroupItemButton;
        }
        GameObject dismissal = NGUITools.AddChild(catGrid, catItem);
        dismissal.transform.Find("NameLabel").GetComponent<UILabel>().text = "解雇";
        dismissal.transform.FindChild("SelectButton").gameObject.SetActive(false);
        UIEventListener.Get(dismissal).onClick = OnDismissalButton;

        catGrid.GetComponent<UIGrid>().repositionNow = true;
    }
    /// <summary>
    /// 移动按钮响应
    /// </summary>
    /// <param name="sender"></param>
    public void OnMoveButtonChicked(GameObject sender)
    {
        if (selectedItems.Count != 0)
        {
            sender.SetActive(false);

            for (int i = 0; i < catGrid.transform.childCount; i++)
            {
                catGrid.transform.GetChild(i).gameObject.SetActive(false);
            }
            // 服务器请求 猫分组列表
            JsonData data = new JsonData();
            data["type"] = CatGroupMenuMediator.CAT_MOVE_GROUP_TITLE;
            AppFacade.GetInstance().SendNotification(NotiConst.GET_CAT_GROUP_TITLE, data);
        }
    }

    #region 解雇猫
    /// <summary>
    /// 解雇
    /// </summary>
    /// <param name="go"></param>
    private void OnDismissalButton(GameObject go)
    {
        ConfirmPanel.SetActive(true);
    }

    /// <summary>
    /// 确定 解雇猫
    /// </summary>
    private void DeleteOkButton()
    {
        for (int i = 0; i < catGrid.transform.childCount; i++)
        {
            Destroy(catGrid.transform.GetChild(i).gameObject);
        }
        JsonData deleteCat = new JsonData();
        deleteCat["count"] = selectedItems.Count;

        for (int i = 0; i < selectedItems.Count; i++)
        {
            var obj = selectedItems[i] as GameObject;
            if (obj != null)
            {
                var cat = obj.GetComponent<CatInfo>();
                if (cat == null)
                {
                    Debug.LogError("错误 组件错误");
                    return;
                }
                if (i == 0)
                {
                    deleteCat["group"] = cat.GroupId;
                }

                deleteCat[i.ToString()] = cat.Id;
            }
        }
        selectedItems.Clear();
        AppFacade.GetInstance().SendNotification(NotiConst.CAT_DELETE, deleteCat);
        moveButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// 取消删除猫 重新请求数据列表
    /// </summary>
    private void DeleteCancelButton()
    {
        for (int i = 0; i < catGrid.transform.childCount; i++)
        {
            Destroy(catGrid.transform.GetChild(i).gameObject);
        }

        JsonData data = new JsonData();
        data["id"] = (selectedItems[0] as GameObject).GetComponent<CatInfo>().GroupId;

        selectedItems.Clear();
        AppFacade.GetInstance().SendNotification(NotiConst.GET_CAT_GROUP_INFO, data);
        moveButton.gameObject.SetActive(true);
    }

    /// <summary>
    /// 猫删除完成 重新请求 猫分组数据
    /// </summary>
    /// <param name="data"></param>
    public void CatDeleteFinsh(object data)
    {
        JsonData group = (JsonData)data;
        JsonData groupData = new JsonData();
        groupData["id"] = group["group"];
        AppFacade.GetInstance().SendNotification(NotiConst.GET_CAT_GROUP_INFO, groupData);
    }
    #endregion

    #region 切换分组
    /// <summary>
    /// 切换分组
    /// </summary>
    /// <param name="data"></param>
    public void SwitchCatGroup(object data)
    {
        JsonData group = (JsonData)data;
        int groupId = (int)group["id"];
        for (int i = 0; i < GroupGrid.transform.childCount; i++)
        {
            if (GroupGrid.transform.GetChild(i).GetComponent<GroupInfo>().GroupId == groupId)
            {
                if (GroupGrid.transform.GetChild(i).GetComponent<UIToggle>().value == true)
                {
                    ShowCatsItem(GroupGrid.transform.GetChild(i).gameObject);
                }
                else
                {
                    GroupGrid.transform.GetChild(i).GetComponent<UIToggle>().value = true;
                }
            }
        }
        //重置一些数据
        moveButton.SetActive(true);
    }
    /// <summary>
    /// 给猫换组的点击响应,
    /// </summary>
    public void OnGroupItemButton(GameObject sender)
    {
        Debug.Log("&&&&&&");
        int groupId = sender.GetComponent<GroupInfo>().GroupId;

        JsonData catMoveData = new JsonData();
        catMoveData["newId"] = groupId;
        catMoveData["count"] = selectedItems.Count;

        for (int i = 0; i < selectedItems.Count; i++)
        {
            var obj = selectedItems[i] as GameObject;
            if (obj != null)
            {
                var cat = obj.GetComponent<CatInfo>();
                if (cat == null)
                {
                    Debug.LogError("错误 组件错误");
                    return;
                }

                if (groupId == cat.GroupId)
                {
                    return;
                }
                catMoveData["oldId"] = cat.GroupId;


                catMoveData[i.ToString()] = cat.Id;
            }

        }

        selectedItems.Clear();
        AppFacade.GetInstance().SendNotification(NotiConst.CAT_SWITCH_GROUP, catMoveData);
    }
    #endregion

    #region 显示猫分组之前的准备

    /// <summary>
    /// Removes all grid children.
    /// </summary>
    private void  removeAllGridChildren()
    {
        if (catGrid != null)
        {
            catGrid.transform.DestroyChildren();
        }
    }
    /// <summary>
    /// Resets the toggle.
    /// </summary>
    private void resetToggle()
    {
        int count = GroupGrid.transform.childCount;
        if (count > 0)
        {
            for (int i = 0; i < count; ++i)
            {
                UIToggle toggle = GroupGrid.transform.GetChild(i).GetComponent<UIToggle>();
                if (toggle != null)
                {
                   
                    toggle.onChange.Clear();
                    toggle.value = false;
                }
            }
        }
    }

   /// <summary>
   /// 显示猫分组，需要请求猫标题，并做准备处理
   /// </summary>
    public void showCatGroup()
    {
        if (!catGroupMenuGameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
            catGroupMenuGameObject.SetActive(true);
            removeAllGridChildren();
            resetToggle();
            moveButton.GetComponentInChildren<UILabel>().text = "移动";
            ShowCatGroupTitle();
        }
        if (SellBtn != null)
            SellBtn.SetActive(false);
    }


    public void limitInfo()
    {
        Debug.Log("移动将达到组员上限，移动失败");
        string s = "移动将达到组员上限，移动失败";
        NotifactionFunc._instance.showNotifaction(s);

    }
   
    #endregion









}