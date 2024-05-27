using Photon.Pun;
using UnityEngine;

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
                PhotonNetwork.LoadLevel("BattleTileScene");
                //SceneManager.LoadScene("BattleTileScene");
            }
            else if (gameObject.CompareTag("FallGuysPortal"))
            {
                PhotonNetwork.LoadLevel($"FallGuysScene{Random.Range(1,4)}");
            }
            else if (gameObject.CompareTag("TowerClimbPortal"))
            {
                PhotonNetwork.LoadLevel("TowerClimbScene");
            }
        }
    }
}
