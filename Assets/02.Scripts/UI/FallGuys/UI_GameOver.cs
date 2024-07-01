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

    private void Start()
    {
        Instance = this;
        GameoverUI.SetActive(false);
        ResultUI.SetActive(false);
        Win.SetActive(false);
        Lose.SetActive(false);
    }
    /*private void Update()
    {
        if (FallGuysManager.Instance._currentGameState == GameState.Over)
        {
            if (!_isOnce) 
            {
                StartCoroutine(ShowPopUp(GameoverUI));
            }
        }
    }*/
    public void CheckFirst()
    {
        GameoverUI.SetActive(true);
        ResultUI.SetActive(true);
        SoundManager.instance.PlaySfx(SoundManager.Sfx.UI_GameOver);
        if (!_isOnce) 
        {           
            Debug.Log("이겼다!");
            Lose.SetActive(false);
            StartCoroutine(ShowPopUp(Win));
            SoundManager.instance.StopSfx(SoundManager.Sfx.UI_GameOver);
            SoundManager.instance.PlaySfx(SoundManager.Sfx.UI_WinVictory);
        }
    }
    public void CheckLast()
    {
        GameoverUI.SetActive(true);
        ResultUI.SetActive(true);
        SoundManager.instance.PlaySfx(SoundManager.Sfx.UI_GameOver);
        if (!_isOnce)
        {
            Debug.Log("졌다~");
            Win.SetActive(false);
            StartCoroutine(ShowPopUp(Lose));
            SoundManager.instance.StopSfx(SoundManager.Sfx.UI_GameOver);
            SoundManager.instance.PlaySfx(SoundManager.Sfx.UI_Lose);
        }      
    }
    
    public IEnumerator ShowPopUp(GameObject gameObject)
    {
        //_isOnce = true;
        ResultUI.SetActive(true);
        yield return new WaitForSeconds(2);
        
        ResultUI.SetActive(false);
        gameObject.SetActive(true);
        yield return new WaitForSeconds(_showTime);
        
        //gameObject.SetActive(false);
        GameoverUI.SetActive(false);
    }
}