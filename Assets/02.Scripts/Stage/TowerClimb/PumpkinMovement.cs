using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinMovement : MonoBehaviour
{
    public Transform object1;
    public Transform object2;
    public GameObject pumpkin1;
    public GameObject pumpkin2;
    public float targetYSelected = 800f;
    public float targetYRemaining = 10f;
    public float smoothSpeed = 0.1f;

    private bool isMoving1 = false;
    private bool isMoving2 = false;

    void Start()
    {

    }

    IEnumerator MoveToY(Transform obj, float targetY, float smoothSpeed, bool isMoving)
    {
        float currentY = obj.position.y;

        while (Mathf.Abs(currentY - targetY) > 0.01f)
        {
            currentY = Mathf.Lerp(currentY, targetY, smoothSpeed * Time.deltaTime);
            obj.position = new Vector3(obj.position.x, currentY, obj.position.z);
            yield return null;
        }

        if (isMoving)
        {
            yield return new WaitForSeconds(5f);
            StartCoroutine(MoveToY(obj, obj == object1 ? targetYRemaining : targetYSelected, smoothSpeed, false));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject == pumpkin1)
            {
                Transform selectedObject = Random.Range(0, 2) == 0 ? object1 : object2;
                Transform remainingObject = (selectedObject == object1) ? object2 : object1;

                StartCoroutine(MoveToY(selectedObject, targetYSelected, smoothSpeed, true));
                StartCoroutine(MoveToY(remainingObject, targetYRemaining, smoothSpeed, false));
            }
            else if (other.gameObject == pumpkin2)
            {
                Transform selectedObject = Random.Range(0, 2) == 0 ? object1 : object2;
                Transform remainingObject = (selectedObject == object1) ? object2 : object1;

                StartCoroutine(MoveToY(selectedObject, targetYSelected, smoothSpeed, false));
                StartCoroutine(MoveToY(remainingObject, targetYRemaining, smoothSpeed, true));
            }
        }
    }
}