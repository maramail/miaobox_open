using UnityEngine;
using System.Collections;

public class BattelCat  :MonoBehaviour {


   
    private StateMachine<BattelCat> StateMachine;
     
    private  CatVO cat;

    public BattelCat(CatVO  cat )
    {
        this.cat = cat;
        StateMachine = new StateMachine<BattelCat>(this);
        StateMachine.SetCurrentState(CatIdle.Instance);
        StateMachine.SetGlobalState(GlobalCatState.Instance);


    }

    public  void  Update()
    {
        StateMachine.UpdateMs();
    }

   
   public  StateMachine<BattelCat> GetMs()
    {
        return StateMachine;
    }
}
