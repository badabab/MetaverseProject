using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileScore : MonoBehaviour
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

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        CountTileObjectsUsingMaterials();
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
    }

}