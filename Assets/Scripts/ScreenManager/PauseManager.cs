using System.Collections.Generic;
using UnityEngine;

public class PauseManager : SingltonBase<PauseManager>
{
    List<MonoBehaviour> _owners = new List<MonoBehaviour>();

    public void Pause(MonoBehaviour mono)
    {
        Time.timeScale = 0f;

        if (_owners.Contains(mono)) return;
        
        _owners.Add(mono);
    }

    public void Unpause(MonoBehaviour mono)
    {
        if(_owners.Contains(mono)) _owners.Remove(mono);

        if(_owners.Count <= 0) Time.timeScale = 1f;
    }
}
