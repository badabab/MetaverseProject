using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StageRing : MonoBehaviour
{
    public UI_StageSpin stageSpinner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView photonView = other.GetComponent<PhotonView>();

            if (photonView != null && photonView.IsMine)
            {
                string stageNumber = "";

                switch (gameObject.name)
                {
                    case "Stage5RingUp":
                        stageNumber = "5";
                        break;
                    case "Stage4RingDown":
                    case "Stage4RingUp":
                        stageNumber = "4";
                        break;
                    case "Stage3RingDown":
                    case "Stage3RingUp":
                        stageNumber = "3";
                        break;
                    case "Stage2RingDown":
                    case "Stage2RingUp":
                        stageNumber = "2";
                        break;
                    case "Stage1RingDown":
                    case "Stage1RingUp":
                        stageNumber = "1";
                        break;
                    case "Stage0Down":
                    case "Stage0Up":
                        stageNumber = "0";
                        break;
                    default:
                        break;
                }

                stageSpinner.SetStageNum(stageNumber);
            }
        }
    }
}