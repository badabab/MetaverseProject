using Photon.Pun;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleTileManager : MonoBehaviourPunCallbacks
{
    public static BattleTileManager Instance { get; private set; }

    private float _gameDuration = 90f; // 90초 = 1분30초
    public float TimeRemaining;

    private int _countDown = 5;
    private int _countEnd = 5;
    private bool _isGameOver = false;
    private bool _isStartCoroutine = false;

    public GameState CurrentGameState = GameState.Ready;

    private void Awake()
    {
        Instance = this;
        TimeRemaining = (int)_gameDuration; // 게임 시작 시 타이머 초기화
    }

    private void Update()
    {
        switch (CurrentGameState)
        {
            case GameState.Ready:
                if (AreAllPlayersReady()) // PhotonNetwork.PlayerList.Length == 1
                {
                    SetGameState(GameState.Loading);
                }
                break;

            case GameState.Loading:
                if (!_isStartCoroutine)
                {
                    StartCoroutine(StartCountDown());
                    _isStartCoroutine = true;
                }
                break;

            case GameState.Go:
                UpdateGameTimer();
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
            if (player.CustomProperties.TryGetValue("IsReady_BattleTile", out isReadyObj))
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

        // 몇 초 동안 대기합니다 (카운트다운).
        while (_countEnd > 0)
        {
            Debug.Log($"CountDown: {_countEnd}");
            yield return new WaitForSeconds(1);
            _countEnd--;
        }

        // VillageScene 씬으로 전환합니다.
        //SceneManager.LoadScene("VillageScene");
        PhotonNetwork.LoadLevel("BattleTileWinScene");
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
            }
        }
    }
}