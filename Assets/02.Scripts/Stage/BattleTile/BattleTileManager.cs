using Photon.Pun;
using System.Linq;
using UnityEngine;

public class BattleTileManager : MonoBehaviourPunCallbacks
{
    public static BattleTileManager Instance { get; private set; }

    private int _countDown = 3;
    private float _gameDuration = 120f; // 2분 = 120초
    public float TimeRemaining;

    public GameState _currentGameState = GameState.Ready;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        switch (_currentGameState)
        {
            case GameState.Ready:
                if (AreAllPlayersReady())
                {
                    SetGameState(GameState.Loading);
                }
                break;

            case GameState.Loading:
                StartCountDown();
                break;

            case GameState.Go:
                UpdateGameTimer();
                break;

            case GameState.Over:
                // 게임 오버 상태일 때의 로직을 추가하세요.
                break;
        }
    }

    void SetGameState(GameState newState)
    {
        _currentGameState = newState;
        Debug.Log($"Game state changed to: {_currentGameState}");

        if (_currentGameState == GameState.Go)
        {
            TimeRemaining = (int)_gameDuration; // 게임 시작 시 타이머 초기화
        }
    }

    bool AreAllPlayersReady()
    {
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList.ToArray(); // PlayerList를 배열로 복제

        foreach (Photon.Realtime.Player player in players)
        {
            object isReadyObj;
            if (player.CustomProperties.TryGetValue("IsReady", out isReadyObj))
            {
                if (!(bool)isReadyObj)
                {
                    return false; // 준비되지 않은 플레이어가 있음
                }
            }
        }
        Debug.Log("플레이어 모두 레디");
        return true; // 모든 플레이어가 준비됨
    }

    void StartCountDown()
    {
        _countDown--;
        Debug.Log($"CountDown: {_countDown}");
        if (_countDown <= 0)
        {
            SetGameState(GameState.Go);
        }
        else
        {
            Invoke("StartCountDown", 1f);
        }
    }

    void UpdateGameTimer()
    {
        if (TimeRemaining > 0)
        {
            TimeRemaining -= Time.deltaTime;
            //Debug.Log($"Game Time Remaining: {TimeRemaining} seconds");

            if (TimeRemaining <= 0)
            {
                TimeRemaining = 0;
                SetGameState(GameState.Over);
            }
        }
    }
}
