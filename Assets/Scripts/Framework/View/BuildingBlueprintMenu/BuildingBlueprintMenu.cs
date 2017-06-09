using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using System;



public class BuildingBlueprintMenu : MonoBehaviour {
    public enum BuildingBlueprintType
    {
        status= 0,
        Building =1,
        Blueprint =2,
    }
   

    public GameObject BuildingBlueprintGameObject;

    public GameObject CancelBtn;

    public GameObject MoveBtn;

    public GameObject InfoPanel;

    public GameObject BuildingGrid;

    public GameObject BuildingTitleGrid;

    public GameObject BuildingItem;

    public GameObject BuildingTitleItem;

    public GameObject ConfirmPanel;
        
    public GameObject DeleteCloseBtn;

    public GameObject DeleteOkBtn;

    public GameObject SellBtn;
    /// <summary>
    /// 猫粮的ID 用于区分展示建筑的信息
    /// </summary>
    public int foodBowID;

    public int modeltrsid { get; set; }

    public EnumGlobal.MenuType menutype;

    /// <summary>
    /// 建筑蓝图静态数据 用Tag值进行分开
    /// </summary>
    private Dictionary <int , List<stat_blueprintRow> > buildingBluepritData = new Dictionary<int, List<stat_blueprintRow>> ();
    /// <summary>
    /// 所有的建筑蓝图数据，用ID存储。方便查找
    /// </summary>
    private Dictionary<int ,stat_blueprintRow > allBuildingBluepritData = null;
    private Dictionary<int, stat_buildingRow> allBulidData = new Dictionary<int, stat_buildingRow>();
    /// <summary>
    /// 建筑是否已经建立
    /// </summary>
    public bool isBulid
    {
        get;
        set;
    }
    /// <summary>
    /// 当前选择的Item
    /// </summary>
    public static  List<int> SelectedItem = new List<int> ();
    /// <summary>
    /// 建筑信息是单选，只要存储一个就行
    /// </summary>
    public static int buildingSelectItem;
    /// <summary>
    /// 标题
    /// </summary>
    private List<string> BuildingBlueprintTitle  = new List<string>();
    /// <summary>
    /// 当前点击的类型
    /// </summary>
    private BuildingBlueprintType buildingType;

     

    private string[] buildingData;

	// Use this for initialization
	void Start () {
        //绑定事件
        UIEventListener.Get(CancelBtn).onClick += onButtonClick;
        UIEventListener.Get(MoveBtn).onClick += onButtonClick;
        UIEventListener.Get(DeleteCloseBtn).onClick += onButtonClick;
        UIEventListener.Get(DeleteOkBtn).onClick += onButtonClick;
        UIEventListener.Get(SellBtn).onClick += onButtonClick;
        
        //发送命令 显示第一页

        BuildingBlueprintTitle.Add("现状");
        BuildingBlueprintTitle.Add("修建");
        BuildingBlueprintTitle.Add("蓝图");
   	}


