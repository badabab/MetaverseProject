using Photon.Pun;
using System.Collections;
using UnityEngine;

public class AerialBomb : MonoBehaviourPunCallbacks
{
    private PhotonView bombPhotonView;

    public GameObject explosionEffectPrefab;
    public float BombFall = 8.5f;
    public float BombTime = 6f;

    private bool hasExploded = false;

    void Start()
    {
        bombPhotonView = GetComponent<PhotonView>();

        StartCoroutine(DropBomb());
    }

    IEnumerator DropBomb()
    {
        yield return new WaitForSeconds(BombFall);

        if (!hasExploded)
        {
            int explosionIndex = GetUniqueExplosionIndex();
            bombPhotonView.RPC("Explode", RpcTarget.AllViaServer, explosionIndex);
            Explode(explosionIndex); 
        }

        Destroy(this.gameObject);
        PhotonNetwork.Destroy(this.gameObject);
    }

    [PunRPC]
    void Explode(int explosionIndex)
    {
        GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
        SoundManager.instance.PlaySfx(SoundManager.Sfx.VillageInteractiveObjectRocket);
        Destroy(explosion, BombTime);

        hasExploded = true;

        if (bombPhotonView.IsMine)
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