using UnityEngine;
using System.Collections;

public class CatIdle :  State<BattelCat> {
    private CatIdle() { }

    public override void Enter(BattelCat Obj)
    {
        base.Enter(Obj);
        Debug.Log("Enter Catidle");

    }
    public override void Execute(BattelCat Obj)
    {
        base.Execute(Obj);
        Debug.Log("Execute Cat ....");
       // Obj.ChangeState()
    }

    public override void Exit(BattelCat Obj)
    {
        base.Exit(Obj);
    }

}
