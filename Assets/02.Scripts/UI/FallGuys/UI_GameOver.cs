using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameOver : MonoBehaviourPunCallbacks
{
    public GameObject GameoverUI;

    public GameObject Win;
    public GameObject Lose;

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
    }
    public void CheckLast()
    {
        Lose.SetActive(true);
        Win.SetActive(false);
    }
}