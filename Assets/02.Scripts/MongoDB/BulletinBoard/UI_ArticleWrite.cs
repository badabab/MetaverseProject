using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ArticleWrite : MonoBehaviour
{
    public UI_ArticeList ArticleListUI;
    public InputField ContentInputFieldUI;

    public void OnClickYesButton()
    {
        ArticleListUI.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    public void OnClickNoButton()
    {
        ArticleListUI.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
