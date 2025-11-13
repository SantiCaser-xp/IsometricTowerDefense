using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelsSign : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _levelTxt;
    [SerializeField] private GoLvBttn _goBttn;
    public void UpdateLevelSign(string lvName, string internalLvName)
    {

        _levelTxt.text = ($"Enter {lvName}?");
        _goBttn.actualLevelName = internalLvName;
    }
}