    public void onButtonClick(GameObject go)
    {
        
        if (go == CancelBtn&&menutype==EnumGlobal.MenuType.BuildingBlueprintMenu )
        {
            BuildingBlueprintGameObject.SetActive(false);
            menutype = EnumGlobal.MenuType.Null;
        }
        else if (go == MoveBtn && menutype == EnumGlobal.MenuType.BuildingBlueprintMenu)
        {
            if (buildingType == BuildingBlueprintType.Blueprint)
            {
                Debug.Log("购买");
                onClickPurchase();
            }
            else if (buildingType == BuildingBlueprintType.Building)
            {
                BuildModelVo vo = new BuildModelVo();
                vo.Modelid = buildingSelectItem;
                vo.foodbowid = foodBowID;
                vo.ModeltrsId = modeltrsid;
                AppFacade.getInstance.SendNotification(NotiConst.SET_BUILDING_MODEL_DATA, vo);
                AppFacade.getInstance.SendNotification(NotiConst.GET_CHNAGE_MODEL_DATA, vo);
                BuildingBlueprintGameObject.SetActive(false);
                menutype = EnumGlobal.MenuType.Null;
            }
            else if (buildingType == BuildingBlueprintType.status)
            {
                DeleteOkBtn.GetComponentInChildren<UILabel>().text = "是";
                DeleteCloseBtn.GetComponentInChildren<UILabel>().text = "否";
                ConfirmPanel.transform.GetChild(0).GetComponentInChildren<UILabel>().text = "是否拆除该设施?";
                ConfirmPanel.SetActive(true);
            }
        }
        else if (go == SellBtn && menutype == EnumGlobal.MenuType.BuildingBlueprintMenu)
        {
            DeleteOkBtn.GetComponentInChildren<UILabel>().text = "是";
            DeleteCloseBtn.GetComponentInChildren<UILabel>().text = "否";
            ConfirmPanel.transform.GetChild(0).GetComponentInChildren<UILabel>().text = "是否出售该蓝图?";
            ConfirmPanel.SetActive(true);


        }
        else if (go == DeleteOkBtn && menutype == EnumGlobal.MenuType.BuildingBlueprintMenu)
        {

        }
        else if (go == DeleteCloseBtn && menutype == EnumGlobal.MenuType.BuildingBlueprintMenu)
        {
            ConfirmPanel.SetActive(false);
        }

    
    }
    /// <summary>
    /// 获取建筑的信息，在获取信息之后展示数据
    /// </summary>
    public void getBuildingData()
    {
        AppFacade.GetInstance().SendNotification(NotiConst.GET_BUILDINGBLUEPRINT_DATA);
        AppFacade.GetInstance().SendNotification(NotiConst.GET_STATBUILD_DATA);
    }
    /// <summary>
    /// 显示建筑菜单中的标签
    /// </summary>
    /// <param name="data">Data.</param>
    public void showBulidingTitle()
    {
        
        if (buildingBluepritData == null || buildingBluepritData.Count <=0 ||allBulidData==null)
        {
            //请求蓝图的信息
            //  AppFacade.GetInstance().SendNotification(NotiConst.GET_BUILDINGBLUEPRINT_DATA);
            getBuildingData();
        }
        int count = BuildingTitleGrid.transform.childCount;
        if (isBulid)
        {

            if (count <= 2)  //如果标题的Item 不足 2个先创建，然后在使用
            {
                for (int i = 0; i <=2 - count; i++)
                {
                    NGUITools.AddChild(BuildingTitleGrid, BuildingTitleItem);
                }
            }
            var j = 0;
            for (; j < BuildingBlueprintTitle.Count; ++j)
            {
                setTitleItemById(ref j, ref j, j == 0);
            }
            for (; j < count; ++j )
            {
                GameObject title = BuildingTitleGrid.transform.GetChild(j).gameObject;
                title.SetActive(false);
            }
        }
        else//否则只显示 2， 1 
        {
            if (count <= 2)  //如果标题的Item 不足 2个先创建，然后在使用
            {
                for (int i = 0; i < 2 - count; i++)
                {
                    GameObject title = NGUITools.AddChild(BuildingTitleGrid, BuildingTitleItem);
                }
            }
            int curIndex = 0;
            for(int i = BuildingBlueprintTitle.Count - 1 ; i > 0 ; --i  )
            {
                setTitleItemById(ref i, ref curIndex, i == BuildingBlueprintTitle.Count - 1);
                ++curIndex;
            }
            for (; curIndex < count; ++curIndex)
            {
                GameObject title = BuildingTitleGrid.transform.GetChild(curIndex  ).gameObject;
                title.SetActive(false);
            }
        }
        //最后刷新一下 Grid
        BuildingTitleGrid.GetComponent<UIGrid>().repositionNow = true;

    }
    /// <summary>
    /// Sets the title item by identifier.
    /// </summary>
    /// <param name="TitleId">Title ide.</param>
    /// <param name="gameObjectId">Game object id.</param>
    /// <param name="isSelected">If set to <c>true</c> is selected.</param>
    private  void  setTitleItemById( ref int TitleId  , ref int  gameObjectId, bool isSelected)
    {
        GameObject title = BuildingTitleGrid.transform.GetChild( gameObjectId).gameObject;
        title.GetComponent<GroupInfo>().GroupId = TitleId;    //保存这个组的id ，即Title的ID
        title.SetActive(true);

        UILabel label = title.transform.Find("content").GetComponent<UILabel>();
        label.text = BuildingBlueprintTitle[TitleId];

        UIToggle toggle = title.GetComponent<UIToggle>();

        EventDelegate ed = new EventDelegate(this, "showItem");
        ed.parameters[0].obj = title;
        toggle.onChange.Add(ed);
        //如果是复用之前的 TitleItem 的话，因为其状态在之前无法修改成功，所以只能手动调用一下
        if (isSelected && toggle.value == true)
        {
            showItem(title);
        }
        else
        {
            toggle.value = isSelected;
        }
    }


