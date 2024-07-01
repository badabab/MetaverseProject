using Photon.Pun;
using System.Collections;
using UnityEngine;

public class PlayerAttackAbility : MonoBehaviourPunCallbacks
{
    public LayerMask playerLayer;
    public Animator _animator;
    private Collider punchCollider;
    private bool isAttacking = false;
    private PlayerMoveAbility playerMoveAbility;

    void Start()
    {
        playerMoveAbility = GetComponent<PlayerMoveAbility>();
        _animator = GetComponent<Animator>();
        punchCollider = GetComponentInChildren<Collider>();

        if (punchCollider != null && punchCollider.CompareTag("Hand"))
        {
            punchCollider.isTrigger = true;
            punchCollider.enabled = false;
        }
    }

    void Update()
    {
        if (!photonView.IsMine)
            return;

        if (Input.GetMouseButtonDown(1))
        {

            if (playerMoveAbility._isRunning)
            {
                _animator.SetBool("FlyingAttack", true);
                SoundManager.instance.PlaySfx(SoundManager.Sfx.PlayerFlyingKick);
            }
            else
            {
                if (_animator.GetCurrentAnimatorStateInfo(3).IsName("Attack"))
                {
                    _animator.SetBool("Attack", false);
                    _animator.SetBool("Attack2", true);
                    SoundManager.instance.PlaySfx(SoundManager.Sfx.PlayerPunch2);
                }
                else if (_animator.GetCurrentAnimatorStateInfo(3).IsName("Attack2"))
                {
                    _animator.SetBool("Attack2", false);
                    _animator.SetBool("Attack", true);
                    SoundManager.instance.PlaySfx(SoundManager.Sfx.PlayerPunch);
                }
                else
                {
                    _animator.SetBool("Attack", true);
                    SoundManager.instance.PlaySfx(SoundManager.Sfx.PlayerPunch);
                }
            }

            isAttacking = true;

            if (punchCollider != null && punchCollider.CompareTag("Hand"))
            {
                punchCollider.enabled = true;
            }

            StartCoroutine(DisablePunchColliderAfterDelay(0.5f));
        }
    }

    IEnumerator DisablePunchColliderAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (punchCollider != null && punchCollider.CompareTag("Hand"))
        {
            punchCollider.enabled = false;
        }
        _animator.SetBool("Attack", false);
        _animator.SetBool("Attack2", false);
        _animator.SetBool("FlyingAttack", false);
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isAttacking)
        {
            PhotonView otherPhotonView = other.GetComponentInParent<PhotonView>();

            if (otherPhotonView == null)
            {
                return;
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !otherPhotonView.IsMine)
            {
                float pushForce;
                if (playerMoveAbility._isRunning)
                {
                    pushForce = 4f;
                    
                }
                else if (_animator.GetCurrentAnimatorStateInfo(3).IsName("Attack2"))
                {
                    pushForce = 2.5f;
                }
                else
                {
                    pushForce = 1.5f;
                }
                SoundManager.instance.PlaySfx(SoundManager.Sfx.PlayerDamages);
                Vector3 pushDirection = (other.transform.position - transform.position).normalized;
                otherPhotonView.RPC("ApplyPushForce", RpcTarget.AllBuffered, pushDirection, pushForce);
            }
        }
    }

    [PunRPC]
    public void ApplyPushForce(Vector3 pushDirection, float force)
    {
        StartCoroutine(ApplyPushForceCoroutine(pushDirection, force));
    }

    private IEnumerator ApplyPushForceCoroutine(Vector3 pushDirection, float force)
    {
        Rigidbody targetRigidbody = GetComponentInParent<Rigidbody>();

        if (targetRigidbody != null)
        {
            float duration = 0.5f;
            float elapsedTime = 0f;
            Vector3 initialPosition = transform.position;
            Vector3 targetPosition = initialPosition + pushDirection * force;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                targetRigidbody.MovePosition(Vector3.Lerp(initialPosition, targetPosition, t));
                yield return null;
            }
        }
    }
}
