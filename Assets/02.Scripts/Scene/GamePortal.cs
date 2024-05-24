using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePortal : MonoBehaviour
{
    private Collider _collider;
    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("BattleTilePortal"))
            {
                SceneManager.LoadScene("BattleTileScene");
            }
            else if (gameObject.CompareTag("FallGuysPortal"))
            {
                SceneManager.LoadScene($"FallGuysScene{Random.Range(1,4)}");
            }
            else if (gameObject.CompareTag("TowerClimbPortal"))
            {
                SceneManager.LoadScene("TowerClimbScene");
            }
        }
    }
}
