using TMPro;
using UnityEngine;

public class TileScore : MonoBehaviour
{
    public static TileScore Instance { get; private set; }

    public Material Tile_Blue;
    public Material Tile_Green;
    public Material Tile_Pink;
    public Material Tile_Violet;

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
                        if (mat == Tile_Blue)
                        {
                            Player1score++;
                            break;
                        }
                        else if (mat == Tile_Green)
                        {
                            Player2score++;
                            break;
                        }
                        else if (mat == Tile_Pink)
                        {
                            Player3score++;
                            break;
                        }
                        else if (mat == Tile_Violet)
                        {
                            Player4score++;
                            break;
                        }
                    }
                }
            }
        }        
    }
}
