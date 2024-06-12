using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StageUI : MonoBehaviour
{
    public TMP_Text PassNumberText;
    public void SetStageNum(string stageNumber)
    {
        PassNumberText.text = stageNumber;
    }
}
