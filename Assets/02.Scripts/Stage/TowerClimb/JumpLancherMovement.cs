using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpLancherMovement : MonoBehaviour
{
    public float bounceForce;
    public float bounceDuration;

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
        while (elapsedTime < bounceDuration)
        {
            controller.Move(Vector3.up * bounceForce * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
