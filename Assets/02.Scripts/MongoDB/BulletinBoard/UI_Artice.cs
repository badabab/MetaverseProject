using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Artice : MonoBehaviour
{
    private static Dictionary<string, Texture> _cache = new Dictionary<string, Texture>();

    public Text NameTextUI;        // 글쓴이
    public Text ContentTextUI;     // 글 내용

    private Article _article;

    public void Init(in Article article)
    {
        _article = article;

        NameTextUI.text = article.Name;
        ContentTextUI.text = article.Content;
    }
}
