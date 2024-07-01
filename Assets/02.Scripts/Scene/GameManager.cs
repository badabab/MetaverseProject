using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using System.Linq;
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
    private TPSCamera tpsCamera;
    private GameObject _cameraRoot;
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
        FindLocalPlayer();
        tpsCamera = FindObjectOfType<TPSCamera>();
    }

    void FindLocalPlayer()
    {
        if (_localPlayerController == null)
        {
            _localPlayerController = FindObjectsOfType<PlayerOptionAbility>()
                .FirstOrDefault(player => player.GetComponent<PhotonView>().IsMine);

            if (_localPlayerController != null)
            {
                Debug.Log($"로컬 플레이어 찾음: {_localPlayerController.name}");
                _cameraRoot = FindCameraRoot(_localPlayerController.transform);
            }
            else
            {
                Debug.LogError("로컬 플레이어를 찾지 못했습니다.");
            }
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"{newPlayer.NickName}님이 입장하였습니다.");
        FindLocalPlayer();
    }
    GameObject FindCameraRoot(Transform playerTransform)
    {
        Transform cameraRootTransform = playerTransform.Find("CameraRoot");

        if (cameraRootTransform != null)
        {
            Debug.Log($"CameraRoot 오브젝트 찾음: {cameraRootTransform.name}");
            return cameraRootTransform.gameObject;
        }
        else
        {
            Debug.LogError("CameraRoot 오브젝트를 찾지 못했습니다.");
            return null;
        }
    }
    public void Pause()
    {
        if (_localPlayerController != null)
        {
            _localPlayerController.Pause();
            if (tpsCamera != null)
            {
                tpsCamera.Pause();
            }
            if (_cameraRoot != null)
            {
                _cameraRoot.SetActive(false);
            }
        }
    }

    public void Continue()
    {
        if (_localPlayerController != null)
        {
            _localPlayerController.Continue();
            if (tpsCamera != null)
            {
                tpsCamera.Continue();
            }
            if (_cameraRoot != null)
            {
                _cameraRoot.SetActive(true);
            }
        }
    }

    public void BackToVillage()
    {
        FindLocalPlayer();

        if (SceneManager.GetActiveScene().name != "VillageScene")
        {
            if (_localPlayerController != null && _localPlayerController.photonView.IsMine)
            {
                //PhotonNetwork.LeaveRoom();
                PhotonManager.Instance.LeaveAndLoadRoom("Village");
            }
            else
            {

                Debug.LogError("권한 없는 플레이어가 방 전환을 시도하였습니다.");
            }
        }
        else
        {
            return;
        }
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

        // 빌드 후 실행됐을 경우 종료하는 방법
        Application.Quit();

#if UNITY_EDITOR
        // 유니티 에디터에서 실행했을 경우 종료하는 방법
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
