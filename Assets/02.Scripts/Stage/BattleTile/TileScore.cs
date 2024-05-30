using TMPro;
using UnityEngine;

public class TileScore : MonoBehaviour
{
    public Material Tile_Blue;
    public Material Tile_Green;
    public Material Tile_Pink;
    public Material Tile_Violet;

    public TextMeshProUGUI P1_Score;
    public TextMeshProUGUI P2_Score;
    public TextMeshProUGUI P3_Score;
    public TextMeshProUGUI P4_Score;

    private int _player1score;
    private int _player2score;
    private int _player3score;
    private int _player4score;

    void Update()
    {
        CountTileObjectsUsingMaterials();

        P1_Score.text = $"{_player1score}";
        P2_Score.text = $"{_player2score}";
        P3_Score.text = $"{_player3score}";
        P4_Score.text = $"{_player4score}";
    }

    void CountTileObjectsUsingMaterials()
    {
        _player1score = 0;
        _player2score = 0;
        _player3score = 0;
        _player4score = 0;

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
                        if (mat == Tile_Blue)
                        {
                            _player1score++;
                            break;
                        }
                        else if (mat == Tile_Green)
                        {
                            _player2score++;
                            break;
                        }
                        else if (mat == Tile_Pink)
                        {
                            _player3score++;
                            break;
                        }
                        else if (mat == Tile_Violet)
                        {
                            _player4score++;
                            break;
                        }
                    }
                }
            }
        }        
    }
}
