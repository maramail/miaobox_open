using UnityEngine;
using System.Collections;

public class StateMachine<T> {

    private T Owner;
    private  State<T> CurrentState;
    private  State<T> PreviousState;
    private  State<T> GlobalState;
    public StateMachine(T owner)
    {
        Owner = owner;
        

    }
    public void SetCurrentState(State<T > s)
    {
        CurrentState = s;

    }
    public void SetPreviousState(State<T> s)
    {
        PreviousState = s;
    }
    public void SetGlobalState(State<T> s)
    {
        GlobalState = s;
    }
    public void UpdateMs()
    {

        if (GlobalState)
            GlobalState.Execute(Owner);

        if (CurrentState)
            CurrentState.Execute(Owner);
    }
    public  void  ChangeSate(State<T> newState)
    {
        PreviousState = CurrentState;
        CurrentState.Exit(Owner);
        CurrentState = newState;
        CurrentState.Enter(Owner);

    }
    public void  RevertToPreviourSate()
    {

        ChangeSate(PreviousState);
    }
    public State<T> GetCurrentState()
    {
        return CurrentState;
    }
    public State<T> GetPreviousState()
    {
        return PreviousState;
    }
    public State<T> GetGlobalState()
    {
        return GlobalState;
    }
}
