using Photon.Pun;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleTileManager : MonoBehaviourPunCallbacks
{
    public static BattleTileManager Instance { get; private set; }

    private float _gameDuration = 120f; // 2분 = 120초
    public float TimeRemaining;

    private int _countDown = 5;
    private int _countEnd = 5;
    private bool _isGameOver = false;
    private bool _isStartCoroutine = false;
    private bool _isNameUI = false;

    public GameState CurrentGameState = GameState.Ready;
    public GameObject GameEndUI;
    public Image Gameover;
    public Image Lose;
    public Image Win;

    private void Awake()
    {
        Instance = this;
        TimeRemaining = (int)_gameDuration; // 게임 시작 시 타이머 초기화
    }

    private void Update()
    {
        if (CurrentGameState == GameState.Go)
        {
            UpdateGameTimer();
        }

        switch (CurrentGameState)
        {
            case GameState.Ready:
                if (AreAllPlayersReady()) // PhotonNetwork.PlayerList.Length == 1
                {
                    SetGameState(GameState.Loading);
                }
                break;

            case GameState.Loading:
                if (!_isNameUI)
                {
                    UI_BattleTile.Instance.RefreshUI();
                    _isNameUI = true;
                }
                if (!_isStartCoroutine)
                {
                    StartCoroutine(StartCountDown());
                    _isStartCoroutine = true;
                }
                break;

            case GameState.Go:
               
                break;

            case GameState.Over:
                if (!_isGameOver)
                {
                    _isGameOver = true;
                    StartCoroutine(ShowVictoryAndLoadScene());
                }
                break;
        }
    }

    public void SetGameState(GameState newState)
    {
        CurrentGameState = newState;
        Debug.Log($"Game state changed to: {CurrentGameState}");
    }

    public bool AreAllPlayersReady()
    {
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList.ToArray();
        Debug.Log("Player count: " + players.Length);
        foreach (Photon.Realtime.Player player in players)
        {
            object isReadyObj;
            if (player.CustomProperties.TryGetValue("IsReady", out isReadyObj))
            {
                if (!(bool)isReadyObj)
                {
                    Debug.Log("플레이어가 준비되지 않았습니다: " + player.NickName);
                    return false; // 준비되지 않은 플레이어가 있음
                }
            }
            else
            {
                Debug.Log("플레이어 준비 상태가 없습니다: " + player.NickName);
                return false; // 준비 상태 정보가 없음
            }
        }
        Debug.Log("플레이어 모두 레디");
        return true; // 모든 플레이어가 준비됨
    }

    private IEnumerator StartCountDown()
    {
        for (int i = 0; i < _countDown + 1; i++)
        {
            yield return new WaitForSeconds(1);
            Debug.Log($"CountDown: {i}");
        }
        SetGameState(GameState.Go);
    }

    private IEnumerator ShowVictoryAndLoadScene()
    {
        TileScore.Instance.DetermineWinner();

        GameEndUI.SetActive(true);
        Gameover.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        Gameover.gameObject.SetActive(false);

        // 이긴 사람만 Win, 나머지는 Lose
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("IsWinner") && (bool)PhotonNetwork.LocalPlayer.CustomProperties["IsWinner"])
        {
            Win.gameObject.SetActive(true);
        }
        else
        {
            Lose.gameObject.SetActive(true);
        }

        // 몇 초 동안 대기합니다 (카운트다운).
        while (_countEnd > 0)
        {
            Debug.Log($"CountDown: {_countEnd}");
            yield return new WaitForSeconds(1);
            _countEnd--;
        }

        // VillageScene 씬으로 전환합니다.
        SceneManager.LoadScene("VillageScene");
    }

    void UpdateGameTimer()
    {
        if (TimeRemaining > 0)
        {
            TimeRemaining -= Time.deltaTime;

            if (TimeRemaining <= 0)
            {
                TimeRemaining = 0;
                CurrentGameState = GameState.Over;
                Gameover.gameObject.SetActive(true);
            }
        }
    }
}