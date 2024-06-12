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
    public TMP_Text StageNum;

    void Update()
    {
        if (!shouldStopRotating)
        {
            transform.Rotate(0, 0, Spinspeed * Time.deltaTime);

            // Check if the current rotation is close to the target rotation
            if (Mathf.Abs(transform.rotation.eulerAngles.z - targetRotation) < rotationThreshold)
            {
                shouldStopRotating = true;
            }
        }
    }
}
