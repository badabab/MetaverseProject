using Photon.Pun;
using System.Linq;
using UnityEngine;

public class BattleTileManager : MonoBehaviourPunCallbacks
{
    public static BattleTileManager Instance { get; private set; }

    private int _countDown = 3;
    private int _gameDuration = 120; // 2분 = 120초

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
                StartCoroutine(StartCountDown());
                break;

            case GameState.Go:
                // 게임이 진행 중일 때의 로직을 추가하세요.
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
            StartCoroutine(GameTimer());
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

    private System.Collections.IEnumerator StartCountDown()
    {
        while (_countDown > 0)
        {
            Debug.Log($"CountDown: {_countDown}");
            yield return new WaitForSeconds(1);
            _countDown--;
        }
        SetGameState(GameState.Go);
    }

    private System.Collections.IEnumerator GameTimer()
    {
        int timeRemaining = _gameDuration;
        while (timeRemaining > 0)
        {
            //Debug.Log($"Game Time Remaining: {timeRemaining} seconds");
            yield return new WaitForSeconds(1);
            timeRemaining--;
        }
        SetGameState(GameState.Over);
    }
}
