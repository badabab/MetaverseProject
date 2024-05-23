using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvasAbility : PlayerAbility
{
    public Canvas PlayerCanvas;
    public Text NicknameTextUI;

    private void Start()
    {
        NicknameTextUI.text = _owner.PhotonView.Controller.NickName;
    }
    private void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }
}
