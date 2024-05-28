using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ArticleGetOut : MonoBehaviour
{
    public UI_ArticleMenu ArticleMenu;

    public void OnClickYesButton()
    {
        // 게임종료
    }
    public void OnClickNoButton()
    {
        ArticleMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
