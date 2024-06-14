using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpinObject : MonoBehaviourPun
{
    public Transform Spin;
    public float speed = 90.0f;  // 초당 90도 회전

    private ParticleSystem _sweat;

    private void Start()
    {
        _sweat = GameObject.Find("Sweat").GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        // Y축을 중심으로 오른쪽으로 회전
        Spin.Rotate(Vector3.up, speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(Spin);
            Instantiate(_sweat, other.transform.position + Vector3.up, Quaternion.identity);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
            _sweat.gameObject.SetActive(false);
        }
    }
}
