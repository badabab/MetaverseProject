using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Countdown_B : MonoBehaviour
{
    public GameObject CountDownUI;
    public TextMeshProUGUI CountText;
    public Image[] RoadingImages;
    public GameObject Go;
    public GameObject Ready;

    private bool isCountCoroutineRunning = false;
    private bool isCountDownCoroutineRunning = false;
    private bool isGoCoroutineRunning = false;
    private bool _isGo = false;
    private int _countdownCount = 5;

    private GameState previousGameState;

    void Start()
    {
        previousGameState = BattleTileManager.Instance._currentGameState;
    }

    void Update()
    {
        GameState currentGameState = BattleTileManager.Instance._currentGameState;

        if (currentGameState != previousGameState)
        {
            if (currentGameState == GameState.Loading)
            {
                if (!isCountCoroutineRunning && !isCountDownCoroutineRunning)
                {
                    StartCoroutine(Count_Coroutine());
                    StartCoroutine(CountDown_Coroutine());
                }
            }
            else if (currentGameState == GameState.Go)
            {
                if (!isGoCoroutineRunning)
                {
                    StartCoroutine(Go_Coroutine());
                }
            }
            previousGameState = currentGameState;
        }
    }

    IEnumerator Count_Coroutine()
    {
        isCountCoroutineRunning = true;
        CountText.gameObject.SetActive(true);
        for (int i = 5; i > 0; i--)
        {
            CountText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        CountText.gameObject.SetActive(false);
        isCountCoroutineRunning = false;
        yield break;
    }

    IEnumerator CountDown_Coroutine()
    {
        isCountDownCoroutineRunning = true;
        for (int i = 0; i < RoadingImages.Length; i++)
        {
            RoadingImages[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            RoadingImages[i].gameObject.SetActive(false);
        }
        _isGo = true;
        isCountDownCoroutineRunning = false;
        yield break;
    }

    IEnumerator Go_Coroutine()
    {
        isGoCoroutineRunning = true;
        if (_isGo == true)
        {
            Go.SetActive(true);
        }
        yield return new WaitForSeconds(2);
        _isGo = false;
        Go.SetActive(false);
        isGoCoroutineRunning = false;
        yield break;
    }
}
