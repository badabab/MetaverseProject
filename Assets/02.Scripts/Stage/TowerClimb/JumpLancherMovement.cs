using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpLancherMovement : MonoBehaviour
{
    public float bounceForce;
    public float bounceDuration;

    private bool isBouncing = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isBouncing)
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                StartCoroutine(BouncePlayer(rb));
            }
        }
    }

    private IEnumerator BouncePlayer(Rigidbody rb)
    {
        isBouncing = true;
        float elapsedTime = 0f;
        Vector3 originalVelocity = rb.velocity;
        rb.velocity = new Vector3(rb.velocity.x, bounceForce, rb.velocity.z); // 위쪽으로 힘을 가함

        while (elapsedTime < bounceDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.velocity = originalVelocity; // 원래 속도로 되돌림
        isBouncing = false;
    }
}
