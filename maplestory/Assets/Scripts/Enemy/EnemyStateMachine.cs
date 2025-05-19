using UnityEngine;

public class EnemyStateMachine
{   

    //스테이트머신은 이게 끝임... 적의 상태를 받아올 변수 하나 선언해주고
    //그 변수를 처음에 참조시켜주고
    //상태 바꿀때는 기존 상태 Exit() 호출한뒤 _newState 엔터 들어가주는게 끝임

    public EnemyState currentState { get; private set; }

    public void Initialize(EnemyState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeState(EnemyState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }

    public void Stop()
    {
        if (currentState != null)
            currentState.Exit();

        currentState = null;
    }
}
