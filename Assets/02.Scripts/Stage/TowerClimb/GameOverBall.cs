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

    private IEnumerator GameOverSequence(GameObject collidingPlayer)
    {
        GameOverUI.SetActive(true);

        yield return new WaitForSecondsRealtime(2f);

        GameOver_testUI.SetActive(false);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        yield return new WaitForSecondsRealtime(3f);

        SceneManager.LoadScene("TowerClimbWinScene");

        Time.timeScale = 1f;
    }
}