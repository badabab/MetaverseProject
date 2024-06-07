using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelObject : MonoBehaviour
{
    public Transform OutTrans;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = OutTrans.position;
        }
    }
}
