using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinshLine : MonoBehaviour
{
    public GameObject FinshLineSphere;
    private float Speed = 50f;
    private CharacterController _characterController;
    private bool isFirstPlayerDetected = false;
    private string firstPlayerId;

    private Dictionary<string, int> _player = new Dictionary<string, int>();

    private void Update()
    {
        if (FinshLineSphere != null)
        {
            FinshLineSphere.transform.Rotate(0, Speed * Time.deltaTime, 0); 
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _characterController = other.GetComponent<CharacterController>();
            PhotonView playerPhotonView = other.GetComponentInParent<PhotonView>();

            if (gameObject.name == "FinshSphere")
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
    public void InitScore()
    {
        Hashtable hashtable = new Hashtable();
        hashtable.Add("CharacterIndex", 100);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }
}

