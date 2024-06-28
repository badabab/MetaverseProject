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

    public GameObject P1_UI;
    public GameObject P2_UI;
    public GameObject P3_UI;
    public GameObject P4_UI;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        P1_UI.SetActive(false);
        P2_UI.SetActive(false);
        P3_UI.SetActive(false);
        P4_UI.SetActive(false);
    }

    private void Update()
    {     
        if (BattleTileManager.Instance.CurrentGameState == GameState.Ready
            || BattleTileManager.Instance.CurrentGameState == GameState.Loading)
        {
            RefreshUI();
        }

        if (BattleTileManager.Instance.CurrentGameState == GameState.Go)
        {
            P1_Score.text = $"{TileScore.Instance.Player1score}";
            P2_Score.text = $"{TileScore.Instance.Player2score}";
            P3_Score.text = $"{TileScore.Instance.Player3score}";
            P4_Score.text = $"{TileScore.Instance.Player4score}";
        }      

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
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("IsReady_BattleTile", out object isReady))
        {
            bool isReadyValue = (bool)isReady;
            Ready.gameObject.SetActive(isReadyValue);
        }
    }

    public void RefreshUI()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.CustomProperties["PlayerNumber"] != null)
            {
                int playerNumber = (int)player.CustomProperties["PlayerNumber"];
                switch (playerNumber)
                {
                    case 1:
                        P1_UI.SetActive(true);
                        P1_name.text = player.NickName;
                        break;
                    case 2:
                        P2_UI.SetActive(true);
                        P2_name.text = player.NickName;
                        break;
                    case 3:
                        P3_UI.SetActive(true);
                        P3_name.text = player.NickName;
                        break;
                    case 4:
                        P4_UI.SetActive(true);
                        P4_name.text = player.NickName;
                        break;
                }
            }           
        }
    }
}
