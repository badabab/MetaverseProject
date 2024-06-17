using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    public GameObject Cannon;
    public GameObject FuseFire;
    public List<GameObject> CannonFire;
    public List<GameObject> ShipFire;
    public List<GameObject> CannonWheels;
    public GameObject ShipDwon;

    private float FuseFireTime = 3f;
    private float CannonFireTime = 15f;
    private bool playerInTrigger = false;
    private bool canPressF = true;
    private float rotationDuration = 1f;
    private float CannonWheelsBack = -7f;
    private int pressCount = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && playerInTrigger && canPressF)
        {
            pressCount++;
            if (Cannon != null)
            {
                Cannon.SetActive(!Cannon.activeSelf);
            }

            if (FuseFire != null)
            {
                StartCoroutine(ActivateFuseFire());
            }

            canPressF = false;
            StartCoroutine(ResetCanPressF());

            if (pressCount >= 5)
            {
                StartCoroutine(RotateAndDeactivateShipDwon());
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("충돌");
            playerInTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("충돌끝");
            playerInTrigger = false;
        }
    }

    IEnumerator ActivateFuseFire()
    {
        FuseFire.SetActive(true);
        yield return new WaitForSeconds(FuseFireTime);
        FuseFire.SetActive(false);
        StartCoroutine(ActivateCannonFire());
    }

    IEnumerator ActivateCannonFire()
    {
        foreach (var fire in CannonFire)
        {
            fire.SetActive(true);
        }

        StartCoroutine(RotateCannonWheels());

        yield return new WaitForSeconds(5f);

        if (ShipFire.Count > 0)
        {
            int randomIndex = Random.Range(0, ShipFire.Count);
            ShipFire[randomIndex].SetActive(true);
        }

        yield return new WaitForSeconds(CannonFireTime - 5f);

        foreach (var fire in CannonFire)
        {
            fire.SetActive(false);
        }
    }

    IEnumerator RotateCannonWheels()
    {
        float elapsedTime = 0f;

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / rotationDuration;

            foreach (var wheel in CannonWheels)
            {
                Quaternion startRotation = Quaternion.Euler(CannonWheelsBack, wheel.transform.rotation.eulerAngles.y, wheel.transform.rotation.eulerAngles.z);
                Quaternion endRotation = Quaternion.Euler(0, wheel.transform.rotation.eulerAngles.y, wheel.transform.rotation.eulerAngles.z);
                wheel.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            }

            yield return null;
        }

        foreach (var wheel in CannonWheels)
        {
            wheel.transform.rotation = Quaternion.Euler(0, wheel.transform.rotation.eulerAngles.y, wheel.transform.rotation.eulerAngles.z);
        }
    }

    IEnumerator ResetCanPressF()
    {
        yield return new WaitForSeconds(20f);
        canPressF = true;
    }

    IEnumerator RotateAndDeactivateShipDwon()
    {
        float elapsedTime = 0f;
        float rotationDuration = 1f; 

        Quaternion startRotation = Quaternion.Euler(-90, ShipDwon.transform.rotation.eulerAngles.y, ShipDwon.transform.rotation.eulerAngles.z);
        Quaternion endRotation = Quaternion.Euler(90, ShipDwon.transform.rotation.eulerAngles.y, ShipDwon.transform.rotation.eulerAngles.z);

        while (elapsedTime < rotationDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / rotationDuration;
            ShipDwon.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        ShipDwon.SetActive(false); 
    }
}