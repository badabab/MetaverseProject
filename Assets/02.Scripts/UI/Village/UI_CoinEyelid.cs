using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CoinEyelid : MonoBehaviour
{
    public float duration = 0.2f;
    public float interval;

    private void Start()
    {
        StartCoroutine(RotateEyelid());
    }

    private IEnumerator RotateEyelid()
    {
        while (true)
        {
            yield return StartCoroutine(Rotate(0, 90, duration));
            yield return StartCoroutine(Rotate(90, 0, duration));
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
            transform.rotation = Quaternion.Euler(angle, 0, 0);
            yield return null;
        }
        transform.rotation = Quaternion.Euler(toAngle, 0, 0);
    }
}
