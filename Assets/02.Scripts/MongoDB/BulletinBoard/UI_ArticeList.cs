using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem;

public class UI_ArticeList : MonoBehaviour
{
    public static UI_ArticeList Instance { get; private set; }
    public GameObject Write_UI;
    public List<UI_Article> UIArticles;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
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
        Debug.Log("Refresh 함수 호출됨");

        // 1. Article매니저로부터 Article들을 가져온다.
        List<Article> articles = ArticleManager.Instance.Articles;
        Debug.Log($"총 {articles.Count}개의 아티클이 있습니다.");

        // 2. 모든 UI_Article을 끈다.
        foreach (var uiArticle in UIArticles)
        {
            uiArticle.gameObject.SetActive(false);
        }

        // 3. 가져온 Article 개수만큼 UI_Article을 킨다.
        for (int i = 0; i < articles.Count; i++)
        {
                Debug.Log($"{articles.Count}");
                UIArticles[i].gameObject.SetActive(true);
                // 4. 각 UI_Article의 내용을 Article로 초기화(Init)한다.
                UIArticles[i].Init(articles[i]);
        }
    }

    public void OnClickOutButton()
    {
        // UI_ArticleMenu.Instanse.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnClickWritingButton()
    {
        Write_UI.SetActive(true);
        gameObject.SetActive(false);
    }

}