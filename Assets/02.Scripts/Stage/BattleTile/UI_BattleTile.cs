using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattleTile : MonoBehaviourPunCallbacks
{
    public static UI_BattleTile Instance {  get; private set; }

    public TextMeshProUGUI P1_name;
    public TextMeshProUGUI P2_name;
    public TextMeshProUGUI P3_name;
    public TextMeshProUGUI P4_name;

    public TextMeshProUGUI P1_Score;
    public TextMeshProUGUI P2_Score;
    public TextMeshProUGUI P3_Score;
    public TextMeshProUGUI P4_Score;

    public TextMeshProUGUI Timer;

    public GameObject ReadyDescriptionUI;
    public GameObject ReadyUI;
    public GameObject NotReady;
    public GameObject Ready;

    public GameObject GameEndUI;
    public Image Gameover;
    public Image Lose;
    public Image Win;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameEndUI.SetActive(false);
        Gameover.gameObject.SetActive(true);
        Lose.gameObject.SetActive(false);
        Win.gameObject.SetActive(false);
    }

    private void Update()
    {     
        P1_Score.text = $"{TileScore.Instance.Player1score}";
        P2_Score.text = $"{TileScore.Instance.Player2score}";
        P3_Score.text = $"{TileScore.Instance.Player3score}";
        P4_Score.text = $"{TileScore.Instance.Player4score}";

        Timer.text = $"{(int)BattleTileManager.Instance.TimeRemaining}";

        if (BattleTileManager.Instance.CurrentGameState == GameState.Ready || BattleTileManager.Instance.CurrentGameState == GameState.Loading)
        {
            if (BattleTileManager.Instance.CurrentGameState == GameState.Ready)
            {
                ReadyDescriptionUI.SetActive(true);
                ReadyUI.gameObject.SetActive(true);
            }
            else if (BattleTileManager.Instance.CurrentGameState == GameState.Loading)
            {
                ReadyDescriptionUI.SetActive(false);
                ReadyUI.gameObject.SetActive(false);
            }
            CheakReadyButton();
        }        
    }

    void CheakReadyButton()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("IsReady", out object isReady))
        {
            bool isReadyValue = (bool)isReady;
            Ready.gameObject.SetActive(isReadyValue);
        }
    }

    public void RefreshUI()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            int playerNumber = (int)player.CustomProperties["PlayerNumber"];
            switch (playerNumber)
            {
                case 1:
                    P1_name.text = player.NickName;
                    break;
                case 2:
                    P2_name.text = player.NickName;
                    break;
                case 3:
                    P3_name.text = player.NickName;
                    break;
                case 4:
                    P4_name.text = player.NickName;
                    break;
            }
        }
    }

    public void CheckWin()
    {
        StartCoroutine(ShowPopup_Coroutine(Win));
    }
    public void CheckLose()
    {
        StartCoroutine(ShowPopup_Coroutine(Lose));
    }

    public IEnumerator ShowPopup_Coroutine (Image image)
    {
        GameEndUI.SetActive(true);
        yield return new WaitForSeconds(2);
        Gameover.gameObject.SetActive(false);
        image.gameObject.SetActive(true);

        yield return new WaitForSeconds(3);
        GameEndUI.SetActive(false);
    }
}
