using Photon.Pun;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class VillageTutorial : MonoBehaviourPunCallbacks
{
    public PlayableDirector TimelineMaker;
    public GameObject SkipButton;

    private void Start()
    {
        TimelineMaker.Play();
        TimelineMaker.stopped += OnPlayableDirectorStopped;
    }
    public override void OnJoinedRoom()
    {
        
    }

    private void OnPlayableDirectorStopped(PlayableDirector aDirector)
    {
        if (TimelineMaker == aDirector)
        {
            LoadVillageScene(); // 타임라인이 끝나면 빌리지 씬으로 이동
        }
    }
    public void OnClickSkipButton()
    {
        LoadVillageScene(); // 스킵 버튼을 누르면 바로 빌리지 씬으로 이동
    }

    private void LoadVillageScene()
    {
        PhotonNetwork.LoadLevel("VillageScene");
    }
}
