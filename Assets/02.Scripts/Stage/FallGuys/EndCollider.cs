using Photon.Pun;
using TMPro;
using UnityEngine;

public class EndCollider : MonoBehaviourPunCallbacks
{
    public Transform Start2, Start3;
    private Rigidbody _rb;
    private bool isFirstPlayerDetected = false;
    private string firstPlayerNickName;

    private Animator animator;

    public TextMeshProUGUI CountNumber;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _rb = other.GetComponent<Rigidbody>();
            PhotonView playerPhotonView = other.GetComponentInParent<PhotonView>();
            if (gameObject.name == "End1")
            {
                Debug.Log("End1 도착");
                _rb.gameObject.SetActive(false);
                other.transform.position = Start2.position;
                _rb.gameObject.SetActive(true);
                CountNumber.text = "2";
            }
            else if (gameObject.name == "End2")
            {
                Debug.Log("End2 도착");
                _rb.gameObject.SetActive(false);
                other.transform.position = Start3.position;
                _rb.gameObject.SetActive(true);
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
                        var playermove = playerPhotonView.gameObject.GetComponent<PlayerMoveAbility>();
                        var animator = playermove.GetComponent<Animator>();
                        animator.SetBool("Win", true);
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
