using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyFSMStates
{
    Idle,
    Move,
    Attack,
    Die
}

public class EnemyFSM<T, J> where J : MonoBehaviour
{
    //public IEnemyState<T> _actualState;
    public EnemyState<T, J> _actualState;
    public Dictionary<T, EnemyState<T, J>> _possibleStates = new Dictionary<T, EnemyState<T, J>>();

    public EnemyFSM()
    {

    }
    public void SetInitialEnemyState(EnemyState<T, J> firstState)
    {
        _actualState = firstState;
        _actualState.OnEnter();
    }
    public void OnExecute() => _actualState.OnExecute();
    public void OnFixedExecute() => _actualState.OnFixedExecute();




    public void ChangeState(T input)
    {
        //if(_actualState.GetTransition(input,out EnemyState<T,J> newState))
        //{
        //    _actualState?.OnExit();
        //    _actualState = newState;
        //    _actualState.OnEnter();
        //}
        if (!_possibleStates.ContainsKey(input)) return;
        _actualState?.OnExit();
        _actualState = _possibleStates[input];
        _actualState.OnEnter();
    }

}

