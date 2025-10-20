using UnityEngine;

public interface IBuildingState
{
    void EndState();
    void OnAction(Vector3 position);
    void UpdateState(Vector3 position);
}