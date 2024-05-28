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
        Instance = this;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }



    public void OnClickNoticeButton(int index)
    {
        // NoticeText 리스트의 모든 텍스트를 비활성화
        for (int i = 0; i < NoticeText.Count; i++)
        {
            NoticeText[i].gameObject.SetActive(false);
        }

        // 클릭된 버튼의 인덱스에 해당하는 텍스트를 활성화
        if (index >= 0 && index < NoticeText.Count)
        {
            NoticeText[index].gameObject.SetActive(true);
        }

        // 버튼 이미지 컬러 변경
        for (int i = 0; i < NoticeButton.Count; i++)
        {
            ColorBlock colorBlock = NoticeButton[i].colors;
            if (i == index)
            {
                colorBlock.normalColor = new Color(1, 1, 1); // RGB 255,255,255
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
        // 각 버튼에 클릭 이벤트 리스너 추가
        for (int i = 0; i < NoticeButton.Count; i++)
        {
            int index = i; // 로컬 변수로 인덱스 캡처
            NoticeButton[i].onClick.AddListener(() => OnClickNoticeButton(index));
        }
    }

}
