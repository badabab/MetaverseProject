using Photon.Pun;
using System.Collections;
using UnityEngine;

public class AerialBomb : MonoBehaviourPunCallbacks
{
    private PhotonView photonView;

    public GameObject explosionEffectPrefab;
    public float BombFall = 8.5f;
    public float BombTime = 6f;

    private bool hasExploded = false;

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        StartCoroutine(DropBomb());
    }

    IEnumerator DropBomb()
    {
        yield return new WaitForSeconds(BombFall);

        if (!hasExploded)
        {
            int explosionIndex = GetUniqueExplosionIndex();
            photonView.RPC("Explode", RpcTarget.AllViaServer, explosionIndex);
            Explode(explosionIndex); 
        }

        Destroy(this.gameObject);
        PhotonNetwork.Destroy(this.gameObject);
    }

    [PunRPC]
    void Explode(int explosionIndex)
    {
        GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, transform.rotation);

        Destroy(explosion, BombTime);

        hasExploded = true;

        if (photonView.IsMine)
        {
            Destroy(this.gameObject);
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    int GetUniqueExplosionIndex()
    {
        return Random.Range(1000, 9999);
    }
}