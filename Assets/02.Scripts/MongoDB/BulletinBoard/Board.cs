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
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            other.GetComponent<PlayerMoveAbility>().isGrounded = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponentInParent<PhotonView>().IsMine)
        {
            Canvas.gameObject.SetActive(false);
            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            other.GetComponent<PlayerMoveAbility>().isGrounded = true;
        }
    }
}
