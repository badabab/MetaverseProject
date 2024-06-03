using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinshLine : MonoBehaviour
{
    public GameObject FinshLineSphere;
    private float Speed = 50f;

    private void Update()
    {
        if (FinshLineSphere != null)
        {
            FinshLineSphere.transform.Rotate(0, Speed * Time.deltaTime, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }
    }
}

