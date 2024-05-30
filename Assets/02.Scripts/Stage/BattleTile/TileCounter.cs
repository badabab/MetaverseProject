using UnityEngine;

public class TileCounter : MonoBehaviour
{
    public Material Tile_Blue;
    public Material Tile_Green;
    public Material Tile_Red;
    public Material Tile_Yellow;

    public int Player1score;
    public int Player2score;
    public int Player3score;
    public int Player4score;

    void Update()
    {
        CountTileObjectsUsingMaterials();
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log($"P1_B: {Player1score}");
            Debug.Log($"P2_G: {Player2score}");
            Debug.Log($"P3_R: {Player3score}");
            Debug.Log($"P4_Y: {Player4score}");
        }
    }

    void CountTileObjectsUsingMaterials()
    {
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
                        else if (mat == Tile_Red)
                        {
                            Player3score++;
                            break;
                        }
                        else if (mat == Tile_Yellow)
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
