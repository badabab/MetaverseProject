using Photon.Pun;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndCollider : MonoBehaviourPunCallbacks
{
    public Transform Start2, Start3;
    private CharacterController _characterController;
    private bool isFirstPlayerDetected = false;
    private string firstPlayerId;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _characterController = other.GetComponent<CharacterController>();
            PhotonView playerPhotonView = other.GetComponentInParent<PhotonView>();
            if (gameObject.name == "End1")
            {
                Debug.Log("End1 도착");
                _characterController.enabled = false;
                other.transform.position = Start2.position;
                _characterController.enabled = true;
            }
            else if (gameObject.name == "End2")
            {
                Debug.Log("End2 도착");
                _characterController.enabled = false;
                other.transform.position = Start3.position;
                _characterController.enabled = true;
            }
            else if (gameObject.name == "End3")
            {
                FallGuysManager.Instance.SetGameState(GameState.Over);
                if (playerPhotonView != null)
                {
                    isFirstPlayerDetected = true;
                    firstPlayerId = playerPhotonView.Owner.UserId;
                    Debug.Log($"{playerPhotonView.Owner.NickName} reached the end first!");
                    photonView.RPC("AnnounceWinner", RpcTarget.All, playerPhotonView.Owner.NickName, playerPhotonView.Owner.UserId);
                }
                Debug.Log("게임 끝");
            }
        }
    }
    [PunRPC]
    void AnnounceWinner(string winnerName, string winnerId)
    {
        Debug.Log($"{winnerName} is the winner!");
        PlayerPrefs.SetString("WinnerId", winnerId);
    }
}
