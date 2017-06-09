using UnityEngine;
using System.Collections;

public class CatAttack : State<BattelCat> {

    private CatAttack() { }
    public override void Enter(BattelCat Obj)
    {
        base.Enter(Obj);
        Debug.Log("Enter CatAttack");

    }
    public override void Execute(BattelCat Obj)
    {
        base.Execute(Obj);
        Debug.Log("Execute Cat Attack ....");
        // Obj.ChangeState()
    }

    public override void Exit(BattelCat Obj)
    {
        base.Exit(Obj);
    }

}

