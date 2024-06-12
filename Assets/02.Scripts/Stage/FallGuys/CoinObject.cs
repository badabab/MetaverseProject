using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(CoinAfter(5));
            PhotonView playerPhotonView = other.GetComponentInParent<PhotonView>();
            if (playerPhotonView != null) 
            {
                PersonalManager.Instance.CoinUpdate(playerPhotonView.Owner.NickName, 10);
            }
        }
    }

    public IEnumerator CoinAfter(float delay)
    {
        this.gameObject.SetActive(false);
        yield return new WaitForSeconds(delay);
        this.gameObject.SetActive(true);
    }
}
