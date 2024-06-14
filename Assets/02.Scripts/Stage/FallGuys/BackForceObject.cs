using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackForceObject : MonoBehaviour
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
            Debug.Log("때림");
            if (playerPhotonView != null && rigidbody != null && playerPhotonView.IsMine)
            {
                Vector3 forceDirection = _trans.forward; // 게임 오브젝트의 forward 방향
                rigidbody.AddForce(forceDirection * _backForce);
                Debug.Log("밀려남");
                // 파티클 시스템 생성
                Instantiate(_bomb, collision.transform.position + Vector3.up, Quaternion.identity);
                Debug.Log("파티클 생성");
                // y값을 유지하면서 이동
                Vector3 newPosition = collision.transform.position + forceDirection * (_backForce / rigidbody.mass);
                newPosition.y =collision.transform.position.y; // y값 유지
                rigidbody.MovePosition(newPosition);
            }
        }
    }
}
