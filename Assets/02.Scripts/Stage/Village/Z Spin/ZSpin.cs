using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZSpin : MonoBehaviour
{
    public float Spinspeed;

    void Update()
    {
        transform.Rotate(0, 0, Spinspeed * Time.deltaTime);
    }
}

