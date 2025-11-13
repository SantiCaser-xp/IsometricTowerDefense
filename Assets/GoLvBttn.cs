using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoLvBttn : MonoBehaviour
{
    public string actualLevelName;

    [SerializeField] private Button _button;

    //private void Start()
    //{ 
    //    _button.onClick.AddListener(() => SceneTransition.Instance.LoadLevel(actualLevelName));
    //}

    public void GoToLevel()
    {
        SceneTransition.Instance.LoadLevel(actualLevelName);
    }
}
