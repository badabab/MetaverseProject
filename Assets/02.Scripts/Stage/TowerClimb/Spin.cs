using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float Spinspeed;

    void Update()
    {
        transform.Rotate(0, Spinspeed * Time.deltaTime, 0);
    }
}
