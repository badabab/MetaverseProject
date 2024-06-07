using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePortal : MonoBehaviourPunCallbacks
{
    private Collider _collider;
    private void Start()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            if (gameObject.name == ("BattleTilePortal"))
            {
                //PhotonNetwork.LoadLevel("BattleTileScene");
                //SceneManager.LoadScene("BattleTileScene");
                photonView.RPC("ChangeSceneRPC", RpcTarget.AllBuffered, "BattleTilePortal");
            }
            else if (gameObject.name == ("FallGuysPortal"))
            {
                SceneManager.LoadScene("FallGuysScene");
                photonView.RPC("ChangeSceneRPC", RpcTarget.AllBuffered, "FallGuysScene");
            }
            else if (gameObject.name == ("TowerClimbPortal"))
            {
                SceneManager.LoadScene("TowerClimbScene");
                photonView.RPC("ChangeSceneRPC", RpcTarget.AllBuffered, "TowerClimbScene");
            }
        }
    }

    [PunRPC]
    void ChangeSceneRPC(string sceneName)
    {
        // 각 클라이언트가 로컬에서 씬을 전환함
        SceneManager.LoadScene(sceneName);
    }
}
