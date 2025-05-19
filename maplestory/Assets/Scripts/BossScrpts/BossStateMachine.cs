using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BossState 객체 패턴을 위한 상태 머신.
/// AddTransition / AddAnyTransition 으로 전이 규칙 등록.
/// </summary>
[RequireComponent(typeof(BaseBoss))]
public class BossStateMachine : MonoBehaviour
{
    private BossState currentState;
    private readonly List<Transition> transitions    = new();
    private readonly List<Transition> anyTransitions = new();

    private BaseBoss boss;

    private void Awake()
    {
        boss = GetComponent<BaseBoss>();
    }

    private void Update()
    {
        // 1) Any-State 전이 우선 체크 (우선순위 높음)
        foreach (var t in anyTransitions)
        {
            if (t.Condition())
            {
                if (t.To != currentState)
                    SetState(t.To);
                return;
            }
        }

        // 2) 현재 상태 Update
        currentState?.Update();

        // 3) 일반 전이 체크 (우선순위 낮음)
        foreach (var t in transitions)
        {
            if (t.Condition())
            {
                if (t.To != currentState)
                    SetState(t.To);
                return;
            }
        }
    }

    /// <summary>초기 상태 설정</summary>
    public void SetInitialState(BossState state)
    {
        currentState = state;
        currentState.Enter();
    }

    /// <summary>강제 상태 변경</summary>
    public void ChangeState(BossState newState) => SetState(newState);

    private void SetState(BossState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    /// <summary>어느 상태에서나 to 로 전이 (우선순위 높음)</summary>
    public void AddAnyTransition(BossState to, Func<bool> condition)
    {
        anyTransitions.Add(new Transition(to, condition));
        Debug.Log("특수전이 : " + to + " " + condition);
    }

    /// <summary>일반 전이 (우선순위 낮음)</summary>
    public void AddTransition(BossState to, Func<bool> condition)
    {
        transitions.Add(new Transition(to, condition));
        Debug.Log("일반전이 : " + to + " " + condition);
    }

    // 내부 전이 정보 저장용
    private class Transition
    {
        public BossState To;
        public Func<bool> Condition;
        public Transition(BossState to, Func<bool> condition)
        {
            To        = to;
            Condition = condition;
        }
    }
}