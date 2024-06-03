using Photon.Pun;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
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

    private int _countDown = 5;
    private int _countEnd = 10;
    private bool isCountingDown = false;
    private bool isGameOver = false;
    private bool isFirstPlayerDetected = false;
    private string firstPlayerId;

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
                if (PhotonNetwork.PlayerList.Length == 1 || AreAllPlayersReady()) // 플레이어가 한 명이거나 모든 플레이어가 레디인 경우
                {
                    SetGameState(GameState.Loading);
                }
                break;


            case GameState.Loading:
                // PlayerReadySpawner();
                StartCoroutine(StartCountDown());
                ColliderState();
                break;

            case GameState.Go:
                // 게임이 진행 중일 때의 로직을 추가하세요.
                break;

            case GameState.Over:
                if (!isGameOver)
                {
                    isGameOver = true;
                    StartCoroutine(ShowVictoryAndLoadScene(PhotonNetwork.LocalPlayer.NickName));
                }
                break;
        }
    }
    public void SetGameState(GameState newState)
    {
        _currentGameState = newState;
        Debug.Log($"Game state changed to: {_currentGameState}");
    }
    void CountDown()
    {
        
    }
    void ColliderState()
    {
        if (_currentGameState == GameState.Ready)
        {
            return;
        }
        else
        {
            foreach (Collider col in ColliderList) 
            {
                col.isTrigger = true;
                col.gameObject.SetActive(false);
            }
        }
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
        yield return new WaitForSeconds(5);
        photonView.RPC("LoadVillageScene", RpcTarget.All);
    }

    [PunRPC]
    void LoadVillageScene()
    {
        SceneManager.LoadScene("VillageScene");
    }

    private System.Collections.IEnumerator ShowVictoryAndLoadScene(string winnerId)
    {

/*            GameObject winner = PhotonNetwork.PlayerList.FirstOrDefault(p => p.UserId == winnerId).TagObject as GameObject;
            if (winner != null)
            {
                GameManager.Instance.AddCoinsToWinner(winnerId, 100);
                Animator animator = winner.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.SetTrigger("Winning");
                }
            }
        */
        while (_countEnd > 0)
        {
            Debug.Log($"CountDown: {_countEnd}");
            yield return new WaitForSeconds(5);
            _countEnd--;
        }
        SceneManager.LoadScene("VillageScene");
    }


    // 플레이어 하나라도 도착하면 게임 끝낼건지?
    // 모든 플레이어 들어올때까지 관전모드(?) 같은 거 할 지 정해야 됨
}
