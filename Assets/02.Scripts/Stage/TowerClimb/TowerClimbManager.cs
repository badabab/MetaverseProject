using Photon.Pun;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public enum TowerClimbGameState
{
    Ready,
    Loading,
    Go,
    Over,
}

public class TowerClimbManager : MonoBehaviour
{
    public static TowerClimbManager Instance { get; private set; }

    private int _countDown = 3;
    private int _countEnd = 10;
    private bool isCountingDown = false;
    private bool isGameOver = false;
    public bool isFirstPlayerDetected = false;  // Public으로 변경하여 FinshLine에서 접근 가능하게 함
    public string firstPlayerId;

    public TowerClimbGameState _currentGameState = TowerClimbGameState.Ready;

    public Collider[] ColliderList;
    public Transform[] spawnPoints;

    public Transform EndPosition;

    private PhotonView photonView;

    private void Awake()
    {
        Instance = this;
        photonView = GetComponent<PhotonView>();  // PhotonView 초기화
    }

    void Update()
    {
        switch (_currentGameState)
        {
            case TowerClimbGameState.Ready:
                if (PhotonNetwork.PlayerList.Length == 1 || AreAllPlayersReady())
                {
                    SetGameState(TowerClimbGameState.Loading);
                }
                break;

            case TowerClimbGameState.Loading:
                StartCoroutine(StartCountDown());
                ColliderState();
                break;

            case TowerClimbGameState.Go:
                // 게임이 진행 중일 때의 로직을 추가하세요.
                break;

            case TowerClimbGameState.Over:
                if (!isGameOver)
                {
                    isGameOver = true;
                    StartCoroutine(ShowVictoryAndLoadScene(firstPlayerId));
                }
                break;
        }
    }

    public void SetGameState(TowerClimbGameState newState)
    {
        _currentGameState = newState;
        Debug.Log($"Game state changed to: {_currentGameState}");
    }

    void ColliderState()
    {
        if (_currentGameState == TowerClimbGameState.Ready)
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
                    return false;
                }
            }
            else
            {
                Debug.Log("플레이어 준비 상태가 없습니다: " + player.NickName);
                return false;
            }
        }
        Debug.Log("플레이어 모두 레디");
        return true;
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
        SetGameState(TowerClimbGameState.Go);
    }

    private System.Collections.IEnumerator EndGame()
    {
        yield return new WaitForSeconds(10);
        photonView.RPC("LoadVillageScene", RpcTarget.All);
    }

    [PunRPC]
    void LoadVillageScene()
    {
        SceneManager.LoadScene("VillageScene");
    }

    private System.Collections.IEnumerator ShowVictoryAndLoadScene(string winnerId)
    {
        /*
        GameObject winner = PhotonNetwork.PlayerList.FirstOrDefault(p => p.UserId == winnerId).TagObject as GameObject;
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
}
