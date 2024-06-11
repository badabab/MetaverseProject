using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePortal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            if (gameObject.name == "BattleTilePortal")
            {
                // "BattleTileScene" 씬으로 이동
                PhotonNetwork.LoadLevel("BattleTileScene");
                Destroy(other.gameObject);
            }
            else if (gameObject.name == "FallGuysPortal")
            {
                // "FallGuysScene" 씬으로 이동
                PhotonNetwork.LoadLevel("FallGuysScene");
                Destroy(other.gameObject);
            }
            else if (gameObject.name == "TowerClimbPortal")
            {
                // "TowerClimbScene" 씬으로 이동
                PhotonNetwork.LoadLevel("TowerClimbScene");
                Destroy(other.gameObject);
            }
        }
    }
}
