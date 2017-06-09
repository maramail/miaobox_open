using UnityEngine;
using System.Collections;

public class BattleSceneManage : MonoBehaviour {
    public enum BattleState
    {
        Begin,
        ChooseQuene,
        ImproveCat,
        Battleing,
        Battleend


    }
    private Timer time;
    private BattleState CurrentBattlstate;
    private  BattleView battleview;
    private bool listen = true;

    void Start()
    {
        Reset();
        StartCoroutine("UpdateBattle");
        battleview = GameObject.Find("BattleView").GetComponent<BattleView>();
       


    }
    
    void Reset()
    {
        listen = true;
        CurrentBattlstate = BattleState.Begin;

    }
     IEnumerator  UpdateBattle()
    {
        while(true )
        {
             
             
              
            switch (CurrentBattlstate)
            {
                case BattleState.Begin:
                    Reset();
                    Debug.Log(" 游戏 开始 2秒后开始选择队伍 ");
                    GUIManage._instance.notifi(" 游戏 开始 2秒后开始选择队伍 ",2f);
                    time = new Timer(2f, true);
                    yield return new WaitForSeconds(2f);
                    battleview.StartChooseQuene();
                    CurrentBattlstate = BattleState.ChooseQuene;

                    break;
                case BattleState.ChooseQuene:
                    if (time.Ready())
                    {
                        
                        
                        Debug.Log("请在 5 秒内选择合适的队伍");
                        GUIManage._instance.notifi("请在 5 秒内选择合适的队伍",2f);
                        battleview.HideorShowPlane(true);
                        yield return new WaitForSeconds(2f);
                       
                        StartCoroutine(GUIManage._instance.showText(5f));
                        StartCoroutine("CheckIsCommit");
                        yield return new WaitForSeconds(5f);
                        if(!battleview.iscommit)
                        {
                            Debug.Log("执行");
                            battleview.commit();
                        }
                         
                      
                        time.Do();
                    }

                    CurrentBattlstate = BattleState.Battleend;
                    break;
                case BattleState.ImproveCat:
                    break;
                case BattleState.Battleing:
                    break;
                case BattleState.Battleend:
                    break;
                default:
                    break;
            }
            yield return new WaitForFixedUpdate();
        }


    }

     IEnumerator CheckIsCommit( )
    {
        while(listen )
        {
             
            if (battleview.iscommit && listen)
            {
                Debug.Log("listen");
                GUIManage._instance.cooldownhideorshow(false);
                listen = false;
            }
            if (CurrentBattlstate==BattleState.ChooseQuene)
            {
                Debug.Log("退出");
                break;
            }
             
            yield return new WaitForFixedUpdate();
        }
    }


}
