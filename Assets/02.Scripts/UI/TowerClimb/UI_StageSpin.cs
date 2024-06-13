using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageSpin : MonoBehaviour
{
    public float Spinspeed;
    private bool shouldStopRotating = false;
    private float targetRotation = 135f;
    private float rotationThreshold = 1f;
    private string currentStageNumber;
    public TMP_Text StageNum;

    public void SetStageNum(string stageNumber)
    {
        StageNum.text = stageNumber;
        currentStageNumber = stageNumber;
        shouldStopRotating = false;
    }

    public void StopRotation()
    {
        shouldStopRotating = true;
    }

    public void Spin()
    {
        if (!shouldStopRotating)
        {
            transform.Rotate(0, 0, Spinspeed * Time.deltaTime);

            if (Mathf.Abs(transform.rotation.eulerAngles.z - targetRotation) < rotationThreshold)
            {
                StopRotation();
            }
        }
    }
    void Update()
    {
        Spin();
    }
}
