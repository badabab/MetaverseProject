using Photon.Pun;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Ready,
    Loading,
    Go,
    Over,
}

public class FallGuysManager : MonoBehaviourPunCallbacks
{
    public static FallGuysManager Instance { get; private set; }

    private int _countDown = 3;
    private bool isCountingDown = false;
    private bool isGameOver = false;
    private bool isFirstPlayerDetected = false;
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
                if (!isGameOver)
                {
                    isGameOver = true;
                }
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
        if (photonView.IsMine && Input.GetKeyDown(KeyCode.R))
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
    private System.Collections.IEnumerator EndGame()
    {
        yield return new WaitForSeconds(10);
        // SceneManager.LoadScene("FallGuysWinScene");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_currentGameState == GameState.Go && !isFirstPlayerDetected)
        {
            if (other.CompareTag("Player"))
            {
                PhotonView playerPhotonView = other.GetComponentInParent<PhotonView>();
                if (playerPhotonView != null)
                {
                    isFirstPlayerDetected = true;
                    SetGameState(GameState.Over);
                    Debug.Log($"{playerPhotonView.Owner.NickName} reached the end first!");
                    StartCoroutine(EndGame());
                }
            }
        }
    }

    // 모든 플레이어가 준비되면 랜덤 시작위치 4군데로 랜덤이동
    // -> 시작위치(4군데) 설정 아직 안함
    // 카운트다운 후 게임 시작
    // 플레이어가 End3에 도착하면 GameState.Over

    // 플레이어 하나라도 도착하면 게임 끝낼건지?
    // 모든 플레이어 들어올때까지 관전모드(?) 같은 거 할 지 정해야 됨
}
