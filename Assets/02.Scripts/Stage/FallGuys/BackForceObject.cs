using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackForceObject : MonoBehaviourPun
{
    private float _backForce = 5f; // 뒤로 밀리는 힘의 크기
    private Transform _trans;
    private ParticleSystem _bomb;

    private void Start()
    {
        _trans = GetComponentInChildren<Transform>();
        _bomb = GameObject.Find("Bomb").GetComponent<ParticleSystem>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PhotonView playerPhotonView = collision.gameObject.GetComponent<PhotonView>();
            Rigidbody rigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (playerPhotonView != null && rigidbody != null && playerPhotonView.IsMine)
            {
                Vector3 forceDirection = _trans.forward; // 게임 오브젝트의 forward 방향
                rigidbody.AddForce(forceDirection * _backForce);
                Debug.Log("밀려남");
                // RPC 호출
                photonView.RPC("ApplyBackForce", RpcTarget.All, collision.gameObject.GetComponent<PhotonView>().ViewID, forceDirection);
            }
        }
    }

    [PunRPC]
    void ApplyBackForce(int viewID, Vector3 forceDirection)
    {
        PhotonView playerPhotonView = PhotonView.Find(viewID);
        if (playerPhotonView != null)
        {
            Rigidbody rigidbody = playerPhotonView.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                Vector3 newPosition = playerPhotonView.transform.position + forceDirection * (_backForce / rigidbody.mass);
                newPosition.y = playerPhotonView.transform.position.y; // y값 유지
                rigidbody.MovePosition(newPosition);

                // 파티클 시스템 생성
                ParticleSystem instantiatedBomb = Instantiate(_bomb, playerPhotonView.transform.position + Vector3.up, Quaternion.identity);
                instantiatedBomb.transform.localScale *= 0.5f; // 스케일을 줄여서 생성
            }
        }
    }
}
