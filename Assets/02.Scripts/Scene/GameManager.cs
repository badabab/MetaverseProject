using UnityEngine;
using System.Collections.Generic;
using System;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager Instance;

    private Dictionary<string, Personal> playerData = new Dictionary<string, Personal>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    


    public void GameOver()
    {
        if ( photonView.IsMine)
        {
            // 빌드 후 실행 했을 경우 종료하는 방법
            Application.Quit();
#if UNITY_EDITOR
            // 유니티 에디터에서 실행했을 경우 종료하는 방법
            UnityEditor.EditorApplication.isPlaying = false;
#endif

        }
    }

}
