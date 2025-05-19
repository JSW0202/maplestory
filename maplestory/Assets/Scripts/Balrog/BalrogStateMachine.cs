using UnityEngine;

public class BalrogStateMachine
{
    public BalrogState currentState { get; private set; }

    public void Initialize(BalrogState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(BalrogState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
