using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpLancherMovement : MonoBehaviour
{
    public float bounceForce;
    public float bounceDuration;
    public float gravity = -9.81f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController controller = other.GetComponent<CharacterController>();
            if (controller != null)
            {
                StartCoroutine(BouncePlayer(controller));
            }
        }
    }

    private IEnumerator BouncePlayer(CharacterController controller)
    {
        float elapsedTime = 0f;
        float verticalVelocity = bounceForce;

        while (elapsedTime < bounceDuration)
        {
            controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
            verticalVelocity += gravity * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the player lands smoothly
        while (!controller.isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
            controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
            yield return null;
        }
    }
}
