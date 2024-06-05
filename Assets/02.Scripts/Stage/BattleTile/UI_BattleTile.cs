using Photon.Pun;
using TMPro;

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

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {     
        P1_Score.text = $"{TileScore.Instance.Player1score}";
        P2_Score.text = $"{TileScore.Instance.Player2score}";
        P3_Score.text = $"{TileScore.Instance.Player3score}";
        P4_Score.text = $"{TileScore.Instance.Player4score}";

        Timer.text = $"{(int)BattleTileManager.Instance.TimeRemaining}";
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
}
