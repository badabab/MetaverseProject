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
            if (gameObject.name == ("BattleTilePortal"))
            {
                //PhotonNetwork.LoadLevel("BattleTileScene");
                SceneManager.LoadScene("BattleTileScene");
            }
            else if (gameObject.name == ("FallGuysPortal"))
            {
                //PhotonNetwork.LoadLevel("FallGuysScene");
                SceneManager.LoadScene("FallGuysScene");
            }
            else if (gameObject.name == ("TowerClimbPortal"))
            {
                //PhotonNetwork.LoadLevel("TowerClimbScene");
                SceneManager.LoadScene("TowerClimbScene");
            }
        }
    }
}
