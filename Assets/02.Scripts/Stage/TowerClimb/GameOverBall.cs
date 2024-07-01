using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameOverBall : MonoBehaviourPunCallbacks
{
    public GameObject GameEndUI;
    public Image GameOver;
    public Image Victory;

    //private bool playerCollided = false;
    private string _firstPlayer;

    private void Start()
    {
        GameEndUI.SetActive(false);
        GameOver.gameObject.SetActive(false);
        Victory.gameObject.SetActive(false);

        SoundManager.instance.PlayBgm(SoundManager.Bgm.TowerClimbScene);
    }

    private void OnTriggerEnter(Collider other)
    {
        PhotonView playerPhotonView = other.GetComponentInParent<PhotonView>();
        if (other.CompareTag("Player") && playerPhotonView.IsMine)
        {
            _firstPlayer = playerPhotonView.Owner.NickName;
            Debug.Log($"{_firstPlayer} reached the end first!");
            PersonalManager.Instance.CoinUpdate(playerPhotonView.Owner.NickName, 100);

            Hashtable firstPlayerName = new Hashtable { { "FirstPlayerName", _firstPlayer } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(firstPlayerName);
            Debug.Log($"{firstPlayerName} 저장");
            
            //Time.timeScale = 0f;
            StartCoroutine(GameOverSequence(other.gameObject));
        }
    }

    private IEnumerator GameOverSequence(GameObject Player)
    {
        GameEndUI.SetActive(true);
        GameOver.gameObject.SetActive(true);
        SoundManager.instance.StopBgm();
        SoundManager.instance.PlaySfx(SoundManager.Sfx.UI_GameOver);
        yield return new WaitForSecondsRealtime(2f);
        SoundManager.instance.StopSfx(SoundManager.Sfx.UI_GameOver);

        GameOver.gameObject.SetActive(false);
        Victory.gameObject.SetActive(true);
        SoundManager.instance.PlaySfx(SoundManager.Sfx.UI_WinVictory);
        yield return new WaitForSecondsRealtime(3f);
        GameEndUI.SetActive(false);

        PhotonView photonView = Player.GetComponent<PhotonView>();
        Animator animator = photonView.GetComponent<Animator>();
        if (photonView != null)
        {
            animator.SetBool("Win", true);
            //PersonalManager.Instance.CoinUpdate(photonView.Owner.NickName, 100);
            //PhotonNetwork.LoadLevel("TowerClimbWinScene");
            //PhotonNetwork.LeaveRoom();
            StartCoroutine(ShowVictoryAndLoadScene());
        }
        //Time.timeScale = 1f;
    }
    private System.Collections.IEnumerator ShowVictoryAndLoadScene()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.LoadLevel("TowerClimbWinScene");
    }
}