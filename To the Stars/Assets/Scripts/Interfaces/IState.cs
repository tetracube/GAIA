using UnityEngine;
using System.Collections;

/// <summary>
/// State of a State Machine (SM). 
/// It holds the three actions to perform at the state. The behaviour at the entrance, during the state and at exit time.
/// </summary>
public interface IState
{
    void Enter();

    void UpdateState();

    void Exit();
}
