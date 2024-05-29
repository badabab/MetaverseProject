using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Article : MonoBehaviour
{
    public TextMeshProUGUI NameTextUI;   
    public TextMeshProUGUI ContentTextUI;    

    private Article _article;


    public static UI_Article Instance { get; private set; }

    public void Init(Article article)
    {
        _article = article;
        NameTextUI.text = article.Name;
        ContentTextUI.text = article.Content;
    }
    public void OnClickMenuButton()
    {
        UI_ArticleMenu.Instance.Show(_article);
    }
}
