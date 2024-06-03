using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Countdown : MonoBehaviour
{
    public GameObject CountDownUI;
    public TextMeshProUGUI CountText;
    public Image[] RoadingImages;
    public GameObject Go;
    public GameObject Ready;

    private bool isCountdownActive = false;
    private bool _isGo = false;
    private int _countdownCount = 5;
    void Update()
    {
        if (FallGuysManager.Instance._currentGameState == GameState.Loading)
        {
            StartCoroutine(Count_Coroutine());
            StartCoroutine(CountDown_Coroutine(5));
        }
        else if (FallGuysManager.Instance._currentGameState == GameState.Go)
        {
            StartCoroutine(Go_Coroutine());
        }
    }

    IEnumerator Count_Coroutine()
    {
        CountText.gameObject.SetActive(true);   
        for (int i = 5; i > 0; i--)
        {
            CountText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        CountText.gameObject.SetActive(false);
        yield break;
    }

    IEnumerator CountDown_Coroutine(int delay)
    {
        for (int i = 0; i < RoadingImages.Length; i++)
        {
            RoadingImages[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            RoadingImages[i].gameObject.SetActive(false);
        }
        _isGo = true;
        yield break;
    }

    IEnumerator Go_Coroutine()
    {
        if (_isGo == true) 
        {
            Go.SetActive(true);
        }
        yield return new WaitForSeconds(5);
        _isGo = false;
        Go.SetActive(false);
        yield break;
    }
}
