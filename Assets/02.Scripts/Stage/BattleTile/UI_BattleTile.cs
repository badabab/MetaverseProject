using Photon.Pun;
using TMPro;
using UnityEngine;

public class UI_BattleTile : MonoBehaviour
{
    public TextMeshProUGUI P1_name;
    public TextMeshProUGUI P2_name;
    public TextMeshProUGUI P3_name;
    public TextMeshProUGUI P4_name;

    public TextMeshProUGUI P1_Score;
    public TextMeshProUGUI P2_Score;
    public TextMeshProUGUI P3_Score;
    public TextMeshProUGUI P4_Score;

    public TextMeshProUGUI Timer;

    private void Start()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            int playerNumber = (int)player.CustomProperties["PlayerNumber"];

            switch (playerNumber)
            {
                case 1:
                    player.NickName = P1_name.text;
                    break;
                case 2:
                    player.NickName = P2_name.text;
                    break;
                case 3:
                    player.NickName = P3_name.text;
                    break;
                case 4:
                    player.NickName = P4_name.text;
                    break;
            }
        }
    }

    private void Update()
    {
        P1_Score.text = $"{TileScore.Instance.Player1score}";
        P2_Score.text = $"{TileScore.Instance.Player2score}";
        P3_Score.text = $"{TileScore.Instance.Player3score}";
        P4_Score.text = $"{TileScore.Instance.Player4score}";

        Timer.text = $"{(int)BattleTileManager.Instance.TimeRemaining}";
    }
}
