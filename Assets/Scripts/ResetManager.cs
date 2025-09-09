using UnityEngine;

public class ResetManager : MonoBehaviour
{
    public void ResetAll()
    {
        foreach (var resettable in FindObjectsOfType<MonoBehaviour>(true))
        {
            if (resettable is IResettable<int> intReset)
                intReset.Reset();
            else if (resettable is IResettable<float> floatReset)
                floatReset.Reset();
        }
    }
}