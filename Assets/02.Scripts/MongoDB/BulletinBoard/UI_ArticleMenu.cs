using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_ArticleMenu : MonoBehaviour
{
    public UI_ArticeList ArticleListUI;

    public UI_ArticleMenu ArticleMenu;

    public UI_ArticleGetOut ArticleGetOut;

    public GameObject BulletinBackground;
    public void OnClickArticleListButton()
    {
        ArticleListUI.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnClickReTrunButton()
    {
        BulletinBackground.SetActive(false);
    }

    public void OnClickGetOutButton()
    {
        ArticleGetOut.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
