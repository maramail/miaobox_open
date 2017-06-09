using UnityEngine;
using System.Collections;

public class State<T> : Singleton<State<T>>
{
    public virtual  void Enter(T Obj)
    {

    }  

    public virtual  void Execute(T Obj)
    {

    }

    public virtual void Exit(T Obj)
    {

    }

}
