using Photon.Pun;
using System.Collections;
using UnityEngine;

public class AerialBomb : MonoBehaviourPunCallbacks, IPunObservable
{
    public GameObject explosionEffectPrefab; // 폭발 효과 프리팹
    public float BombFall = 8.5f; // 폭발까지의 지연 시간
    public float BombTime = 6f; // 폭발 효과 지속 시간

    private bool hasExploded = false; // 폭발 여부

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
        }
    }

    [PunRPC]
    void Explode()
    {
        // 폭발 효과를 생성하고 모든 클라이언트에게 동기화합니다.
        GameObject explosion = Instantiate(explosionEffectPrefab, transform.position, transform.rotation);

        // BombTime 시간 후에 폭발 효과를 파괴합니다.
        Destroy(explosion, BombTime);

        hasExploded = true;

        // 폭탄이 소유한 클라이언트만 폭탄 게임 오브젝트를 파괴합니다.
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // 이 예제에서는 네트워크 동기화가 필요하지 않으므로 구현하지 않습니다.
    }
}