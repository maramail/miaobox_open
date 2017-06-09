using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 测试类
/// </summary>
/// 
    
public class CreatPoint :Singleton<CreatPoint> {
   
    private const float  cosAngle = 18*PI;
    private const float  culAngle = 54*PI;
    private const float   PI =  Mathf.PI/180;
    private const float mathangle = 72f;


     
    private int rotatedir = 1;

    [Range(1,72)]
    public float speed = 1f;

    private bool isok = false;
    private const float angle = 72 * PI;
    private float timer = 0;
    public Transform CenterPoint;
    public int Cir_R=1;
    private List<Vector3> PointList = new List<Vector3>();                           //储存位置信息
    private List<GameObject> PrefabsList = new List<GameObject>();                   //储存猫对象
    private Dictionary<int, Vector3> flagposdir = new Dictionary<int, Vector3>();    // 对应位置ID
                                          
   
   
	public enum PointId :int { One=0,Two=1,Three=2,Four=3,Five=4};
    private PointId CurrentPointEnum = PointId.One;
    
     
    private MiaoData miaodata ;
     
   
    void Start()
    {
        
        for (int i = 0; i < 5; i++)
        {
            CalculationPoint(CenterPoint.position, Cir_R);
         
            Debug.Log(PointList[i]);
            
        }
        
      
        Debug.Log(Mathf.Sin(90*PI));
        //MoveGameobj(PrefabsList, PointList);

    }


    /// <summary>
    /// 接收外部转来的队伍 
    /// </summary>
    /// <param name="objList"></param>
    public    void   Creatprefabs(List<GameObject>  objList)
    {
        PrefabsList.Clear();
       if(objList.Count<=0)
        {

            Debug.Log("队伍为空 ");
            return;

        }
       else
        {
            PrefabsList = objList;

        }
        for (int i=0;i< PrefabsList.Count; i++)
        {

            PrefabsList[i].SetActive(true);
            PrefabsList[i].transform.localPosition = PointList[i];
            PrefabsList[i].transform.rotation = CenterPoint.transform.rotation;
            miaodata = PrefabsList[i].AddComponent<MiaoData>();
            miaodata._currentid = i;
            miaodata._point = PointList[i];

        }

    }
    void CalculationPoint(Vector3 Point,int R)
    {
        Vector3 tempPoint = new Vector3(0,0,0);

        tempPoint.y = Point.y;
        switch (CurrentPointEnum)
        {
            case PointId.One:
                 tempPoint.x = Point.x - R * Mathf.Cos(cosAngle);
                 tempPoint.z = Point.z + R * Mathf.Sin(cosAngle);
                //tempPoint.x = Point.x - R;
               // tempPoint.z = Point.z;
                PointList.Add(tempPoint);
                flagposdir.Add( 0, tempPoint);
                CurrentPointEnum = PointId.Two;
               
                break;
            case PointId.Two:
                tempPoint.x = Point.x;
                tempPoint.z = Point.z + R;
               // tempPoint.x = Point.x - R*Mathf.Cos(angle);
               // tempPoint.z = Point.z+R* Mathf.Sin(angle); 
                PointList.Add(tempPoint);
                flagposdir.Add(1, tempPoint);
                CurrentPointEnum = PointId.Three;
                
                break;
            case PointId.Three:
                 tempPoint.x = Point.x + R * Mathf.Cos(cosAngle);
               tempPoint.z = Point.z + R * Mathf.Sin(cosAngle);
               // tempPoint.x = Point.x - R * Mathf.Cos(angle*2);
                //tempPoint.z = Point.z + R * Mathf.Sin(angle*2);
                PointList.Add(tempPoint);
                flagposdir.Add(2, tempPoint);
                CurrentPointEnum = PointId.Four;
                
                break;
            case PointId.Four:
                tempPoint.x = Point.x + R * Mathf.Cos(culAngle);
                tempPoint.z = Point.z - R * Mathf.Sin(culAngle);
               // tempPoint.x = Point.x - R * Mathf.Cos(angle * 3);
                //tempPoint.z = Point.z + R * Mathf.Sin(angle * 3);
                PointList.Add(tempPoint);
                flagposdir.Add(3, tempPoint);
                CurrentPointEnum = PointId.Five;
                
                break;
            case PointId.Five:
                tempPoint.x = Point.x - R * Mathf.Cos(culAngle);
               tempPoint.z = Point.z - R * Mathf.Sin(culAngle);
               // tempPoint.x = Point.x - R * Mathf.Cos(angle * 4);
               // tempPoint.z = Point.z + R * Mathf.Sin(angle * 4);
                PointList.Add(tempPoint);
                flagposdir.Add(4, tempPoint);
                CurrentPointEnum = PointId.Five;
                
                break;
        }  
    }
    void Update()
    { 

        if (Input.GetKeyDown(KeyCode.X))
        {
            isok = true;


        }
        if (isok)
        {
            timer += speed;
            if (timer <= mathangle)
            {
              
                    MoveGameobj(PrefabsList, rotatedir* speed);
             
                
               
                
            }
            else {

                isok = false;
                timer = 0f;
                reset();
             
               
                
            }
        }
           
        
    }
    /// <summary>
    /// 所有移动
    /// </summary>
    /// <param name="currentlist"></param>
    /// <param name="angledir"></param>
    void MoveGameobj(List<GameObject> currentlist,float angledir)
    {
         
        if(currentlist==null||currentlist.Count<=0)
        {

            return;
        }
        foreach(GameObject obj in currentlist)
        {

            obj.transform.RotateAround(CenterPoint.position, CenterPoint.up, angledir);
        }
     


    }
   
    void reset( )
    {

        
       foreach(GameObject obj in PrefabsList)
        {
            foreach(Vector3 pos in PointList)
            {
                if (Vector3.Distance(obj.transform.position, pos) <= 1)
                {
                    obj.transform.position = pos;
                    obj.transform.rotation = CenterPoint.rotation;
                    foreach(KeyValuePair<int, Vector3> dir in flagposdir)
                    {
                        if(dir.Value==obj.transform.position)
                        {
                            obj.GetComponent<MiaoData>()._currentid = dir.Key;

                        }
                    }
                   
                }
            }
        }

           

        

    }
    public void changepos(int dir )
    { 
        isok = true;rotatedir = dir;

    }
    

    
}
