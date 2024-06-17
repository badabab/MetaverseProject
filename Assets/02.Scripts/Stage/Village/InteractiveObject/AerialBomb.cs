using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AerialBomb : MonoBehaviour
{
    public GameObject explosionEffect;

    private void OnTriggerEnter(Collider other)
    {
        PhotonView photonView = other.GetComponent<PhotonView>();

        if (photonView != null && photonView.IsMine)
        {
            other.gameObject.SetActive(false);

            if (explosionEffect != null)
            {
                GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
                explosion.SetActive(true);

                Destroy(explosion, 3f);
            }

            Destroy(other.gameObject, 3f);
        }
    }
}
