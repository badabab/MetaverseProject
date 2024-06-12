using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverBall : MonoBehaviour
{
    public GameObject GameOverUI;
    public GameObject GameOver_testUI;

    private bool playerCollided = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerCollided)
        {
            playerCollided = true;
            Time.timeScale = 0f;
            StartCoroutine(GameOverSequence(other.gameObject));
        }
    }
    private IEnumerator GameOverSequence(GameObject Player)
    {
        GameOverUI.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        GameOver_testUI.SetActive(false);
        yield return new WaitForSecondsRealtime(3f);
        PhotonView photonView = Player.GetComponent<PhotonView>();
        Animator animator = photonView.GetComponent<Animator>();
        if (photonView != null)
        {
            animator.SetBool("Win", true);
            PersonalManager.Instance.CoinUpdate(photonView.Owner.NickName, 100);
            PhotonNetwork.LoadLevel("TowerClimbWinScene");
            PhotonNetwork.LeaveRoom();
        }
        Time.timeScale = 1f;
    }
}