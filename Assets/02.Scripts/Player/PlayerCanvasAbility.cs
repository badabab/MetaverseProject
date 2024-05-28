using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasAbility : PlayerAbility
{
    public static PlayerCanvasAbility Instance {  get; private set; }

    public Canvas PlayerCanvas;
    public Text NicknameTextUI;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        string nickname = PlayerPrefs.GetString("LoggedInId");
        NicknameTextUI.text = nickname;
    }
    private void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }

    public void SetNickname(string nickname)
    {
        NicknameTextUI.text = nickname;
        Debug.Log($"{nickname}");
    }
}
