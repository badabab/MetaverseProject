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

    private bool playerCollided = false;
    private string _firstPlayer;

    private void Start()
    {
        GameEndUI.SetActive(false);
        GameOver.gameObject.SetActive(false);
        Victory.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!playerCollided)
            {
                playerCollided = true;
                _firstPlayer = other.name;
                if (PhotonNetwork.IsMasterClient)
                {
                    Hashtable firstPlayerName = new Hashtable { { "FirstPlayerName", _firstPlayer } };
                    PhotonNetwork.CurrentRoom.SetCustomProperties(firstPlayerName);
                }

                PhotonView photonView = other.GetComponent<PhotonView>();
                if (photonView.IsMine)
                {
                    Time.timeScale = 0f;
                    StartCoroutine(GameOverSequence(other.gameObject));
                }
            }
            else
            {
                PhotonView photonView = other.GetComponent<PhotonView>();
                if (photonView.IsMine)
                {
                    other.GetComponent<Animator>().SetBool("Sad", true);
                    PhotonNetwork.LoadLevel("VillageLoadScene");
                    PhotonNetwork.LeaveRoom();
                }
            }
        }
    }

    private IEnumerator GameOverSequence(GameObject Player)
    {
        GameEndUI.SetActive(true);
        GameOver.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);

        GameOver.gameObject.SetActive(false);
        Victory.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(3f);
        GameEndUI.SetActive(false);

        PhotonView photonView = Player.GetComponent<PhotonView>();
        Animator animator = photonView.GetComponent<Animator>();
        if (photonView != null)
        {
            animator.SetBool("Win", true);
            PersonalManager.Instance.CoinUpdate(photonView.Owner.NickName, 100);
            PhotonNetwork.LoadLevel("TowerClimbWinScene");
            //PhotonNetwork.LeaveRoom();
        }
        Time.timeScale = 1f;
    }
}