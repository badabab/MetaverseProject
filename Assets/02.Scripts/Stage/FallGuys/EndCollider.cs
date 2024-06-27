using ExitGames.Client.Photon;
using Photon.Pun;
using System.Linq;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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
        PhotonView playerPhotonView = other.GetComponentInParent<PhotonView>();
        if (other.CompareTag("Player") && playerPhotonView.IsMine)
        {
            SoundManager.instance.PlaySfx(SoundManager.Sfx.UI_FallGuysStageMove);
            _rb = other.GetComponent<Rigidbody>();
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
                    // 자기 자신만 바꾸죠
                    FallGuysManager.Instance.SetGameState(GameState.Over);
                    isFirstPlayerDetected = true;
                    firstPlayerNickName = playerPhotonView.Owner.NickName;
                    Debug.Log($"{firstPlayerNickName} reached the end first!");
                    PersonalManager.Instance.CoinUpdate(playerPhotonView.Owner.NickName, 100);

                    if (PhotonNetwork.IsMasterClient)
                    {
                        Hashtable firstPlayerName = new Hashtable { { "FirstPlayerName", firstPlayerNickName } };
                        PhotonNetwork.CurrentRoom.SetCustomProperties(firstPlayerName);
                        Debug.Log($"{firstPlayerName} 저장");
                    }
                }

                Debug.Log("게임 끝");
            }
        }
    }
}
