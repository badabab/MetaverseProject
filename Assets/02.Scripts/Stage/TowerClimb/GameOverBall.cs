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

    private string firstPlayerNickName;

    private void Start()
    {
        GameEndUI.SetActive(false);
        GameOver.gameObject.SetActive(false);
        Victory.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        PhotonView playerPhotonView = other.GetComponentInParent<PhotonView>();
        if (other.CompareTag("Player") && playerPhotonView.IsMine)
        {
            firstPlayerNickName = playerPhotonView.Owner.NickName;
            Debug.Log($"{firstPlayerNickName} reached the end first!");
            PersonalManager.Instance.CoinUpdate(photonView.Owner.NickName, 100);

            if (PhotonNetwork.IsMasterClient)
            {
                Hashtable firstPlayerName = new Hashtable { { "FirstPlayerName", firstPlayerNickName } };
                PhotonNetwork.CurrentRoom.SetCustomProperties(firstPlayerName);
            }
        }
      
        if (other.CompareTag("Player"))
        {
            if (!playerCollided)
            {
                playerCollided = true;
                Time.timeScale = 0f;
                StartCoroutine(GameOverSequence(other.gameObject));
                firstPlayerNickName = other.name;
            }
            else
            {
                other.GetComponent<Animator>().SetBool("Sad", true);
                PhotonNetwork.LoadLevel("VillageLoadScene");
                PhotonNetwork.LeaveRoom();
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
            PhotonNetwork.LoadLevel("TowerClimbWinScene");
            //PhotonNetwork.LeaveRoom();
        }
        Time.timeScale = 1f;
    }
}