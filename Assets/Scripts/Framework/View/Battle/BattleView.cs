using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;


public class BattleView : MonoBehaviour
{

    public GameObject Grid;
    public GameObject Titleflag;
    public GameObject BtnCommit;
    public GameObject BtnLeft;
    public GameObject BtnRight;
    public GameObject BGPlane;
    public GameObject ChangeBtn;
    private int CurrentGroupInfo = 0;



    private List<string> TitleList = new List<string>();
    private List<GameObject> prefabslist = new List<GameObject>();

    private bool _iscommit;
    public bool iscommit
    {
        get
        { return _iscommit; }
        private set { _iscommit = value; }
    }    //确认是否提交


    void Start()
    {
        ChangeBtn.SetActive(false);
        UIEventListener.Get(BtnCommit).onClick += OnButtonClick;
        UIEventListener.Get(BtnLeft).onClick += OnButtonClick;
        UIEventListener.Get(BtnRight).onClick += OnButtonClick;
        TitleList.Add("探险一组");
        TitleList.Add("探险二组");
        TitleList.Add("探险三组");
      //  GetCatCurrentGroupData();

    }

    void OnButtonClick(GameObject go)
    {
        if (go == BtnRight)
        {
            Debug.Log("BtnRight");
            ChangeGroup(go);
            GetCatCurrentGroupData();

        }
        else if (go == BtnLeft)
        {
            Debug.Log("BtnLeft");
            ChangeGroup(go);
            GetCatCurrentGroupData();

        }
        else if (go == BtnCommit)
        {
            commit();
        }

    }


    /// <summary>
    /// 得到猫分组信息  
    /// </summary>
    void GetCatCurrentGroupData()
    {
        JsonData data = new JsonData();
        data["id"] = ++CurrentGroupInfo;                           //对应索引相差1；
        Debug.Log("当前选择组" + CurrentGroupInfo);
        AppFacade.GetInstance().SendNotification(NotiConst.GET_BATTLE_CAT_GROUP_INFO, data);
    }

    /// <summary>
    /// 切换不同的猫分组 
    /// </summary>
    /// <param name="go"></param>
    void ChangeGroup(GameObject go)
    {
        if (go == BtnLeft)
        {
            string currentext = Titleflag.GetComponentInChildren<UILabel>().text;
            int index = TitleList.IndexOf(currentext) - 1;
            if (index == -1)
            {
                index = TitleList.Count - 1;
            }
            Titleflag.GetComponentInChildren<UILabel>().text = TitleList[index];
            CurrentGroupInfo = index;
        }
        else if (go == BtnRight)
        {

            string currentext = Titleflag.GetComponentInChildren<UILabel>().text;
            int index = TitleList.IndexOf(currentext) + 1;
            if (index == TitleList.Count)
            {
                index = 0;

            }
            Titleflag.GetComponentInChildren<UILabel>().text = TitleList[index];
            CurrentGroupInfo = index;

        }


    }

    /// <summary>
    /// 展示当前的猫分组
    /// </summary>
    public void ShowCurrentCatGroup(object data)
    {
        CatPool.GetInstance().DestructAll();
        prefabslist.Clear();                              //每次展示队伍前 ，清理上次的展示队伍
        JsonData cats = (JsonData)data;
        int count = (int)cats["count"];

        for (int i = 0; i < count; i++)
        {
            Debug.Log(" cat  count" + count);
            JsonData catinfo = cats[i];
            int cattype = (int)catinfo["cattypeid"];
            prefabslist.Add(CatPool.GetInstance().GetCatPool(cattype).CreateObject(Grid.transform.GetChild(i).transform.position));

            prefabslist[i].layer = 5;
            prefabslist[i].transform.SetChildLayer(5);
            /*  Vector3 prefabsize = prefabs.GetComponent<CharacterController>().bounds.size;
              Vector3 Reallyszie = new Vector3();
              Reallyszie.x = prefabs.transform.localScale.x * prefabsize.x;
              Reallyszie.y = prefabs.transform.localScale.y * prefabsize.y;
              Reallyszie.z = prefabs.transform.localScale.z * prefabsize.z;
              Debug.Log(Reallyszie);
              float sizex=  Grid.transform.GetChild(i).GetComponent<UISprite>().localSize.x;
              float sizey = Grid.transform.GetChild(i).GetComponent<UISprite>().localSize.y;
              Debug.Log(Grid.transform.GetChild(i).GetComponent<UISprite>().localSize);
              float offsety =   Reallyszie.y/2;
              */
            prefabslist[i].transform.position = new Vector3(prefabslist[i].transform.position.x, -0.25f, -0.1f);
            prefabslist[i].transform.eulerAngles = new Vector3(0, 180, 0);
            prefabslist[i].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            prefabslist[i].AddComponent<BattleDisplayAnimator>();



        }

    }

    /// <summary>
    /// 重复利用显示面板的模型 将其重置 
    /// </summary>
    private void ResetPrefabs()
    {

        if (prefabslist != null && prefabslist.Count > 0)
        {
            foreach (GameObject temp in prefabslist)
            {

                temp.layer = 0;
                temp.transform.SetChildLayer(0);
                //  temp.GetComponent<BattleDisplayAnimator>().enabled = false;
                temp.SetActive(false);
                temp.transform.eulerAngles = Vector3.zero;
                temp.transform.localScale = Vector3.one;

            }
        }

    }

    public void commit()
    {
        iscommit = true;
        Debug.Log("BtnSure");
        ResetPrefabs();
        HideorShowPlane(false);
        ChangeBtn.SetActive(true);
        CreatPoint.Instance.Creatprefabs(prefabslist);
        
        // GetCatGroupData();

    }
    public void HideorShowPlane(bool state)
    {
        BGPlane.SetActive(state);

    }
    public void StartChooseQuene()
    {
        HideorShowPlane(true);
        GetCatCurrentGroupData();
    }



}