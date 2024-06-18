using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject FuseFire;
    public List<GameObject> CannonFire;
    public List<GameObject> ShipFire;
    private bool isFireActive = false;
    private float FuseFireTime = 0.5f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFireActive)
        {
            StartCoroutine(ActivateFuseFireForSeconds(FuseFireTime));
            RestartCannonFire(); // CannonFire 재시작 함수 호출
        }
    }

    IEnumerator ActivateFuseFireForSeconds(float seconds)
    {
        isFireActive = true;
        FuseFire.SetActive(true);

        yield return new WaitForSeconds(seconds);

        FuseFire.SetActive(false);
        isFireActive = false;
    }

    void RestartCannonFire()
    {
        if (CannonFire != null)
        {
            foreach (GameObject cannon in CannonFire)
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