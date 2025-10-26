using System.Collections.Generic;
using UnityEngine;

public class MementoState
{

    public Vector3 position;
    public Quaternion rotation;
    public float life;
    public int gold;

    //object[] data;
    List<ParamsMemento> _data = new List<ParamsMemento>();

    public void Rec(params object[] p)
    {
        if (_data.Count >= 1000)
        {
            _data.RemoveAt(0);
        }

        _data.Add(new ParamsMemento(p));
    }
    public bool IsRemember()
    {
        return _data.Count > 0;
    }


    public ParamsMemento Remember()
    {
        var x = _data[_data.Count - 1];
        _data.Remove(x);

        return x;
    }
}
