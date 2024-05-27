using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasAbility : PlayerAbility
{
    public static PlayerCanvasAbility Instance {  get; private set; }

    public Canvas PlayerCanvas;
    public Text NicknameTextUI;

    private void Start()
    {
        Instance = this;
        //NicknameTextUI.text = _owner.PhotonView.Controller.NickName;
    }
    private void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }

    public void SetNickname(string nickname)
    {
        if (NicknameTextUI != null)
        {
            NicknameTextUI.text = nickname;
        }
    }
}
