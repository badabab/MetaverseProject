using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class TileScore : MonoBehaviourPunCallbacks
{
    public static TileScore Instance { get; private set; }

    public Material Tile_Green;
    public Material Tile_Pink;
    public Material Tile_Yellow;
    public Material Tile_Blue;

    public int Player1score;
    public int Player2score;
    public int Player3score;
    public int Player4score;

    private int _previousPlayer1score;
    private int _previousPlayer2score;
    private int _previousPlayer3score;
    private int _previousPlayer4score;
    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        CountTileObjectsUsingMaterials();
        PlayScoreIncreaseSound();
    }

    void CountTileObjectsUsingMaterials()
    {
        Player1score = 0;
        Player2score = 0;
        Player3score = 0;
        Player4score = 0;


        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("Tile"))
            {
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    foreach (Material mat in renderer.sharedMaterials)
                    {
                        if (mat == Tile_Green)
                        {
                            Player1score++;
                            break;
                        }
                        else if (mat == Tile_Pink)
                        {
                            Player2score++;
                            break;
                        }
                        else if (mat == Tile_Yellow)
                        {
                            Player3score++;
                            break;
                        }
                        else if (mat == Tile_Blue)
                        {
                            Player4score++;
                            break;
                        }
                    }
                }
            }
        }
    }

    void PlayScoreIncreaseSound()
    {
        if (Player1score > _previousPlayer1score || Player2score > _previousPlayer2score ||
            Player3score > _previousPlayer3score || Player4score > _previousPlayer4score)
        {
            SoundManager.instance.PlaySfx(SoundManager.Sfx.Tile);
        }

        _previousPlayer1score = Player1score;
        _previousPlayer2score = Player2score;
        _previousPlayer3score = Player3score;
        _previousPlayer4score = Player4score;
    }

    public void DetermineWinner()
    {
        Dictionary<string, int> playerScores = new Dictionary<string, int>();

        foreach (var player in PhotonNetwork.PlayerList)
        {
            string playerName = player.NickName;
            int playerNumber = (int)player.CustomProperties["PlayerNumber"];

            switch (playerNumber)
            {
                case 1:
                    playerScores[playerName] = Player1score;
                    break;
                case 2:
                    playerScores[playerName] = Player2score;
                    break;
                case 3:
                    playerScores[playerName] = Player3score;
                    break;
                case 4:
                    playerScores[playerName] = Player4score;
                    break;
            }
        }

        string winner = playerScores.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
        int maxScore = playerScores[winner];

        Debug.Log($"Winner is {winner} with {maxScore} tiles!");
        PersonalManager.Instance.CoinUpdate(winner, 100);

        foreach (var player in PhotonNetwork.PlayerList)
        {

            string playerName = player.NickName;

            if (PhotonNetwork.IsMasterClient && playerName == winner)
            {
                Hashtable firstPlayerName = new Hashtable { { "FirstPlayerName", playerName } };
                PhotonNetwork.CurrentRoom.SetCustomProperties(firstPlayerName);
                Debug.Log($"{firstPlayerName} 저장");
            }
        }
    }
}