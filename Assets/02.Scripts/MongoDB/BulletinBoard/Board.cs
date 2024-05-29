using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Canvas Canvas;
    private void Start()
    {
        Canvas.gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponentInParent<PhotonView>().IsMine)
        {
            Canvas.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponentInParent<PhotonView>().IsMine)
        {
            Canvas.gameObject.SetActive(false);
        }
    }
}
