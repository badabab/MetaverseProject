using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePortal : MonoBehaviour
{
    public Collider BattleTilePotal;
    public Collider FallGuysPotal;
    public Collider TowerClimbPotal;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (this.gameObject.CompareTag("BattlePortal"))
            {
                SceneManager.LoadScene("BattleTileScene");
            }
            else if (this.gameObject.CompareTag("FallGuysPortal"))
            {
                SceneManager.LoadScene($"FallGuysScene{Random.Range(1,4)}");
            }
            else if (this.gameObject.CompareTag("TowerClimbPortal"))
            {
                SceneManager.LoadScene("TowerClimbScene");
            }
        }
    }
}
