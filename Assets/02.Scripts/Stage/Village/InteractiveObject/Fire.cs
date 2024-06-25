using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public PhotonView PhotonView { get; private set; }

    [SerializeField] private GameObject fuseFire;
    [SerializeField] private List<GameObject> cannonFire;
    [SerializeField] private List<GameObject> shipFire;

    private bool isFireActive = false;
    private bool isShipFireRoutineRunning = false;
    private const float fuseFireTime = 0.5f;
    private const float shipFireInterval = 6f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isFireActive)
        {

            StartCoroutine(ActivateFuseFireForSeconds(fuseFireTime));
            RestartCannonFire();

            int collisionCount = CalculateCollisionCount();
            if (!isShipFireRoutineRunning)
            {
                StartCoroutine(RestartShipFireRoutine(collisionCount));
            }
        }
    }

    IEnumerator ActivateFuseFireForSeconds(float seconds)
    {
        SoundManager.instance.PlaySfx(SoundManager.Sfx.VillageInteractiveObjectCannon);
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

    IEnumerator RestartShipFireRoutine(int collisionCount)
    {
        isShipFireRoutineRunning = true;
        yield return new WaitForSeconds(shipFireInterval);

        int firesToActivate = Mathf.Min(collisionCount, shipFire.Count);
        List<int> activatedIndices = new List<int>();

        for (int i = 0; i < firesToActivate; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, shipFire.Count);
            } while (activatedIndices.Contains(randomIndex));

            activatedIndices.Add(randomIndex);
        }

        foreach (int index in activatedIndices)
        {
            GameObject selectedShipFire = shipFire[index];
            if (selectedShipFire != null)
            {
                selectedShipFire.SetActive(true);
                ParticleSystem particleSystem = selectedShipFire.GetComponentInChildren<ParticleSystem>();
                if (particleSystem != null)
                {
                    particleSystem.Play();
                }
            }
        }

        isShipFireRoutineRunning = false;
    }

    int CalculateCollisionCount()
    {
        return Random.Range(1, shipFire.Count + 1);
    }
}