    /// <summary>
    /// 点击标题的回调函数，显示喵分组
    /// </summary>
    /// <param name="sender">Sender.</param>
    private void showItem(GameObject sender)
    {
        // 如果不显示按钮，则显示按钮
        if (!MoveBtn.activeInHierarchy)
        {
            MoveBtn.SetActive(true);
        }
        //当点击toggle的时候，无论是选中还是取消选中都会调用这里，所以需要判断
        if (sender.GetComponent<UIToggle>().value)
        {
            buildingType = (BuildingBlueprintType)sender.GetComponent<GroupInfo>().GroupId;
            if (buildingType == BuildingBlueprintType.Blueprint)
            {
                SellBtn.SetActive(false);
                MoveBtn.GetComponentInChildren<UILabel>().text = "购入";
                SelectedItem.Clear();
                showBuildingBlueprint();
            }
            else if (buildingType == BuildingBlueprintType.Building)
            {
                MoveBtn.GetComponentInChildren<UILabel>().text = "建造";
                SellBtn.SetActive(true);
               
                SellBtn.GetComponentInChildren<UILabel>().text = "出售";
                AppFacade.GetInstance().SendNotification(NotiConst.GET_BUILDING_DATA);
            }
            else if (buildingType == BuildingBlueprintType.status)
            {
                SellBtn.SetActive(false);
                MoveBtn.GetComponentInChildren<UILabel>().text = "拆除";
                removeAllGridChildren();
                 
            
            }
        }
    }
    /// <summary>
    /// 显示蓝图信息
    /// </summary>
    private void showBuildingBlueprint()
    {
        int count = BuildingGrid.transform.childCount;
        List<stat_blueprintRow> tmplist = buildingBluepritData[foodBowID];
        for(int i =0 , j =0 ; i<count || j<buildingBluepritData[foodBowID].Count ; ++i , ++j)
        {
            if (i < count && j < buildingBluepritData[foodBowID].Count)
            {
                GameObject item = BuildingGrid.transform.GetChild(i).gameObject;
                item.SetActive(true);
                //设置数据
                item.GetComponent<BuildingBlueprintInfo>().BuildingtType = buildingType;
                item.GetComponent<BuildingBlueprintInfo>().blueprintData = buildingBluepritData[foodBowID][j];
                item.GetComponent<BuildingBlueprintInfo>().infoPanel = InfoPanel;
               
                item.GetComponent<BuildingBlueprintInfo>().showItem();
                //设置第一个是选中的按钮，显示其信息
                if (j == 0)
                    item.GetComponent<BuildingBlueprintInfo>().onClickItem(null);

            }
            else if (i >= count && j < buildingBluepritData[foodBowID].Count)
            {
                GameObject item = NGUITools.AddChild(BuildingGrid, BuildingItem);
                item.GetComponent<BuildingBlueprintInfo>().BuildingtType = buildingType;
                item.GetComponent<BuildingBlueprintInfo>().blueprintData = buildingBluepritData[foodBowID][j];
                item.GetComponent<BuildingBlueprintInfo>().infoPanel = InfoPanel;
                item.GetComponent<BuildingBlueprintInfo>().showItem();
                //设置第一个是选中的按钮，显示其信息
                if (j == 0)
                    item.GetComponent<BuildingBlueprintInfo>().onClickItem(null);
            }
            //否则有多余的 Item, 隐藏
            else if (i < count && j >= buildingBluepritData[foodBowID].Count)
            {
                BuildingGrid.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        // 重新刷新位置
        BuildingGrid.GetComponent<UIGrid>().repositionNow = true;

    }
    /// <summary>
    /// 显示所有的建筑信息
    /// </summary>
    private void  showBuildingDataItem()
    {
        int curItemCount = BuildingGrid.transform.childCount;
        int buildId = 0;
        BuildingBlueprintInfo info = null;
        for(int i =0 , j =0 ; i<curItemCount || j< buildingData.Length ; ++i , ++j)
        {
            if (i < curItemCount && j < buildingData.Length)
            {
                buildId = Int32.Parse(buildingData[j]);
                GameObject item = BuildingGrid.transform.GetChild(i).gameObject;
                item.SetActive(true);
                info = item.GetComponent<BuildingBlueprintInfo>();
                //设置数据
                info.BuildingtType = buildingType;
                info.buildingID = buildId;
                info.bulitData = allBulidData[buildId];
                info.infoPanel = InfoPanel;
                info.showItem();
                //              info.cancelSelectedItem();
                //设置第一个是选中的按钮，显示其信息
                if (j == 0)
                {
                    info.onClickItem(null);
                    info.toggle.value = true;
                    //设置一下，因为上一句无法保证会调用其onChange 函数，所以只能手动调用，保证正确
                    buildingSelectItem = buildId;
                }
                


                 
            }
            else if (i >= curItemCount && j < buildingData.Length)
            {
                buildId = Int32.Parse(buildingData[j]);
                GameObject item = NGUITools.AddChild(BuildingGrid, BuildingItem);
                info = item.GetComponent<BuildingBlueprintInfo>();
                info.BuildingtType = buildingType;
                info.buildingID = buildId;
                info.bulitData = allBulidData[buildId];
                info.infoPanel = InfoPanel;
                info.showItem();
                //设置第一个是选中的按钮，显示其信息
                if (j == 0)
                {
                    item.GetComponent<BuildingBlueprintInfo>().onClickItem(null);
                    info.toggle.value = true;
                    //设置一下，因为上一句无法保证会调用其onChange 函数，所以只能手动调用，保证正确
                    buildingSelectItem = buildId;
                }
                

            }
            //否则有多余的 Item, 隐藏
            else if (i < curItemCount && j >= buildingData.Length)
            {
                BuildingGrid.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        // 重新刷新位置
        BuildingGrid.GetComponent<UIGrid>().repositionNow = true;
    }






 


    void OnEnable ()
    {
        Debug.Log("BuildingBlueprintMenu OnEnable ");
    }
        
    void OnDisable()
    {
        Debug.Log("BuildingBlueprintMenu OnDisable");
    }
    /// <summary>
    /// 设置蓝图的静态数据，在启动的时候读取
    /// </summary>
    /// <param name="data">Data.</param>
    public void setBuildingBlueprintData(object data)
    {
        allBuildingBluepritData = data as Dictionary<int , stat_blueprintRow>;
        foreach (stat_blueprintRow  blueprint in allBuildingBluepritData.Values)
        {
            if (buildingBluepritData.ContainsKey(blueprint.type))
            {
                buildingBluepritData[blueprint.type].Add(blueprint);
            }
            else
            {
                List<stat_blueprintRow> list = new List<stat_blueprintRow>();
                list.Add(blueprint);
                buildingBluepritData[blueprint.type] = list;
            }
        }
    }
    /// <summary>
    /// 设置建筑静态数据
    /// </summary>
    /// <param name="data"></param>
    public void setBuildStatData(object data)
    {
        Dictionary<int, stat_buildingRow> Data = data as Dictionary<int, stat_buildingRow>;
        foreach (stat_buildingRow build in Data.Values)
        {
            if (!allBulidData.ContainsKey(build.id))
            {
                allBulidData.Add(build.id, build);
            }

        }

    }
    /// <summary>
    /// 设置建筑信息，每次打开建筑界面都需要读取（为了保证数据的一致性）
    /// </summary>
    /// <param name="data">Data.</param>
    public void setBuildingData(string data)
    {
        if (data == null)
        {
            removeAllGridChildren();
            return;

        }
        buildingData = data.Split(new char[]{ ',' });
//        buildingType = BuildingBlueprintType.Building;
        //显示 建筑信息
        showBuildingDataItem();
    }


    /// <summary>
    /// 蓝图界面点击购买
    /// </summary>
    /// <param name="go">Go.</param>
    void onClickPurchase()
    {
        if (SelectedItem.Count > 0)
        {
            AppFacade.GetInstance().SendNotification(NotiConst.PURCHASE_BLUEPRINT,SelectedItem);
            
        } 
    }

   
    /// <summary>
    /// 蓝图购买成功
    /// </summary>
    public void PurchaseBlueprintResult(bool isSuccess)
    {
        if (isSuccess)
        {
            Debug.Log("√√√√√√√√√蓝图购买成功");
            //播放购买成功的动画
        }
        else
            Debug.Log("XXXXXX蓝图购买失败");
        //提示蓝图购买成功，并且取消所有的选择

    }

    private void removeAllGridChildren()
    {
        if (BuildingGrid != null)
        {
            BuildingGrid.transform.DestroyChildren();
        }
    }
    /// <summary>
    /// 设置所有的标题为未选中 ,这样第一次选择的时候，不会因为之前是已选中的状态而不再不走显示Item 流程
    /// </summary>
    private void setAllTitleToggleUnselected()
    {
        int count = BuildingTitleGrid.transform.childCount;
        if (count > 0)
        {
            for (int i = 0; i < count; ++i)
            {
                UIToggle toggle = BuildingTitleGrid.transform.GetChild(i).GetComponent<UIToggle>();
                if (toggle != null)
                {
                    toggle.onChange.Clear();
                    //貌似不起作用，看代码意思是 设置value的条件不满足，所以，value的值仍然是原来的值
                    toggle.value = false;
                }
            }
        }
    }


    public void  showBuildingBlueprint(int foodbowId)
    {
        this.foodBowID = foodbowId;
        this.gameObject.SetActive(true);
        BuildingBlueprintGameObject.SetActive(true);
        removeAllGridChildren();
        setAllTitleToggleUnselected();
        showBulidingTitle();
    }

}
