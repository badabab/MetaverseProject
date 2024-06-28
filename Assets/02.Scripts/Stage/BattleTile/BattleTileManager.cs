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
    private bool _countdownSoundPlayed = false;

    public GameState CurrentGameState = GameState.Ready;

    private void Start()
    {
        SoundManager.instance.PlayBgm(SoundManager.Bgm.TimerWaiting);
        Instance = this;
        TimeRemaining = _gameDuration; // 게임 시작 시 타이머 초기화
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
                // 코루틴을 시작하기 위한 조건을 StartCoroutine에서 처리하기 때문에 여기서는 비워둡니다.
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
        HandleGameStateChange(newState);
    }

    private void HandleGameStateChange(GameState newState)
    {
        switch (newState)
        {
            case GameState.Loading:
                SoundManager.instance.StopBgm();
                if (!_isStartCoroutine)
                {
                    StartCoroutine(StartCountDown());
                    _isStartCoroutine = true;
                }
                break;

            case GameState.Go:
                SoundManager.instance.PlayBgm(SoundManager.Bgm.BattleTileScene);
                break;

            case GameState.Over:
                SoundManager.instance.StopSfx(SoundManager.Sfx.UI_Count);
                break;
        }
    }

    public bool AreAllPlayersReady()
    {
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList.ToArray();
        Debug.Log("Player count: " + players.Length);
        foreach (Photon.Realtime.Player player in players)
        {
            if (player.CustomProperties.TryGetValue("IsReady_BattleTile", out object isReadyObj))
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
            SoundManager.instance.PlaySfx(SoundManager.Sfx.CountDown);
            yield return new WaitForSeconds(1);
            Debug.Log($"CountDown: {i}");
        }
        SoundManager.instance.StopSfx(SoundManager.Sfx.CountDown);
        SoundManager.instance.PlaySfx(SoundManager.Sfx.Go);
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
        PhotonNetwork.LoadLevel("BattleTileWinScene");
    }

    void UpdateGameTimer()
    {
        if (TimeRemaining > 0)
        {
            TimeRemaining -= Time.deltaTime;
            if (TimeRemaining <= 6 && !_countdownSoundPlayed) 
            {
                SoundManager.instance.PlaySfx(SoundManager.Sfx.VillageInteractiveObjectWarningChicken7);
                _countdownSoundPlayed = true; // Set the flag to true
            }
            else if (TimeRemaining <= 0)
            {
                SoundManager.instance.StopBgm();
                SoundManager.instance.StopSfx(SoundManager.Sfx.VillageInteractiveObjectWarningChicken7);
                TimeRemaining = 0;
                SetGameState(GameState.Over);
            }
        }
    }
}