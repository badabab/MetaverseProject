using Photon.Pun;
using System.Collections;
using System.Linq;
using UnityEngine;

public class BattleTileManager : MonoBehaviourPunCallbacks
{
    public static BattleTileManager Instance { get; private set; }

    private int _countDown = 3;
    private float _gameDuration = 120f; // 2분 = 120초
    public float TimeRemaining;

    public GameState State { get; private set; } = GameState.Ready;

    private void Awake()
    {
        Instance = this;
        StartCoroutine(Start_Coroutine());
    }

    public void Refresh()
    {
        switch (State)
        {
            case GameState.Ready:
                if (AreAllPlayersReady())
                {
                    TimeRemaining = (int)_gameDuration; // 게임 시작 시 타이머 초기화
                    State = GameState.Loading;
                }
                break;

            case GameState.Loading:
                break;

            case GameState.Go:
                Debug.Log("게임 시작");
                break;

            case GameState.Over:
                Debug.Log("게임 종료");
                break;
        }
    }

    private void Update()
    {
        if (State == GameState.Go)
        {
            UpdateGameTimer();
        }
    }

    private bool AreAllPlayersReady()
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
    private IEnumerator Start_Coroutine()
    {
        State = GameState.Ready;
        Refresh();

        yield return new WaitForSeconds(2f);
        State = GameState.Go;
        Refresh();
    }


    void UpdateGameTimer()
    {
        if (TimeRemaining > 0)
        {
            TimeRemaining -= Time.deltaTime;

            if (TimeRemaining <= 0)
            {
                TimeRemaining = 0;
                State = GameState.Over;
                Refresh();
            }
        }
    }
}