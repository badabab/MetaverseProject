using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private GameObject fuseFire;
    [SerializeField] private List<GameObject> cannonFire;
    [SerializeField] private List<GameObject> shipFire;

    private bool isFireActive = false;
    private const float fuseFireTime = 0.5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFireActive)
        {
            StartCoroutine(ActivateFuseFireForSeconds(fuseFireTime));
            RestartCannonFire();
        }
    }

    IEnumerator ActivateFuseFireForSeconds(float seconds)
    {
        isFireActive = true;
        if (fuseFire != null)
        {
            fuseFire.SetActive(true);

            yield return new WaitForSeconds(seconds);

            fuseFire.SetActive(false);
        }
        isFireActive = false;
    }

    void RestartCannonFire()
    {
        if (cannonFire != null)
        {
            foreach (GameObject cannon in cannonFire)
            {
                if (cannon != null)
                {
                    cannon.SetActive(true);
                    ParticleSystem particleSystem = cannon.GetComponentInChildren<ParticleSystem>();
                    if (particleSystem != null)
                    {
                        particleSystem.Play();
                    }
                }
            }
        }
    }
}