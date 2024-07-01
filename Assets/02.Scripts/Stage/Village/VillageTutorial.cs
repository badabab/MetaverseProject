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

    public GameObject HelloPlayer;

    public GameObject TutorialBackground;
    public GameObject TutorialA;
    public GameObject TutorialB;

    private void Start()
    {
        SkipButton.SetActive(false);
        TutorialBackground.SetActive(false);
        StartCoroutine(tutorial_coroutine());
        StartCoroutine(WalkandHello());
        TimelineMaker.Play();
        StartCoroutine(Show_Coroutine());
        TimelineMaker.stopped += OnPlayableDirectorStopped;
        SoundManager.instance.PlayBgm(SoundManager.Bgm.VillageSceneTutorials);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SoundManager.instance.PlaySfx(SoundManager.Sfx.UI_LobbyButtonQTutorialsButton);
            if (isLeavingRoom)
            {
                return;
            }
            LoadVillageScene();
        }
    }
    IEnumerator tutorial_coroutine()
    {
        yield return new WaitForSeconds(1);
        TutorialBackground.SetActive(true);
        TutorialB.SetActive(false);
        TutorialA.SetActive(true);
        yield return new WaitForSeconds(5);
        TutorialA.SetActive(false);
        TutorialB.SetActive(true);
    }
    IEnumerator WalkandHello()
    {
        yield return new WaitForSeconds(2);
        HelloPlayer.GetComponent<Animator>().SetBool("Hello", true);
    }
    IEnumerator Show_Coroutine()
    {
        yield return new WaitForSeconds(5);
        SkipButton.SetActive(true);
    }

    private void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if(isLeavingRoom)
        {
            return;
        }

        if (TimelineMaker == aDirector)
        {
            LoadVillageScene(); // 타임라인이 끝나면 빌리지 씬으로 이동
        }
    }

    private void LoadVillageScene()
    {

        isLeavingRoom = true;

        RoomOptions roomOptions = new RoomOptions
        {
            MaxPlayers = 20,
            IsVisible = true,
            IsOpen = true,
            EmptyRoomTtl = 1000 * 20,
        };

        PhotonNetwork.LoadLevel("LoadingScene");
        PhotonNetwork.JoinOrCreateRoom(RoomID, roomOptions, TypedLobby.Default);
        Debug.Log($"{RoomID}");

      /*  if (!isLeavingRoom)
        {
            Debug.Log($"방 입장 성공! : ({PhotonNetwork.CurrentRoom.Name})");
            PhotonNetwork.IsMessageQueueRunning = false; // 메시지 큐 일시 정지
            PhotonNetwork.LoadLevel("VillageScene"); // PhotonNetwork를 통해 씬 로드
            PhotonNetwork.IsMessageQueueRunning = true; // 메시지 큐 재개
            isLeavingRoom = true;
        }*/

    }
}
