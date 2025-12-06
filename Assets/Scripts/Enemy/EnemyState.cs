using System.Collections.Generic;
using UnityEngine;

public class EnemyState<T, J> where J : MonoBehaviour       /*IEnemyState<T>*/
{
    protected EnemyFSM<T,J> enemyFSM;
    protected J avatar;
    private Dictionary<T, IEnemyState<T>> transitions = new Dictionary<T, IEnemyState<T>>();

    public EnemyState<T, J> SetUp(EnemyFSM<T,J> newFSM)
    {
        enemyFSM = newFSM;
        return this;
    }
    public EnemyState<T, J> SetAvatar(J newAvatar)
    {
        avatar = newAvatar;
        return this;
    }
    public virtual void OnEnter()
    {
        //Debug.Log($"Entered {this.GetType().ToString()} state");
    }
    public virtual void OnExecute() { }
    public virtual void OnFixedExecute() { }
    public virtual void OnExit()
    {
        //Debug.Log($"Exit {this.GetType().ToString()} state");
    }


    /*public void AddTransition(T input, IEnemyState<T> enemyState)
    {
        transitions.Add(input, enemyState);
    }

    public bool GetTransition(T input, out IEnemyState<T> enemyState)
    {
        if (transitions.ContainsKey(input))
        {
            enemyState = transitions[input];
            return true;
        }
        enemyState = null;
        return false;
    }*/
}
