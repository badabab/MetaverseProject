using TMPro;
using UnityEngine;

public class UI_BattleTile : MonoBehaviour
{
    public TextMeshProUGUI P1_Score;
    public TextMeshProUGUI P2_Score;
    public TextMeshProUGUI P3_Score;
    public TextMeshProUGUI P4_Score;

    public TextMeshProUGUI Timer;

    private void Update()
    {
        P1_Score.text = $"{TileScore.Instance.Player1score}";
        P2_Score.text = $"{TileScore.Instance.Player2score}";
        P3_Score.text = $"{TileScore.Instance.Player3score}";
        P4_Score.text = $"{TileScore.Instance.Player4score}";

        //Timer.text = $"{(int)BattleTileManager.Instance.TimeRemaining}";
        Timer.text = "0";
    }
}
