using Photon.Pun;
using System.Collections;
using UnityEngine;

public class BattleTileManager : MonoBehaviourPunCallbacks
{
    public static BattleTileManager Instance {  get; private set; }

    public enum GameState { Ready, Go, Over }
    public GameState CurrentState = GameState.Ready;

    private int readyPlayers = 0;

    public int PlayerNumber;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        AssignPlayerNumber();
    }

    void AssignPlayerNumber()
    {
        PlayerNumber = PhotonNetwork.CurrentRoom.PlayerCount;
        PhotonNetwork.LocalPlayer.CustomProperties["PlayerNumber"] = PlayerNumber;
    }

    public void PlayerReady()
    {
        photonView.RPC("PlayerReadyRPC", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void PlayerReadyRPC()
    {
        readyPlayers++;
        if (readyPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            StartCoroutine(StartGameCountdown());
        }
    }

    IEnumerator StartGameCountdown()
    {
        yield return new WaitForSeconds(3);
        CurrentState = GameState.Go;
        photonView.RPC("ChangeGameStateRPC", RpcTarget.AllBuffered, GameState.Go);
        StartCoroutine(GameTimer());
    }

    [PunRPC]
    void ChangeGameStateRPC(GameState newState)
    {
        CurrentState = newState;
    }

    IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(120);
        CurrentState = GameState.Over;
        photonView.RPC("ChangeGameStateRPC", RpcTarget.AllBuffered, GameState.Over);
    }
}
