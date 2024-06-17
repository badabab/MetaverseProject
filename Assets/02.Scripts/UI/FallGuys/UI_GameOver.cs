using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameOver : MonoBehaviourPunCallbacks
{
    public GameObject GameoverUI;
    public GameObject ResultUI;
    public GameObject Win;
    public GameObject Lose;

    private int _showTime = 3;
    private bool _isOnce;
    public static UI_GameOver Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        GameoverUI.gameObject.SetActive(false);
        ResultUI.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (FallGuysManager.Instance._currentGameState == GameState.Over)
        {
            if (!_isOnce) 
            {
                StartCoroutine(ShowPopUp(GameoverUI));
            }
        }
    }
    public void CheckFirst()
    {
        ResultUI.gameObject.SetActive(true);
        if (!_isOnce) 
        {
            Debug.Log("이겼다!");
            StartCoroutine(ShowPopUp(Win));
        }
        Lose.SetActive(false);
    }
    public void CheckLast()
    {
        ResultUI.gameObject.SetActive(true);
        if (!_isOnce)
        {
            Debug.Log("졌다~");
            StartCoroutine(ShowPopUp(Lose));
        }
        Win.SetActive(false);
    }
    
    public IEnumerator ShowPopUp(GameObject gameObject)
    {
        _isOnce = true;
        gameObject.SetActive(true);
        while (_showTime > 0)
        {
            yield return new WaitForSeconds(1);
            _showTime--;
        }
        gameObject.SetActive(false);
    }
}