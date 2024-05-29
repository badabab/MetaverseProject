using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_ArticleWrite : MonoBehaviour
{
    public Toggle NoticeToggle;
    public UI_ArticeList ArticleListUI;
    public InputField ContentInputFieldUI;

    public static UI_ArticleWrite Instance { get; private set; }
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
    public void OnClickYesButton()
    {
        ArticleType articleType = NoticeToggle.isOn ? ArticleType.Notice : ArticleType.Normal;
        string name = PlayerPrefs.GetString("LoggedInId");
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("사용자 이름을 찾을 수 없습니다.");
            return;
        }
        string content = ContentInputFieldUI.text;
        if (string.IsNullOrEmpty(content))
        {
            return;
        }
        ArticleManager.Instance.Write(articleType, name, content);
        ArticleListUI.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
    public void OnClickNoButton()
    {
        ArticleListUI.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
