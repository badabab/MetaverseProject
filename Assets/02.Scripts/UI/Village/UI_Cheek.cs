using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Cheek : MonoBehaviour
{
    public float duration; // 회전 시간
    public float interval; // 회전 주기

    private void Start()
    {
        StartCoroutine(RotateEyelid());
    }

    private IEnumerator RotateEyelid()
    {
        while (true)
        {
            yield return StartCoroutine(Rotate(95, 135, duration));
            yield return StartCoroutine(Rotate(135, 95, duration));
            yield return new WaitForSeconds(interval);
        }
    }

    private IEnumerator Rotate(float fromAngle, float toAngle, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float angle = Mathf.Lerp(fromAngle, toAngle, elapsed / duration);
            transform.rotation = Quaternion.Euler(0, 0, angle);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(0, 0, toAngle);
    }
}
