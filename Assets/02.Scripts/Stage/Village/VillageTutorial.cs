using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class VillageTutorial : MonoBehaviourPunCallbacks
{
    public PlayableDirector TimelineMaker;
    public GameObject SkipButton;

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
    

    private void LoadVillageScene()
    {
        PhotonNetwork.LoadLevel("VillageScene");
    }
}
