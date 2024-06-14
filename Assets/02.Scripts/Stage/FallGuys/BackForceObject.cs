using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackForceObject : MonoBehaviour
{
    private float _backForce = 500f; // 뒤로 밀리는 힘의 크기
    private Transform _trans;
    private ParticleSystem _bombPlayer;

    private void Start()
    {
        _trans = GetComponentInChildren<Transform>();
        _bombPlayer = GameObject.Find("Bomb").GetComponent<ParticleSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PhotonView playerPhotonView = other.GetComponent<PhotonView>();
            Rigidbody rigidbody = other.GetComponent<Rigidbody>();

            if (playerPhotonView != null && rigidbody != null && playerPhotonView.IsMine)
            {
                Vector3 forceDirection = _trans.forward; // 게임 오브젝트의 forward 방향
                rigidbody.AddForce(forceDirection * _backForce);
                Transform transform = other.GetComponentInChildren<Transform>();
                Instantiate(_bombPlayer, transform.position + Vector3.up, Quaternion.identity);
            }
        }
    }
}
