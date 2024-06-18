using Photon.Pun;
using System.Collections;
using UnityEngine;

public class AerialBomb : MonoBehaviourPunCallbacks
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
            int explosionIndex = GetUniqueExplosionIndex();
            photonView.RPC("Explode", RpcTarget.AllViaServer, explosionIndex);
            Explode(explosionIndex);
        }

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
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    int GetUniqueExplosionIndex()
    {
        return Random.Range(1000, 9999);
    }
}