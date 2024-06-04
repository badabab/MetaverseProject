using Photon.Pun;
using TMPro;
using UnityEngine;

public class EndCollider : MonoBehaviourPunCallbacks
{
    public Transform Start2, Start3;
    private CharacterController _characterController;
    private bool isFirstPlayerDetected = false;
    private string firstPlayerNickName;


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
                if (!isFirstPlayerDetected)
                {
                    FallGuysManager.Instance.SetGameState(GameState.Over);
                    isFirstPlayerDetected = true;
                    firstPlayerNickName = playerPhotonView.Owner.NickName;
                    Debug.Log($"{firstPlayerNickName} reached the end first!");
                    PersonalManager.Instance.CoinUpdate(playerPhotonView.Owner.NickName);
                }

                // 모든 플레이어에 대해 승/패 여부를 업데이트
                if (playerPhotonView.IsMine)
                {
                    if (firstPlayerNickName == playerPhotonView.Owner.NickName)
                    {
                        UI_GameOver.Instance.CheckFirst();
                    }
                    else
                    {
                        UI_GameOver.Instance.CheckLast();
                    }
                }

                Debug.Log("게임 끝");
            }
        }
    }
}
