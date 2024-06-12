using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum StageState
{
    Stage0,
    Stage1,
    Stage2,
    Stage3,
    Stage4,
    Stage5,
}
public class UI_PlayerStage : MonoBehaviour
{
    public UI_StageSpin stageSpinner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StageState state;
            switch (gameObject.name)
            {
                case "Stage5":
                    state = StageState.Stage5;
                    break;
                case "Stage4":
                    state = StageState.Stage4;
                    break;
                case "Stage3":
                    state = StageState.Stage3;
                    break;
                case "Stage2":
                    state = StageState.Stage2;
                    break;
                case "Stage1":
                    state = StageState.Stage1;
                    break;
                case "Stage0":
                    state = StageState.Stage0;
                    break;
                default:
                    state = StageState.Stage0;
                    break;
            }

            stageSpinner.SetStageNum(state.ToString());
        }
    }
}
