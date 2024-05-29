using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_ArticleWrite : MonoBehaviour
{
    public InputField ContentInputFieldUI;
    public static UI_ArticleWrite Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        this.gameObject.SetActive(false);
    }
    public void Show()
    {
        this.gameObject.SetActive(true);
    }
    public void OnClickYesButton()
    {
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
        ArticleManager.Instance.Write(name, content);
        Debug.Log("새로운 아티클이 추가되었습니다.");
        ArticleManager.Instance.FindAll();
        UI_ArticeList.Instance.Show();
        gameObject.SetActive(false);
    }
    public void OnClickNoButton()
    {
        UI_ArticeList.Instance.Show();
        gameObject.SetActive(false);
    }
}
