using Simplex;
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

    private void Awake()
    {
        PersonalManager.Instance.Init();
        UpdateNoticeTexts();
    }
    private void UpdateNoticeTexts()
    {
        List<Notice> notices = PersonalManager.Instance.GetAllNotices();

        for (int i = 0; i < NoticeText.Count; i++)
        {
            if (i < notices.Count)
            {
                NoticeText[i].text = $"{notices[i].Title}\n{notices[i].Content}";
                NoticeText[i].gameObject.SetActive(true);
            }
            else
            {
                NoticeText[i].gameObject.SetActive(false);
            }
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }


    public void OnClickNoticeButton(int index)
    {
        for (int i = 0; i < NoticeText.Count; i++)
        {
            NoticeText[i].gameObject.SetActive(false);
        }

        if (index >= 0 && index < NoticeText.Count)
        {
            NoticeText[index].gameObject.SetActive(true);
        }

        for (int i = 0; i < NoticeButton.Count; i++)
        {
            ColorBlock colorBlock = NoticeButton[i].colors;
            if (i == index)
            {
                colorBlock.normalColor = new Color(1.5f, 1.5f, 1.5f); // RGB 255,255,255
            }
            else
            {
                colorBlock.normalColor = new Color(0.78f, 0.78f, 0.78f); // RGB 200,200,200
            }
            NoticeButton[i].colors = colorBlock;
        }
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

    private void Start()
    {
        for (int i = 0; i < NoticeButton.Count; i++)
        {
            int index = i; 
            NoticeButton[i].onClick.AddListener(() => OnClickNoticeButton(index));
        }
    }

}
