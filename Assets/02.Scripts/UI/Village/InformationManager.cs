using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InformationManager : MonoBehaviour
{
    public GameObject Information;
    public GameObject Map;

    private bool isInformationAtInitialPosition = true;
    private bool isMapAtInitialPosition = true;
    private bool isInformationMoving = false;
    private bool isMapMoving = false;
    private Vector3 informationInitialPosition;
    private Vector3 informationTargetPosition;
    private Vector3 mapInitialPosition;
    private Vector3 mapTargetPosition;

    void Start()
    {
        informationInitialPosition = new Vector3(Information.GetComponent<RectTransform>().localPosition.x, 390, Information.GetComponent<RectTransform>().localPosition.z);
        informationTargetPosition = new Vector3(Information.GetComponent<RectTransform>().localPosition.x, 110, Information.GetComponent<RectTransform>().localPosition.z);
        mapInitialPosition = new Vector3(Map.GetComponent<RectTransform>().localPosition.x, 700, Map.GetComponent<RectTransform>().localPosition.z);
        mapTargetPosition = new Vector3(Map.GetComponent<RectTransform>().localPosition.x, 400, Map.GetComponent<RectTransform>().localPosition.z);

        Information.GetComponent<RectTransform>().localPosition = informationInitialPosition;
        Map.GetComponent<RectTransform>().localPosition = mapInitialPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isInformationMoving)
        {
            StartCoroutine(MoveUI(Information.GetComponent<RectTransform>(), isInformationAtInitialPosition ? informationTargetPosition : informationInitialPosition, () => isInformationMoving = false));
            isInformationAtInitialPosition = !isInformationAtInitialPosition;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !isMapMoving)
        {
            StartCoroutine(MoveUI(Map.GetComponent<RectTransform>(), isMapAtInitialPosition ? mapTargetPosition : mapInitialPosition, () => isMapMoving = false));
            isMapAtInitialPosition = !isMapAtInitialPosition;
        }
    }

    IEnumerator MoveUI(RectTransform rectTransform, Vector3 targetPosition, System.Action onComplete)
    {
        SoundManager.instance.PlaySfx(SoundManager.Sfx.UI_Village12MButton);
        float elapsedTime = 0;
        float duration = 0.5f;
        Vector3 startingPosition = rectTransform.localPosition;

        while (elapsedTime < duration)
        {
            rectTransform.localPosition = Vector3.Lerp(startingPosition, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rectTransform.localPosition = targetPosition;
        onComplete();
    }
}
