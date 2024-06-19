using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class GameOverBall : MonoBehaviourPunCallbacks
{
    public GameObject GameOverUI;
    public GameObject GameOver_textUI;

    private bool playerCollided = false;

    private string _firstPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Hashtable firstPlayerName = new Hashtable { { "FirstPlayerName", _firstPlayer } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(firstPlayerName);
        }
        if (other.CompareTag("Player"))
        {
            if (!playerCollided)
            {
                playerCollided = true;
                Time.timeScale = 0f;
                StartCoroutine(GameOverSequence(other.gameObject));
                _firstPlayer = other.name;
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
        GameOverUI.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        GameOver_textUI.SetActive(false);
        yield return new WaitForSecondsRealtime(3f);
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