using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
public enum SceneType
{
    Villige,
    MiniGame1,
    MiniGame2,
    MiniGame3,
}

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    private Dictionary<string, Personal> playerData = new Dictionary<string, Personal>();
    private PlayerOptionAbility _localPlayerController;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // 로컬 플레이어 찾기
        //FindLocalPlayer();
    }

    void FindLocalPlayer()
    {
        if (_localPlayerController == null)
        {
            foreach (var player in FindObjectsOfType<PlayerOptionAbility>())
            {
                PhotonView photonView = player.GetComponent<PhotonView>();
                if (photonView != null && photonView.IsMine)
                {
                    _localPlayerController = player;
                    Debug.Log($"플레이어 찾음: {player.name}");
                    break;
                }
            }
            if (_localPlayerController == null)
            {
                Debug.LogError("Local player not found!");
            }
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName}님이 입장하였습니다.");
        FindLocalPlayer();
    }

    public void Pause()
    {
        if (_localPlayerController != null)
        {
            _localPlayerController.Pause();
        }
    }

    public void Continue()
    {
        if (_localPlayerController != null)
        {
            _localPlayerController.Continue();
        }
    }

    public void BackToVillage()
    {
        if (SceneManager.GetActiveScene().name == "VillageScene")
        {
            return;
        }

        PhotonManager.Instance.NextRoomName = "Village";
        PhotonNetwork.LeaveRoom();
        //SceneManager.LoadScene("VillageScene");
       // PhotonNetwork.LoadLevel("VillageScene");
    }


    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.LogFormat($"{otherPlayer.NickName}님이 방을 떠났습니다.");
    }

    public void GameOver()
    {
        if (_localPlayerController != null)
        {
            // Photon Network에서 방 나가기
            PhotonNetwork.LeaveRoom();
        }
    }

/*    public override void OnLeftRoom()
    {
        // 방을 나간 후에 애플리케이션 종료
        if (_localPlayerController != null)
        {
            // 빌드 후 실행했을 경우 종료하는 방법
            Application.Quit();
#if UNITY_EDITOR
            // 유니티 에디터에서 실행했을 경우 종료하는
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }*/
}
