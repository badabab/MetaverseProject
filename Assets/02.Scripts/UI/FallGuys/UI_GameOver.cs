using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameOver : MonoBehaviourPunCallbacks
{
    public GameObject GameoverUI;

    public GameObject Win;
    public GameObject Lose;

    private int _showTime = 3;
    public static UI_GameOver Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        GameoverUI.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (FallGuysManager.Instance._currentGameState == GameState.Over)
        {
            GameoverUI.SetActive(true);
        }
    }
    public void CheckFirst()
    {
       Win.SetActive(true);
       Lose.SetActive(false);
        Debug.Log("이겼다!");
        StartCoroutine(ShowPopUp(Win));
    }
    public void CheckLast()
    {
        Lose.SetActive(true);
        Win.SetActive(false);
        Debug.Log("졌다~");
        StartCoroutine(ShowPopUp(Lose));
    }
    
    public IEnumerator ShowPopUp(GameObject gameObject)
    {
        while (_showTime > 0)
        {
            yield return new WaitForSeconds(3);
            _showTime--;
        }
        gameObject.gameObject.SetActive(false);
    }
}