using Photon.Pun;
using System.Collections;
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
            if (playerPhotonView != null && rigidbody != null && playerPhotonView.IsMine)
            {
                Vector3 forceDirection = _trans.forward; // 게임 오브젝트의 forward 방향
                Debug.Log("밀려남");
                Instantiate(_bomb, collision.transform.position, Quaternion.identity);
                SoundManager.instance.PlaySfx(SoundManager.Sfx.Hammer);

                StartCoroutine(ApplyPushForceCoroutine(rigidbody, forceDirection, _backForce));
            }
        }
    }

    private IEnumerator ApplyPushForceCoroutine(Rigidbody targetRigidbody, Vector3 pushDirection, float force)
    {
        if (targetRigidbody != null)
        {
            float duration = 0.5f;
            float elapsedTime = 0f;
            Vector3 initialPosition = targetRigidbody.position;
            Vector3 targetPosition = initialPosition + pushDirection * force;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                Vector3 newPosition = Vector3.Lerp(initialPosition, targetPosition, t);
                newPosition.y = initialPosition.y; // y값 유지
                targetRigidbody.MovePosition(newPosition);
                yield return null;
            }
        }
    }
}
