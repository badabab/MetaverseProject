using Photon.Pun;
using System.Linq;
using UnityEngine;

public enum GameState
{
    Ready,
    Go,
    Over,
}

public class FallGuysManager : MonoBehaviourPunCallbacks
{
    public static FallGuysManager Instance { get; private set; }

    public GameState _currentGameState = GameState.Ready;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (AreAllPlayersReady() && _currentGameState == GameState.Ready)
        {
            SetGameState(GameState.Go);
        }
    }

    void SetGameState(GameState newState)
    {
        _currentGameState = newState;
        Debug.Log($"Game state changed to: {_currentGameState}");
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
        return true; // 모든 플레이어가 준비됨
    }


    // 모든 플레이어가 준비되면 랜덤 시작위치 4군데로 랜덤이동
    // -> 시작위치(4군데) 설정 아직 안함
    // 카운트다운 후 게임 시작
    // 플레이어가 End3에 도착하면 GameState.Over

    // 플레이어 하나라도 도착하면 게임 끝낼건지?
    // 모든 플레이어 들어올때까지 관전모드(?) 같은 거 할 지 정해야 됨
}
