using Photon.Pun;
using TMPro;
using UnityEngine;

public class PlayerCanvasAbility : MonoBehaviourPunCallbacks
{
    public static PlayerCanvasAbility Instance { get; private set; }

    public Canvas PlayerCanvas;
    public TextMeshProUGUI NicknameTextUI;

    private PhotonView parentPhotonView;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // 부모 객체에서 PhotonView를 찾습니다.
        parentPhotonView = GetComponentInParent<PhotonView>();
        if (parentPhotonView == null)
        {
            Debug.LogError("Parent PhotonView not found!");
        }
    }

    private void Start()
    {
        SetNickname(PhotonNetwork.NickName);
    }

    private void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }

    public void SetNickname(string nickname)
    {
        NicknameTextUI.text = nickname;
    }
}
