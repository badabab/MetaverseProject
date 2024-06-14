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
    private bool _isStart = false;
    private bool isGameOver = false;
    //private bool isFirstPlayerDetected = false;
    //private string firstPlayerId;

    public GameState _currentGameState = GameState.Ready;

    public Collider[] ColliderList;
    public Transform[] spawnPoints;

    public ParticleSystem ParticleSystem;
    public ParticleSystem ReadyParticle;
    public ParticleSystem WaterParticle;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        switch (_currentGameState)
    
        {
            case GameState.Ready:
                if (AreAllPlayersReady()) // 모든 플레이어가 레디인 경우
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
                    StartCoroutine(ShowVictoryAndLoadScene());
                }
                break;
        }
    }
    public void SetGameState(GameState newState)
    {
        _currentGameState = newState;
        Debug.Log($"Game state changed to: {_currentGameState}");
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
            ParticleSystem.gameObject.SetActive(false);
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
        if (!_isStart)
        {
            _isStart = true;
            for (int i = 0; i < _countDown + 1; i++)
            {
                yield return new WaitForSeconds(1);
                Debug.Log($"CountDown: {i}");
            }
            SetGameState(GameState.Go);
        }      
    }
    private System.Collections.IEnumerator ShowVictoryAndLoadScene()
    {
        while (_countEnd > 0)
        {
            Debug.Log($"CountDown: {_countEnd}");
            yield return new WaitForSeconds(1);
            _countEnd--;
        }
        //PhotonManager.Instance.NextRoomName = "Village";
        PhotonNetwork.LoadLevel("FallGuysWinScene");
        //PhotonNetwork.LeaveRoom();
    }
    public void SetPlayerReadyVFX(bool isReady, Vector3 position)
    {
        if (isReady)
        {
           ParticleSystem particle = Instantiate(ReadyParticle, position, Quaternion.identity);
           particle.transform.localScale *= 0.4f;
        }
    }
}
