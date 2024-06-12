using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FinishingLineCount : MonoBehaviour
{
    public UI_StageUI uI_StageUI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            string stageNumber = "";

            switch (gameObject.name)
            {
                case "FinishingLineCount1":
                    stageNumber = "1";
                    break;
                case "FinishingLineCount2":
                    stageNumber = "2";
                    break;
                case "FinishingLineCount3":
                    stageNumber = "3";
                    break;
                default:
                    break;
            }
            uI_StageUI.SetStageNum(stageNumber);
        }
    }
}
