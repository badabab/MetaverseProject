using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class VillageTutorial : MonoBehaviourPunCallbacks
{
    public PlayableDirector TimelineMaker;
    public GameObject SkipButton;
    private string RoomID = "Village";
    private bool isLeavingRoom = false;

    private void Start()
    {
        SkipButton.SetActive(false);
        TimelineMaker.Play();
        StartCoroutine(Show_Coroutine());
        TimelineMaker.stopped += OnPlayableDirectorStopped;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            LoadVillageScene();
        }
    }

    IEnumerator Show_Coroutine()
    {
        yield return new WaitForSeconds(5);
        SkipButton.SetActive(true);
    }

    private void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (TimelineMaker == aDirector)
        {
            LoadVillageScene(); // 타임라인이 끝나면 빌리지 씬으로 이동
        }
    }

    private void LoadVillageScene()
    {
        PhotonNetwork.LoadLevel("VillageScene");
        /*if (!isLeavingRoom)
        {
            isLeavingRoom = true;
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom(); // 방을 떠나고 OnLeftRoom 콜백에서 새로운 방에 참가
            }
            else
            {
                JoinOrCreateRoom(); // 방에 없으면 바로 새로운 방에 참가
            }
        }*/
    }

/*    public override void OnLeftRoom()
    {
        Debug.Log("Left room. Now joining or creating room: " + RoomID);
        JoinOrCreateRoom();
    }

    private void JoinOrCreateRoom()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            RoomOptions roomOptions = new RoomOptions
            {
                MaxPlayers = 20,
                IsVisible = true,
                IsOpen = true,
                EmptyRoomTtl = 1000 * 20,
            };
            PhotonNetwork.JoinOrCreateRoom(RoomID, roomOptions, TypedLobby.Default);
            Debug.Log($"{RoomID}");
            isLeavingRoom = false; // 방 참가 시도를 했으므로 플래그 리셋
        }
        else
        {
            Debug.LogWarning("PhotonNetwork is not ready.");
            isLeavingRoom = false; // 실패했으므로 플래그 리셋
        }
    }*/
}
