public interface IObserver
{
    void UpdateData(float currentValue, float maxValue);
    void UpdateData(int value);
    void UpdateGameStatus(GameStatus status);
}