using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_NoticeList : MonoBehaviour
{
    public List<Text> NoticeText;
    public List<Button> NoticeButton;
    public Button OutButton;
    public Button YesButton;
    public Button NoButton;
    public GameObject NoticeBackground;

    public static UI_Notice Instance { get; private set; }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void OnClickNoticeButton()
    {

    }

    public void OnClickOutButton()
    {
        if (NoticeBackground != null)
        {
            NoticeBackground.SetActive(false);
        }
    }

    public void OnClickYesButton()
    {
        gameObject.SetActive(true);
    }

    public void OnClickNoButton()
    {
        gameObject.SetActive(false);
    }
}
