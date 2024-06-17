using Photon.Pun;
using System.Collections;
using UnityEngine;

public class AerialBomb : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject explosionEffectPrefab; 
    public float BombFall = 8.5f; 
    public float BombTime = 6f; 

    private bool hasExploded = false; 

    void Start()
    {
        StartCoroutine(DropBomb());
    }

    IEnumerator DropBomb()
    {
        yield return new WaitForSeconds(BombFall);

        if (!hasExploded)
        {
            photonView.RPC("Explode", RpcTarget.AllViaServer);
            Explode();
        }
    }

    [PunRPC]
    void Explode()
    {
        GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, transform.rotation);

        Destroy(explosion, BombTime);

        hasExploded = true;

        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

    }
}