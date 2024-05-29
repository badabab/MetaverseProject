using UnityEngine;
using UnityEngine.UI;

public class UI_Article : MonoBehaviour
{
    public Text NameTextUI;        // 글쓴이
    public Text ContentTextUI;     // 글 내용

    private Article _article;

    public static UI_Article Instance { get; private set; }

    public void Init(Article article)
    {
        _article = article;
        NameTextUI.text = article.Name;
        ContentTextUI.text = article.Content;
    }
}
