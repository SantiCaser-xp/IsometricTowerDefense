using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScreen : MonoBehaviour
{
    public GameObject _pausePanel;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(ScreenManager.Instance.IfScreenActive(_pausePanel))
            {
                ScreenManager.Instance.DeactivateScreen();
            }
            else
            {
                ScreenManager.Instance.ActivateScreen(_pausePanel);
            }
        }
    }
}
