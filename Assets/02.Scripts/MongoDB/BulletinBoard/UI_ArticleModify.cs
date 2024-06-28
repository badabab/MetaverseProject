using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

public class UI_ArticleModify : MonoBehaviour
{

    public InputField InputFieldArticle;
    public static UI_ArticleModify Instance { get; private set; }
    private Article _article;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        this.gameObject.SetActive(false);
    }
    public void Show(in Article article)
    {
        _article = article;
        InputFieldArticle.text = _article.Content;
        this.gameObject.SetActive(true);
    }
    public void OnClickModifyX()
    {
        SoundManager.instance.PlaySfx(SoundManager.Sfx.UI_LobbyButtonQTutorialsButton);
        UI_ArticeList.Instance.Show();
        this.gameObject.SetActive(false);
    }
    public void OnClickModifyFinish()
    {
        SoundManager.instance.PlaySfx(SoundManager.Sfx.UI_LobbyButtonQTutorialsButton);
        _article.Content = InputFieldArticle.text;
        ArticleManager.Instance.Replace(_article);
        ArticleManager.Instance.FindAll();
        UI_ArticeList.Instance.Show();
        this.gameObject.SetActive(false);
    }
}
