using Photon.Pun;
using TMPro;
using UnityEngine;

public class EndCollider : MonoBehaviourPunCallbacks
{
    public Transform Start2, Start3;
    private CharacterController _characterController;
    private bool isFirstPlayerDetected = false;
    private string firstPlayerId;


    public TextMeshProUGUI CountNumber;
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
                CountNumber.text = "2";
            }
            else if (gameObject.name == "End2")
            {
                Debug.Log("End2 도착");
                _characterController.enabled = false;
                other.transform.position = Start3.position;
                _characterController.enabled = true;
                CountNumber.text = "3";
            }
            else if (gameObject.name == "End3")
            {
                FallGuysManager.Instance.SetGameState(GameState.Over);
                if (playerPhotonView != null)
                {
                    isFirstPlayerDetected = true;
                    firstPlayerId = playerPhotonView.Owner.UserId;
                    Debug.Log($"{playerPhotonView.Owner.NickName} reached the end first!");
                    Debug.Log("게임 끝");
                    PersonalManager.Instance.CoinUpdate(playerPhotonView.Owner.NickName);
                }
            }
        }
    }
}
