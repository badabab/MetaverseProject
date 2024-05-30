using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class EndCollider : MonoBehaviourPunCallbacks
{
    public Transform Start2, Start3;
    private CharacterController _characterController;
    private bool isFirstPlayerDetected = false;
    private string firstPlayerId;

    private Dictionary<string, int> _player = new Dictionary<string, int>();


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
                    Debug.Log("게임 끝");
                    PersonalManager.Instance.CoinUpdate(playerPhotonView.Owner.NickName);
/*                    if (playerPhotonView.Owner.IsLocal) 
                    {
                        InitScore();
                    }*/
                }
            }
        }
    }
    public void InitScore()
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add("CharacterIndex", 100);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }
}
