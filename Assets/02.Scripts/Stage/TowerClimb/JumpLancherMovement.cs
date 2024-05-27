using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpLancherMovement : MonoBehaviour
{
    public float bounceForce = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody playerRigidbody = other.GetComponent<Rigidbody>();
            if (playerRigidbody != null)
            {
                playerRigidbody.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
            }
        }
    }
}
