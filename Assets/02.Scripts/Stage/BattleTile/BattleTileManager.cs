using Photon.Pun;
using System.Linq;
using UnityEngine;

public class BattleTileManager : MonoBehaviourPunCallbacks
{
    public static BattleTileManager Instance { get; private set; }

    private int _countDown = 3;

    public GameState _currentGameState = GameState.Ready;

    public Collider[] ColliderList;
    public Transform[] spawnPoints;

    public Transform EndPosition;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        switch (_currentGameState)
        {
            case GameState.Ready:
                ReadyPlayer();
                if (AreAllPlayersReady())
                {
                    SetGameState(GameState.Loading);
                }
                break;

            case GameState.Loading:
                StartCoroutine(StartCountDown());
                for (int i = 0; ColliderList.Length < i; i++)
                {
                    Collider col = ColliderList[i];
                    col.gameObject.SetActive(false);
                }
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
    }
    void ReadyPlayer()
    {
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
            {
                { "IsReady", true }
            };
            Debug.Log("레디 버튼 누름");
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
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

    void PlayerReadySpawner()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Photon.Realtime.Player[] players = PhotonNetwork.PlayerList.ToArray();
            for (int i = 0; i < players.Length; i++)
            {
                Vector3 spawnPosition = spawnPoints[i % spawnPoints.Length].position;
                photonView.RPC("MovePlayerToSpawn", players[i], spawnPosition);
                Debug.Log("플레이어 스폰 위치로 이동");
            }
        }
    }

    [PunRPC]
    void MovePlayerToSpawn(Vector3 position)
    {
        if (photonView.IsMine)
        {
            transform.position = position;
        }
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
}
