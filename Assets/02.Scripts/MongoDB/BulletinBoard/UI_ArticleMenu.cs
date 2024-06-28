using UnityEngine;

public class UI_ArticleMenu : MonoBehaviour
{
    private Article _article;

    public static UI_ArticleMenu Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        this.gameObject.SetActive(false);
    }
    public void Show(in Article article)
    {
        _article = article;
        gameObject.SetActive(true);
    }
    public void OnClickModifyButton()
    {
        SoundManager.instance.PlaySfx(SoundManager.Sfx.UI_LobbyButtonQTutorialsButton);
        UI_ArticleModify.Instance.Show(_article);
        gameObject.SetActive(false);
    }
    public void OnClickDeleteButton()
    {
        SoundManager.instance.PlaySfx(SoundManager.Sfx.UI_LobbyButtonQTutorialsButton);
        ArticleManager.Instance.Delete(_article.Id);
        ArticleManager.Instance.FindAll();
        this.gameObject.SetActive(false);
        UI_ArticeList.Instance.Refresh();
    }
    public void OnClickBackground()
    {
        SoundManager.instance.PlaySfx(SoundManager.Sfx.UI_LobbyButtonQTutorialsButton);
        gameObject.SetActive(false);
    }
}
