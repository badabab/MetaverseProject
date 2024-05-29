using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_ArticeList : MonoBehaviour
{
    public static UI_ArticeList Instance { get; private set; }

    public List<UI_Artice> UIArticles;
    public GameObject WriteObject;
    public GameObject MenuObject;



    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        Refresh();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Refresh();
    }

    // 새로고침
    public void Refresh()
    {
        // 1. Article매니저로부터 Article들을 가져온다.
        List<Article> articles = ArticleManager.Instance.Articles;

        // 2. 모든 UI_Article을 끈다.
        foreach (UI_Artice uiArticle in UIArticles)
        {
            uiArticle.gameObject.SetActive(false);
        }

        for (int i = 0; i < articles.Count; i++)
        {
            // 3. 가져온 Article 개수만큼 UI_Article을 킨다.
            UIArticles[i].gameObject.SetActive(true);
            // 4. 각 UI_Article의 내용을 Article로 초기화(Init)한다.
            UIArticles[i].Init(articles[i]);
        }
    }

    public void OnClickOutButton()
    {
        MenuObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnClickWritingButton()
    {
        WriteObject.SetActive(true);
        gameObject.SetActive(false);
    }

}