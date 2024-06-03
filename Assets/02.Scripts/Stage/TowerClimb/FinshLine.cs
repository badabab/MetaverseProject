using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinshLine : MonoBehaviour
{
    public GameObject FinshLineSphere;
    private float Speed = 50f;

    private void Update()
    {
        if (FinshLineSphere != null)
        {
            FinshLineSphere.transform.Rotate(0, Speed * Time.deltaTime, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !TowerClimbManager.Instance.isFirstPlayerDetected)
        {
            TowerClimbManager.Instance.isFirstPlayerDetected = true;
            PhotonView playerPhotonView = other.GetComponentInParent<PhotonView>();

            if (playerPhotonView != null)
            {
                TowerClimbManager.Instance.SetGameState(TowerClimbGameState.Over);
                TowerClimbManager.Instance.firstPlayerId = playerPhotonView.Owner.UserId;
                Debug.Log($"{playerPhotonView.Owner.NickName} reached the end first!");
                Debug.Log("게임 끝");
                PersonalManager.Instance.CoinUpdate(playerPhotonView.Owner.NickName);
            }
        }
    }
}

