public interface IEnemyState<T>
{
    void OnEnter();
    void OnExecute();
    void OnFixedExecute();
    void OnExit();
    void AddTransition(T input, IEnemyState<T> enemyState);
    bool GetTransition(T input, out IEnemyState<T> enemyState);
}
