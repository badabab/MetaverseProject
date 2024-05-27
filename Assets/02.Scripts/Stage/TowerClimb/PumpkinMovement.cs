using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinMovement : MonoBehaviour
{
    public Transform objectA;
    public Transform objectB;
    public float targetYSelected = 800f;
    public float targetYRemaining = 10f;
    public float smoothSpeed = 0.1f;

    void Start()
    {
        Transform selectedObject = Random.Range(0, 2) == 0 ? objectA : objectB;
        Transform remainingObject = (selectedObject == objectA) ? objectB : objectA;

        StartCoroutine(MoveToY(selectedObject, targetYSelected, smoothSpeed));
        StartCoroutine(MoveToY(remainingObject, targetYRemaining, smoothSpeed));
    }

    IEnumerator MoveToY(Transform obj, float targetY, float smoothSpeed)
    {
        float currentY = obj.position.y;

        while (Mathf.Abs(currentY - targetY) > 0.01f)
        {
            currentY = Mathf.Lerp(currentY, targetY, smoothSpeed * Time.deltaTime);
            obj.position = new Vector3(obj.position.x, currentY, obj.position.z);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

        }

    }
}
