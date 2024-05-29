using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ArticleGetOut : MonoBehaviour
{


    public void OnClickYesButton()
    {
        // 게임종료
    }
    public void OnClickNoButton()
    {
        UI_ArticleMenu.Instanse.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
