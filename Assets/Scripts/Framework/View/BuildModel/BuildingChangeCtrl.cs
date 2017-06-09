using UnityEngine;
using System.Collections;
using System.Collections.Generic;




/*
 * 
 * 
 * 
 * 类名：BuildingChangeCtrl
 * 
 * 
 * 
 * 作用：控制模型切换和更新加载的方法
 * 
 * 日期 2017/1/6
 * */
public class BuildingChangeCtrl : MonoBehaviour
{
    //  public GameObject UIButton;
     
    private bool isbuilt { get; set; }                                       //是否建立
    private int ModelTrsID;                                                  //建筑的位置ID

    public GameObject[] Model;                                                //加载模型对象
    private Dictionary<int, Transform> Buildspts = new Dictionary<int, Transform>();//模型位置字典
    private Dictionary<int, List<GameObject>> tempmodel = new Dictionary<int, List<GameObject>>();
    private  Dictionary<int, BuildModelVo> BuildModelVoDic=null ;
   






    //private Dictionary<int, stat_buildingspRow> statbuildingspdata = new Dictionary<int, stat_buildingspRow>();

    void Start()
    {
        Init();
        getfoodBowIdBuiltdata();

    }

    /// <summary>
    /// 初始化 将foodbow的位置与模型对应
    /// </summary>
    public void Init()
    {


        foreach (GameObject temp in Model)
        {
            BuildingBluepointCtrl cl = temp.GetComponent<BuildingBluepointCtrl>();
            ModelTrsID = cl.modeltrsID;
            Transform ts = temp.transform;
            if (!Buildspts.ContainsKey(ModelTrsID))
            {
                Buildspts.Add(ModelTrsID, ts);

            }


        }

    }

    /// <summary>
    /// 得到模型数据
    /// </summary>
    void getfoodBowIdBuiltdata()
    {
        AppFacade.getInstance.SendNotification(NotiConst.GET_BUILDING_MODEL_DATA);


    }

    public  void showinitmodel(object data  )
    {
       BuildModelVoDic=data as Dictionary<int ,BuildModelVo> ;
        foreach (GameObject temp in Model)
        {
                BuildingBluepointCtrl cl = temp.GetComponent<BuildingBluepointCtrl>();
            
            
                if (BuildModelVoDic.ContainsKey(cl.modeltrsID))
                {

                    InitBuilt(BuildModelVoDic[cl.modeltrsID], cl.foodBowID);
                }
                

           




        }


    }
    
    /// <summary>
    /// 加载时更新模型
    /// </summary>
    /// <param name="foodbowid"></param>
    public void InitBuilt(BuildModelVo  vo ,int foodbow)
    {
        if (tempmodel != null && !tempmodel.ContainsKey(vo.ModeltrsId))
        {
            if (vo.Modelid != 0)
            {
                setmodelfalse(vo);
            }
            List < GameObject > list= new List<GameObject>();
            for (int i = 1; i < 7; i++)
            {
                GameObject Model = BulitPool.GetInstance().GetBulitPool(i).CreateObject(Buildspts[vo.ModeltrsId].position);
                BuildingBluepointCtrl bl = Model.AddComponent<BuildingBluepointCtrl>();
                bl.modeltrsID = vo.ModeltrsId;
                bl.isBuild = true;
                bl.foodBowID = foodbow;
                list.Add(Model);
                 
                if (i == vo.Modelid)
                {
                    Model.SetActive(true);

                }
                else
                {
                    Model.SetActive(false);

                }
               
               
            }
            tempmodel.Add(vo.ModeltrsId, list);


        }
   
    
            

        
    }
    /// <summary>
    /// 建造模型
    /// </summary>
    /// <param name="vo"></param>
    public void ChangeBulit(BuildModelVo vo)
    {
        Debug.Log(vo.Modelid + "  " + vo.ModeltrsId);
        setmodelfalse(vo);
    if (tempmodel.ContainsKey(vo.ModeltrsId))
       {
            for (int i = 1; i < 7; i++)
            {
                if (i == vo.Modelid)
                {
                    tempmodel[vo.ModeltrsId][i-1].SetActive(true);
                }
                else
                {
                    tempmodel[vo.ModeltrsId][i-1].SetActive(false );
                }

            }

        }
   /* GameObject model = BulitPool.GetInstance().GetBulitPool(vo.Modelid).CreateObject(Buildspts[vo.ModeltrsId].position);
    BuildingBluepointCtrl bl = model.AddComponent<BuildingBluepointCtrl>();
    bl.modeltrsID = vo.ModeltrsId;
    bl.foodBowID = vo.foodbowid;
    if (vo.Modelid == 1)
    {
        bl.isBuild = false;
    }
    else 
    bl.isBuild = true;
    if (Model[vo.ModeltrsId - 1] != null)
    {
       Model[vo.ModeltrsId - 1].SetActive(false);
    }

      
       */


    }

    private void  setmodelfalse(BuildModelVo  vo)
    {
        foreach (GameObject temp in Model)
        {
            BuildingBluepointCtrl cl = temp.GetComponent<BuildingBluepointCtrl>();
            if (cl.modeltrsID == vo.ModeltrsId)
            {
                 
                temp.SetActive(false);
            }
        }

    }


}